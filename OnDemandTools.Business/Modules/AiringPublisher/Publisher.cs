﻿using EasyNetQ;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Business.Modules.AiringPublisher.Models;
using OnDemandTools.Business.Modules.AiringPublisher.Validating;
using OnDemandTools.Business.Modules.AiringPublisher.Validating.Validators;
using OnDemandTools.Business.Modules.AiringPublisher.Workflow;
using OnDemandTools.Business.Modules.Queue;
using OnDemandTools.Common.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLAiring = OnDemandTools.Business.Modules.Airing.Model;
using BLQueue = OnDemandTools.Business.Modules.Queue.Model;

namespace OnDemandTools.Business.Modules.AiringPublisher
{
    public class Publisher : IPublisher
    {
        StringBuilder jobLogs = new StringBuilder();
        private readonly Serilog.ILogger logger;
        private readonly AppSettings appsettings;
        private readonly string processId;
        private readonly IQueueService queueService;
        private readonly IAiringService airingService;
        private readonly IEnvelopeDistributor envelopeDistributor;
        private readonly IEnvelopeStuffer envelopeStuffer;
        private readonly IQueueReporterService reportStatusCommand;
        private readonly IMessageDeliveryValidator messageDeliveryValidator;
        private readonly IAiringValidatorStep bimContentValidator;
        private readonly IAiringValidatorStep mediaIdValidator;
        private const int BIMFOUND = 17;
        private const int BIMNOTFOUND = 18;
        private const int BIMMISMATCH = 19;

        public const int Limit = 1000;

        public Publisher(
            Serilog.ILogger logger,
            AppSettings appsettings,
            IQueueService queueService,
            IAiringService airingService,
            IEnvelopeDistributor envelopeDistributor,
            IEnvelopeStuffer envelopeStuffer,
            IQueueReporterService reportStatusCommand,
            IMessageDeliveryValidator messageDeliveryValidator,
            BimContentValidator bimContentValidator,
            MediaIdValidator mediaIdValidator)
        {
            this.logger = logger;
            this.queueService = queueService;
            this.airingService = airingService;
            processId = GetProcessId();
            this.envelopeDistributor = envelopeDistributor;
            this.envelopeStuffer = envelopeStuffer;
            this.reportStatusCommand = reportStatusCommand;
            this.messageDeliveryValidator = messageDeliveryValidator;
            this.bimContentValidator = bimContentValidator;
            this.mediaIdValidator = mediaIdValidator;
            this.appsettings = appsettings;
        }

        public void Execute(string queueName)
        {
            try
            {
                LogInformation(string.Format("started publisher job for queue: {0} and processId: {1}", queueName, processId));

                var queue = queueService.GetByApiKey(queueName);

                if (queue == null || !queue.Active)
                {
                    LogInformation(string.Format("No Active queue found for queue name: {0}", queueName));
                    return;
                }

                bool lockAquired = false;

                try
                {
                    LogInformation(string.Format("Acquiring lock on queue {0}; Queue Name: {1}, process Id: {2}", queue.FriendlyName, queueName, processId));

                    if (queueService.Lock(queue.Name, processId))
                    {
                        lockAquired = true;

                        LogInformation("Acquired lock on queue");

                        // Proceed to processing the queue                    
                        LogInformation("Hard core processing on queue started");
                        ProcessQueue(queue);
                        LogInformation("Successfully completed hard core processing on queue");

                        // Set last process time for the queue
                        LogInformation("Setting queue last processed time");
                        queueService.UpdateQueueProcessedTime(queue.Name);
                        LogInformation("Successfully set queue last processed time");
                    }
                    else
                    {
                        LogInformation("Couldn't acquire lock on queue. It is locked by another process.");
                    }

                    LogInformation("Successfully completed operation on queue");
                }
                catch (Exception ex)
                {
                    LogInformation(string.Format("Abruptly stopped operation on queue. Exception: {0}", ex.Message));
                    LogError(ex, "Abruptly stopped operation on queue", queue);
                    throw;
                }
                finally
                {
                    if (lockAquired)
                    {
                        // Release lock on the queue
                        LogInformation("Removing lock on queue");
                        queueService.Unlock(queue.Name, processId);
                        LogInformation("Successfully removed lock on queue");
                    }
                }


                LogInformation("Publisher job completed for queue:" + queueName);
            }
            finally
            {
                logger.Information(jobLogs.ToString());
            }

        }

