using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Business.Modules.Queue;
using OnDemandTools.Business.Modules.Queue.Model;
using OnDemandTools.DAL.Modules.Airings.Commands;
using OnDemandTools.DAL.Modules.Airings.Model;
using OnDemandTools.DAL.Modules.Airings.Queries;
using OnDemandTools.DAL.Modules.Queue.Command;
using OnDemandTools.DAL.Modules.Reporting.Command;
using OnDemandTools.Jobs.JobRegistry.Models;
using OnDemandTools.Jobs.JobRegistry.Publisher.Validating;
using OnDemandTools.Jobs.JobRegistry.Publisher.Validating.Validators;
using OnDemandTools.Jobs.JobRegistry.Publisher.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnDemandTools.Jobs.JobRegistry.Publisher
{
    public class Publisher
    {
        StringBuilder jobLogs = new StringBuilder();

        //resolve all concrete implementations in constructor        
        private readonly Serilog.ILogger logger;
        private readonly string processId;
        private readonly IQueueService queueService;
        private readonly IQueueLocker queueLocker;
        private readonly CurrentAiringsQuery currentAiringsQuery;
        private readonly DeletedAiringsQuery deletedAiringsQuery;
        private readonly IEnvelopeDistributor envelopeDistributor;
        private readonly IEnvelopeStuffer envelopeStuffer;
        private readonly IQueueReporter reportStatusCommand;
        private readonly IUpdateAiringQueueDelivery updateAiringQueueDelivery;
        private readonly IUpdateDeletedAiringQueueDelivery updateDeletedAiringQueueDelivery;
        private readonly IMessageDeliveryValidator messageDeliveryValidator;
        private readonly IAiringValidatorStep bimContentValidator;
        private readonly IAiringValidatorStep mediaIdValidator;

        private const int BIMFOUND = 17;
        private const int BIMNOTFOUND = 18;
        private const int BIMMISMATCH = 19;

        public Publisher(
            Serilog.ILogger logger,
            IQueueService queueService,
            IQueueLocker queueLocker,
            CurrentAiringsQuery currentAiringsQuery,
            DeletedAiringsQuery deletedAiringsQuery,
            IEnvelopeDistributor envelopeDistributor,
            IEnvelopeStuffer envelopeStuffer,
            IQueueReporter reportStatusCommand,
            IUpdateAiringQueueDelivery updateAiringQueueDelivery,
            IUpdateDeletedAiringQueueDelivery updateDeletedAiringQueueDelivery,
            IMessageDeliveryValidator messageDeliveryValidator,
            BimContentValidator bimContentValidator,
            MediaIdValidator mediaIdValidator)
        {
            this.logger = logger;
            this.queueService = queueService;
            this.queueLocker = queueLocker;
            var processId = GetProcessId();
            this.currentAiringsQuery = currentAiringsQuery;
            this.deletedAiringsQuery = deletedAiringsQuery;
            this.envelopeDistributor = envelopeDistributor;
            this.envelopeStuffer = envelopeStuffer;
            this.reportStatusCommand = reportStatusCommand;
            this.updateAiringQueueDelivery = updateAiringQueueDelivery;
            this.updateDeletedAiringQueueDelivery = updateDeletedAiringQueueDelivery;
            this.messageDeliveryValidator = messageDeliveryValidator;
            this.bimContentValidator = bimContentValidator;
            this.mediaIdValidator = mediaIdValidator;
        }

        public void Execute(string queueName)
        {
            try
            {
                LogInformation(string.Format("started publisher job for queue: {0} and processId: {1}", queueName, processId));

                var queue = queueService.GetByApiKey(queueName);

                if (queue != null && !queue.Active)
                {
                    LogInformation(string.Format("No Active queue found for queue name: {0}", queueName));
                    return;
                }

                try
                {
                    LogInformation(string.Format("Acquiring lock on queue {0}; Queue Name: {1}, process Id: {2}", queue.FriendlyName, queueName, processId));

                    // Critical section within threads
                    DeliveryQueueLock qLock;

                    qLock = queueLocker.AquireLockFor(queue.Name, processId);


                    // Verify if the queue currently has a lock. If so, don't do anything; else, proceed with processing the queue
                    if (qLock.IsLockedBy(processId))
                    {
                        var deliveryDetails = SetupDeliveryDetails();

                        LogInformation("Acquired lock on queue");

                        // Proceed to processing the queue                    
                        LogInformation("Hard core processing on queue started");
                        ProcessQueue(queue, deliveryDetails);
                        LogInformation("Successfully completed hard core processing on queue");

                        // Set last process time for the queue
                        LogInformation("Setting queue last processed time");
                        queueService.UpdateQueueProcessedTime(queue.Name);
                        LogInformation("Successfully set queue last processed time");
                    }
                    else
                    {
                        LogInformation(string.Format("Couldn't acquire lock on queue. It is locked by process {0}", qLock.ProcessId));
                    }

                    LogInformation("Successfully completed operation on queue");
                }
                catch (Exception ex)
                {
                    LogError(ex, "Abruptly stopped operation on queue", queue);
                    throw;
                }
                finally
                {
                    // Release lock on the queue
                    LogInformation("Removing lock on queue");
                    queueLocker.ReleaseLockFor(queue.Name, processId);
                    LogInformation("Successfully removed lock on queue");
                }


                logger.Information("Publisher job completed for queue:" + queueName);
            }
            finally
            {
                logger.Information(jobLogs.ToString());
            }

        }

        private DeliveryDetails SetupDeliveryDetails()
        {
            return new DeliveryDetails();
        }

        private void LogInformation(string message)
        {
            jobLogs.Append(message);
            jobLogs.Append("\r\n");
        }

        private void LogError(Exception exception, string message, Queue queue)
        {
            logger.Error(exception, "{0}. QueueName: {1}  Queue: {2}, Process Id: {3}", message, queue.Name, queue.FriendlyName, processId);
        }

        private string GetProcessId()
        {
            return Environment.MachineName;
        }

        /// <summary>
        /// Processes the queue.
        /// </summary>
        /// <param name="queue">The queue.</param>
        /// <param name="details">The details.</param>
        private void ProcessQueue(Queue queue, DeliveryDetails details)
        {
            LogInformation("Retrieving current airings that should be send to queue");
            var currentAirings = GetCurrentAirings(queue, details.Limit);
            LogInformation(string.Format("Successfully retrieved {0} current airings", currentAirings.Count));


            LogInformation("Retrieving deleted airings that should be send to queue");
            var deletedAirings = GetDeletedAirings(queue, details.Limit);
            LogInformation(string.Format("Successfully retrieved {0} deleted airings", deletedAirings.Count));


            LogInformation("Applying validation on all current airings based on queue settings");
            var validAirings = ValidateAirings(queue, currentAirings, details);
            LogInformation(string.Format("Completed validation on all {0} current airings, resulting in a total of {1} valid current airings", currentAirings.Count, validAirings.Count));


            LogInformation("Applying validation on all deleted airings");

            var validDeletedAirings = ValidateDeletedAirings(queue, deletedAirings, details);
            LogInformation(string.Format("Completed validation on all {0} deleted airings, resulting in a total of {1} valid deleted airings", deletedAirings.Count, validDeletedAirings.Count));


            if (validAirings.Any())
            {
                //Creates Queue/Binding if not exists
                LogInformation("Queue setup - started");
                //TODO 384 Create Queue handler
                //_remoteQueueHandler.Create(queue);
                LogInformation("Queue setup - completed");
            }

            LogInformation("Preparing to distribute current and deleted airings to queue");

            var envelopes = envelopeStuffer.Generate(validAirings, queue, Action.Modify);
            envelopes.AddRange(envelopeStuffer.Generate(validDeletedAirings, queue, Action.Delete));
            envelopeDistributor.Distribute(envelopes, queue, details);
            LogInformation("Successfully distributed current and deleted airings to queue");


            LogInformation("Update delivery status of all distributed airings");

            UpdateDeliveredTo(validAirings, validDeletedAirings, queue.Name);
            LogInformation("Successfully updated delivery status of all distributed airings");
        }

        private List<Airing> GetDeletedAirings(Queue queue, int limit)
        {
            var deletedAirings = new List<Airing>();
            deletedAirings.AddRange(deletedAiringsQuery.GetBy(queue.Query, queue.HoursOut, new[] { queue.Name }, true).ToList());
            deletedAirings.AddRange(deletedAiringsQuery.GetDeliverToBy(queue.Name, limit).ToList());
            return deletedAirings.Distinct(new AiringComparer()).ToList();
        }

        private List<Airing> GetCurrentAirings(Queue queue, int limit)
        {
            var currentAirings = new List<Airing>();
            currentAirings.AddRange(currentAiringsQuery.GetBy(queue.Query, queue.HoursOut, new[] { queue.Name }).ToList());
            currentAirings.AddRange(currentAiringsQuery.GetDeliverToBy(queue.Name, limit).ToList());
            return currentAirings.Distinct(new AiringComparer()).ToList();
        }

        private List<Airing> ValidateAirings(Queue queue, IEnumerable<Airing> airings, DeliveryDetails details)
        {
            var validators = LoadValidators(queue);

            var ignoreAirings = new List<string>();

            if (!validators.Any())
                return airings.ToList();

            var validator = new Validating.AiringValidator(validators);
            var validAirings = new List<Airing>();

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
                    reportStatusCommand.Report(queue, airing.AssetId, message, result.StatusEnum, true);
                }
                if (queue.BimRequired)
                {
                    SendBIMStatus(results, airing, queue, details);
                }

            }

            if (ignoreAirings.Any())
            {
                UpdateIgnoreQueues(queue.Name, ignoreAirings);
            }

            return validAirings;
        }


        private void SendBIMStatus(IList<ValidationResult> results, Airing airing, Queue queue, DeliveryDetails details)
        {
            var bimFoundResult = results.Where(r => r.Valid && r.StatusEnum == BIMFOUND).FirstOrDefault();
            var bimNotFoundResult = results.Where(r => !r.Valid && r.StatusEnum == BIMNOTFOUND).FirstOrDefault();
            var bimisMatch = results.Where(r => !r.Valid && r.StatusEnum == BIMMISMATCH).FirstOrDefault();
            if (bimFoundResult != null)
            {
                LogInformation(string.Format("BIM  Found. {0} - {1}", airing.AssetId, bimFoundResult.Message));
                reportStatusCommand.BimReport(queue, airing.AssetId, bimFoundResult.Message, bimFoundResult.StatusEnum);
            }
            if (bimNotFoundResult != null)
            {
                LogInformation(string.Format("BIM  Found. {0} - {1}", airing.AssetId, bimFoundResult.Message));
                reportStatusCommand.BimReport(queue, airing.AssetId, bimNotFoundResult.Message, bimNotFoundResult.StatusEnum);
            }

            if (bimisMatch != null)
            {
                LogInformation(string.Format("BIM  Mismatch. {0} - {1}", airing.AssetId, bimFoundResult.Message));
                reportStatusCommand.BimReport(queue, airing.AssetId, bimisMatch.Message, bimisMatch.StatusEnum);
            }
        }

        private void UpdateIgnoreQueues(string queueName, IEnumerable<string> ignoreAirings, bool isDeleted = false)
        {
            foreach (var airingId in ignoreAirings)
            {
                if (isDeleted)
                    updateDeletedAiringQueueDelivery.PushIgnoredQueueTo(airingId, queueName);
                else
                    updateAiringQueueDelivery.PushIgnoredQueueTo(airingId, queueName);
            }
        }

        private List<Airing> ValidateDeletedAirings(Queue queue, List<Airing> airings, DeliveryDetails details)
        {
            var validAirings = new List<Airing>();
            if (!airings.Any()) return validAirings;

            var invalidArings = new List<string>();

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
                    invalidArings.Add(airing.AssetId);
                }
            }


            if (invalidArings.Any())
            {
                UpdateIgnoreQueues(queue.Name, invalidArings, true);
            }

            return validAirings;
        }

        private List<IAiringValidatorStep> LoadValidators(Queue queue)
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

        private void UpdateDeliveredTo(IEnumerable<Airing> currentAirings, IEnumerable<Airing> deleteAirings, string name)
        {
            foreach (var airing in currentAirings)
            {
                updateAiringQueueDelivery.PushDeliveredTo(airing.AssetId, name);
            }

            foreach (var airing in deleteAirings)
            {
                updateDeletedAiringQueueDelivery.PushDeliveredTo(airing.AssetId, name);
            }
        }
    }
}
