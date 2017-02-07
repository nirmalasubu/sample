using OnDemandTools.Business.Modules.Airing.Model;
using OnDemandTools.Business.Modules.Queue;
using OnDemandTools.Business.Modules.Queue.Model;
using OnDemandTools.DAL.Modules.Queue.Command;
using OnDemandTools.Jobs.JobRegistry.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace OnDemandTools.Jobs.JobRegistry.Publisher
{
    public class Publisher
    {
        //resolve all concrete implementations in constructor        
        Serilog.ILogger logger;
        IQueueService queueService;
        IQueueLocker queueLocker;
        string processId;

        public Publisher(
            Serilog.ILogger logger,
            IQueueService queueService,
            IQueueLocker queueLocker)
        {
            this.logger = logger;
            this.queueService = queueService;
            this.queueLocker = queueLocker;
            var processId = GetProcessId();
        }

        public void Execute(string queueName)
        {
            logger.Information("started publisher job for queue:" + queueName);

            var queue = queueService.GetByApiKey(queueName);

            if (queue != null && !queue.Active)
            {
                logger.Information("No Active queue found for queue name: {0}", queueName);
                return;
            }

            try
            {
                logger.Information("Acquiring lock on queue {0}; Queue Name: {1}, process Id", queue.FriendlyName, queueName, processId);

                // Critical section within threads
                DeliveryQueueLock qLock;

                qLock = queueLocker.AquireLockFor(queue.Name, processId);


                // Verify if the queue currently has a lock. If so, don't do anything; else, proceed with processing the queue
                if (qLock.IsLockedBy(processId))
                {
                    var deliveryDetails = SetupDeliveryDetails();

                    LogInformation("Acquiring lock on queue", queue);

                    // Proceed to processing the queue                    
                    LogInformation("Hard core processing on queue started", queue);
                    ProcessQueue(queue, deliveryDetails);
                    LogInformation("Successfully completed hard core processing on queue", queue);

                    // Set last process time for the queue
                    LogInformation("Setting queue last processed time", queue);
                    _markProcessedCommand.UpdateQueueProcessedTime(queue.Name);
                    LogInformation("Successfully set queue last processed time", queue);
                }
                else
                {
                    LogInformation(string.Format("Couldn't acquire lock on queue. It is locked by process {0}", qLock.ProcessId), queue);
                }

                LogInformation("Successfully completed operation on queue", queue);
            }
            catch (Exception ex)
            {
                LogError(ex, "Abruptly stopped operation on queue", queue);
                throw;
            }
            finally
            {
                // Release lock on the queue
                LogInformation("Removing lock on queue", queue);
                queueLocker.ReleaseLockFor(queue.Name, processId);
                LogInformation("Successfully removed lock on queue", queue);
            }


            logger.Information("Publisher job completed for queue:" + queueName);
        }

        private DeliveryDetails SetupDeliveryDetails()
        {
            return new DeliveryDetails();
        }

        private void LogInformation(string message, Queue queue)
        {
            logger.Information("{0}. QueueName: {1}  Queue: {2}, Process Id: {3}", message, queue.Name, queue.FriendlyName, processId);
        }

        private void LogError(Exception exception, string message, Queue queue)
        {
            logger.Error(exception, "{0}. QueueName: {1}  Queue: {2}, Process Id: {3}", message, queue.Name, queue.FriendlyName, processId);
        }

        private string GetProcessId()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Processes the queue.
        /// </summary>
        /// <param name="queue">The queue.</param>
        /// <param name="details">The details.</param>
        private void ProcessQueue(Queue queue, DeliveryDetails details)
        {
            LogInformation("Retrieving current airings that should be send to queue", queue);
            var currentAirings = GetCurrentAirings(queue, details.Limit);
            LogInformation(string.Format("Successfully retrieved {0} current airings", currentAirings.Count), queue);


            LogInformation("Retrieving deleted airings that should be send to queue", queue);
            var deletedAirings = GetDeletedAirings(queue, details.Limit);
            LogInformation(string.Format("Successfully retrieved {0} deleted airings", deletedAirings.Count), queue);


            LogInformation("Applying validation on all current airings based on queue settings", queue);
            var validAirings = ValidateAirings(queue, currentAirings, details);
            LogInformation(string.Format("Completed validation on all {0} current airings, resulting in a total of {1} valid current airings", currentAirings.Count, validAirings.Count), queue);


            LogInformation("Applying validation on all deleted airings", queue);

            var validDeletedAirings = ValidateDeletedAirings(queue, deletedAirings, details);
            LogInformation(string.Format("Completed validation on all {0} deleted airings, resulting in a total of {1} valid deleted airings", deletedAirings.Count, validDeletedAirings.Count), queue);


            if (validAirings.Any())
            {
                //Creates Queue/Binding if not exists
                LogInformation("Queue setup - started", queue);
                _remoteQueueHandler.Create(queue);
                LogInformation("Queue setup - completed", queue);
            }

            LogInformation("Preparing to distribute current and deleted airings to queue", queue);

            var envelopes = _envelopeStuffer.Generate(validAirings, queue, Action.Modify);
            envelopes.AddRange(_envelopeStuffer.Generate(validDeletedAirings, queue, Action.Delete));
            _envelopeDistributor.Distribute(envelopes, queue, details);
            LogInformation("Successfully distributed current and deleted airings to queue", queue);


            LogInformation("Update delivery status of all distributed airings", queue);

            UpdateDeliveredTo(validAirings, validDeletedAirings, queue.Name);
            LogInformation("Successfully updated delivery status of all distributed airings", queue);
        }

        private List<Airing> GetDeletedAirings(Queue queue, int limit)
        {
            var deletedAirings = new List<Airing>();
            deletedAirings.AddRange(_deletedAiringsQuery.GetBy(queue.Query, queue.HoursOut, new[] { queue.Name }, true).ToList());
            deletedAirings.AddRange(_deletedAiringsQuery.GetDeliverToBy(queue.Name, limit).ToList());
            return deletedAirings.Distinct(new AiringComparer()).ToList();
        }

        private List<Airing> GetCurrentAirings(Queue queue, int limit)
        {
            var currentAirings = new List<Airing>();
            currentAirings.AddRange(_currentAiringsQuery.GetBy(queue.Query, queue.HoursOut, new[] { queue.Name }).ToList());
            currentAirings.AddRange(_currentAiringsQuery.GetDeliverToBy(queue.Name, limit).ToList());
            return currentAirings.Distinct(new AiringComparer()).ToList();
        }

        private List<Airing> ValidateAirings(Queue queue, IEnumerable<Airing> airings, DeliveryDetails details)
        {
            var validators = LoadValidators(queue);

            var ignoreAirings = new List<string>();

            if (!validators.Any())
                return airings.ToList();

            var validator = new AiringValidator(validators);
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
                    LogInformation(string.Format("Validation error. {0} - {1}", airing.AssetId, result.Message), queue);
                    _reportStatusCommand.Report(queue, airing.AssetId, message, result.StatusEnum, true);
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


        private void SendBIMStatus(IList<ValidationResult> results, Airing airing, DeliveryQueue queue, DeliveryDetails details)
        {
            var bimFoundResult = results.Where(r => r.Valid && r.StatusEnum == BIMFOUND).FirstOrDefault();
            var bimNotFoundResult = results.Where(r => !r.Valid && r.StatusEnum == BIMNOTFOUND).FirstOrDefault();
            var bimisMatch = results.Where(r => !r.Valid && r.StatusEnum == BIMMISMATCH).FirstOrDefault();
            if (bimFoundResult != null)
            {
                LogInformation(string.Format("BIM  Found. {0} - {1}", airing.AssetId, bimFoundResult.Message), queue);
                _reportStatusCommand.BimReport(queue, airing.AssetId, bimFoundResult.Message, bimFoundResult.StatusEnum);
            }
            if (bimNotFoundResult != null)
            {
                LogInformation(string.Format("BIM  Found. {0} - {1}", airing.AssetId, bimFoundResult.Message), queue);
                _reportStatusCommand.BimReport(queue, airing.AssetId, bimNotFoundResult.Message, bimNotFoundResult.StatusEnum);
            }

            if (bimisMatch != null)
            {
                LogInformation(string.Format("BIM  Mismatch. {0} - {1}", airing.AssetId, bimFoundResult.Message), queue);
                _reportStatusCommand.BimReport(queue, airing.AssetId, bimisMatch.Message, bimisMatch.StatusEnum);
            }
        }

        private void UpdateIgnoreQueues(string queueName, IEnumerable<string> ignoreAirings, bool isDeleted = false)
        {
            foreach (var airingId in ignoreAirings)
            {
                if (isDeleted)
                    _updateDeletedAiringQueueDelivery.PushIgnoredQueueTo(airingId, queueName);
                else
                    _udateAiringQueueDelivery.PushIgnoredQueueTo(airingId, queueName);
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
                if (_messageDeliveryValidator.Validate(airing, queue.Name))
                {
                    validAirings.Add(airing);
                }
                else
                {
                    LogInformation(string.Format("Validation error. {0} - No 'Modify' action message delivered to the queue", airing.AssetId), queue);
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
                validators.Add(_bimContentValidator);

            // Verify if queue is configured to receive
            // airings without versions. If not, apply validation.
            if (!queue.AllowAiringsWithNoVersion)
            {
                validators.Add(new VersionValidator());
            }

            if (queue.IsProhibitResendMediaId)
            {
                validators.Add(_mediaIdValidator);
            }

            return validators;
        }

        private void UpdateDeliveredTo(IEnumerable<Airing> currentAirings, IEnumerable<Airing> deleteAirings, string name)
        {
            foreach (var airing in currentAirings)
            {
                _udateAiringQueueDelivery.PushDeliveredTo(airing.AssetId, name);
            }

            foreach (var airing in deleteAirings)
            {
                _updateDeletedAiringQueueDelivery.PushDeliveredTo(airing.AssetId, name);
            }
        }
    }
}