        private void LogInformation(string message)
        {
            jobLogs.AppendWithTime(message);
        }

        private void LogError(Exception exception, string message, BLQueue.Queue queue)
        {
            logger.Error(exception, string.Format("{0}. QueueName: {1}  Queue: {2}, Process Id: {3}", message, queue.Name, queue.FriendlyName, processId));
        }

        private string GetProcessId()
        {
            return Environment.MachineName;
        }

        /// <summary>
        /// Processes the queue.
        /// </summary>
        /// <param name="queue">The queue.</param>
        private void ProcessQueue(BLQueue.Queue queue)
        {
            LogInformation("Retrieving current airings that should be send to queue");
            var currentAirings = GetCurrentAirings(queue, Limit);
            LogInformation(string.Format("Successfully retrieved {0} current airings", currentAirings.Count));


            LogInformation("Retrieving deleted airings that should be send to queue");
            var deletedAirings = GetDeletedAirings(queue, Limit);
            LogInformation(string.Format("Successfully retrieved {0} deleted airings", deletedAirings.Count));


            LogInformation("Applying validation on all current airings based on queue settings");
            var validAirings = ValidateAirings(queue, currentAirings);
            LogInformation(string.Format("Completed validation on all {0} current airings, resulting in a total of {1} valid current airings", currentAirings.Count, validAirings.Count));


            LogInformation("Applying validation on all deleted airings");

            var validDeletedAirings = ValidateDeletedAirings(queue, deletedAirings);
            LogInformation(string.Format("Completed validation on all {0} deleted airings, resulting in a total of {1} valid deleted airings", deletedAirings.Count, validDeletedAirings.Count));


            if (!(validAirings.Any() || validDeletedAirings.Any()))
            {
                LogInformation("No Airings found to distribute");
                return;
            }

            LogInformation("Preparing to distribute current and deleted airings to queue");

            var envelopes = envelopeStuffer.Generate(validAirings, queue, Workflow.Action.Modify);
            envelopes.AddRange(envelopeStuffer.Generate(validDeletedAirings, queue, Workflow.Action.Delete));
            Distribute(queue, envelopes);
            LogInformation("Successfully distributed current and deleted airings to queue");
        }

        private void Distribute(BLQueue.Queue queue, List<Envelope> envelopes)
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.Uri = appsettings.CloudQueue.MqUrl;

            using (IConnection conn = factory.CreateConnection())
            {
                try
                {
                    using (IModel channel = conn.CreateModel())
                    {
                        try
                        {
                            channel.ExchangeDeclare(appsettings.CloudQueue.MqExchange, ExchangeType.Direct, true);
                            var deliveryDetails = new DeliveryDetails
                            {
                                RabbitMqChannel = channel,
                                ExchangeName = appsettings.CloudQueue.MqExchange
                            };

                            envelopeDistributor.Distribute(envelopes, queue, deliveryDetails, jobLogs);

                        }
                        finally
                        {
                            channel.Close();
                        }

                    }
                }
                finally
                {
                    conn.Close();

                }

            }

        }

        private List<BLAiring.Airing> GetDeletedAirings(BLQueue.Queue queue, int limit)
        {
            var deletedAirings = new List<BLAiring.Airing>();
            deletedAirings.AddRange(airingService.GetBy(queue.Query, queue.HoursOut, new[] { queue.Name }, true, AiringCollection.DeletedCollection).ToList());
            deletedAirings.AddRange(airingService.GetDeliverToBy(queue.Name, limit, AiringCollection.DeletedCollection).ToList());
            return deletedAirings.Distinct(new AiringComparer()).ToList();
        }

        private List<BLAiring.Airing> GetCurrentAirings(BLQueue.Queue queue, int limit)
        {
            var currentAirings = new List<BLAiring.Airing>();
            currentAirings.AddRange(airingService.GetBy(queue.Query, queue.HoursOut, new[] { queue.Name }).ToList());
            currentAirings.AddRange(airingService.GetDeliverToBy(queue.Name, limit).ToList());
            return currentAirings.Distinct(new AiringComparer()).ToList();
        }

        private List<BLAiring.Airing> ValidateAirings(BLQueue.Queue queue, IEnumerable<BLAiring.Airing> airings)
        {
            var validators = LoadValidators(queue);

            var ignoreAirings = new List<string>();

            if (!validators.Any())
                return airings.ToList();

            var validator = new Validating.AiringValidator(validators);
            var validAirings = new List<BLAiring.Airing>();

            foreach (var airing in airings)
            {
                var results = validator.Validate(airing, queue.Name);
                if (results.All(r => r.Valid))
                    validAirings.Add(airing);
                else if (results.Any(r => r.IgnoreQueue))
                    ignoreAirings.Add(airing.AssetId);

                foreach (var result in results.Where(r => !r.Valid && (r.StatusEnum != BIMNOTFOUND && r.StatusEnum != BIMMISMATCH)))
                {
                    // Append additional useful information
                    string message = "Queue validation error. ";
                    message += result.Message;
                    LogInformation(string.Format("Validation error. {0} - {1}", airing.AssetId, result.Message));
                    reportStatusCommand.Report(queue, airing.AssetId, true, message, result.StatusEnum, true);
                }
                if (queue.BimRequired)
                {
                    SendBIMStatus(results, airing, queue);
                }

            }

            if (ignoreAirings.Any())
            {
                UpdateIgnoreQueues(queue.Name, ignoreAirings);
            }

            return validAirings;
        }


        private void SendBIMStatus(IList<ValidationResult> results, BLAiring.Airing airing, BLQueue.Queue queue)
        {
            var bimFoundResult = results.Where(r => r.Valid && r.StatusEnum == BIMFOUND).FirstOrDefault();
            var bimNotFoundResult = results.Where(r => !r.Valid && r.StatusEnum == BIMNOTFOUND).FirstOrDefault();
            var bimisMatch = results.Where(r => !r.Valid && r.StatusEnum == BIMMISMATCH).FirstOrDefault();
            if (bimFoundResult != null)
            {
                // LogInformation(string.Format("BIM  Found. {0} - {1}", airing.AssetId, bimFoundResult.Message));
                reportStatusCommand.BimReport(queue, airing.AssetId, true, bimFoundResult.Message, bimFoundResult.StatusEnum);
            }
            if (bimNotFoundResult != null)
            {
                //  LogInformation(string.Format("BIM  Not Found. {0} - {1}", airing.AssetId, bimNotFoundResult.Message));
                reportStatusCommand.BimReport(queue, airing.AssetId, true, bimNotFoundResult.Message, bimNotFoundResult.StatusEnum);
            }

            if (bimisMatch != null)
            {
                //  LogInformation(string.Format("BIM  Mismatch. {0} - {1}", airing.AssetId, bimisMatch.Message));
                reportStatusCommand.BimReport(queue, airing.AssetId, true, bimisMatch.Message, bimisMatch.StatusEnum);
            }
        }

        private void UpdateIgnoreQueues(string queueName, IEnumerable<string> ignoreAirings, bool isDeleted = false)
        {
            foreach (var airingId in ignoreAirings)
            {
                if (isDeleted)
                    airingService.PushIgnoredQueueTo(airingId, queueName, AiringCollection.DeletedCollection);
                else
                    airingService.PushIgnoredQueueTo(airingId, queueName);
            }
        }

        private List<BLAiring.Airing> ValidateDeletedAirings(BLQueue.Queue queue, List<BLAiring.Airing> airings)
        {
            var validAirings = new List<BLAiring.Airing>();
            if (!airings.Any()) return validAirings;

            var invalidAirings = new List<string>();

            foreach (var airing in airings)
            {
                //Delete message will be delivered only when there is any modified message delivered for the same queue.
                if (messageDeliveryValidator.Validate(airing, queue.Name))
                {
                    validAirings.Add(airing);
                }
                else
                {
                    LogInformation(string.Format("Validation error. {0} - No 'Modify' action message delivered to the queue", airing.AssetId));
                    invalidAirings.Add(airing.AssetId);
                }
            }


            if (invalidAirings.Any())
            {
                UpdateIgnoreQueues(queue.Name, invalidAirings, true);
            }

            return validAirings;
        }

        private List<IAiringValidatorStep> LoadValidators(BLQueue.Queue queue)
        {
            var validators = new List<IAiringValidatorStep>();

            if (queue.BimRequired)
                validators.Add(bimContentValidator);

            // Verify if queue is configured to receive
            // airings without versions. If not, apply validation.
            if (!queue.AllowAiringsWithNoVersion)
            {
                validators.Add(new VersionValidator());
            }

            if (queue.IsProhibitResendMediaId)
            {
                validators.Add(mediaIdValidator);
            }

            return validators;
        }
    }
}
