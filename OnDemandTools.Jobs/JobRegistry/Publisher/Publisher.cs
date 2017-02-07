using OnDemandTools.Business.Modules.Queue;
using System;
using System.Collections.Generic;
using System.Threading;

namespace OnDemandTools.Jobs.JobRegistry.Publisher
{
    public class Publisher
    {
        //resolve all concrete implementations in constructor        
        Serilog.ILogger logger;
        IQueueService queueService;

        public Publisher(Serilog.ILogger logger, IQueueService queueService)
        {
            this.logger = logger;
            this.queueService = queueService;
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

            var processId = GetProcessId();

            try
            {
                logger.Information("Acquiring lock on queue {0}; Queue Name: {1}, process Id", queue.FriendlyName, queueName, processId);

                // Critical section within threads
                DeliveryQueueLock qLock;
                lock (threadLock)
                {
                    qLock = _queueLocker.AquireLockFor(queue.Name, processId);
                }

                // Verify if the queue currently has a lock. If so, don't do anything; else, proceed with processing the queue
                if (qLock.IsLockedBy(processId))
                {
                    LogInformation("Acquiring lock on queue", queue, processId);

                    // Proceed to processing the queue                    
                    LogInformation("Hard core processing on queue started", queue, processId);
                    ProcessQueue(queue, details);
                    LogInformation("Successfully completed hard core processing on queue", queue, processId);

                    // Set last process time for the queue
                    LogInformation("Setting queue last processed time", queue, processId);
                    _markProcessedCommand.UpdateQueueProcessedTime(queue.Name);
                    LogInformation("Successfully set queue last processed time", queue, processId);
                }
                else
                {
                    LogInformation(string.Format("Couldn't acquire lock on queue. It is locked by process {0}", qLock.ProcessId), queue, processId);
                }

                LogInformation("Successfully completed operation on queue", queue, processId);
            }
            catch (Exception ex)
            {
                LogError(ex, "Abruptly stopped operation on queue", queue, processId);
                throw;
            }
            finally
            {
                // Release lock on the queue
                LogInformation("Removing lock on queue", queue, processId);
                _queueLocker.ReleaseLockFor(queue.Name, processId);
                LogInformation("Successfully removed lock on queue", queue, processId);
            }


            logger.Information("Publisher job completed for queue:" + queueName);
        }

        private void LogInformation(string message, Business.Modules.Queue.Model.Queue queue, string processId)
        {
            logger.Information("{0}. QueueName: {1}  Queue: {2}, Process Id: {3}", message, queue.Name, queue.FriendlyName, processId);
        }

        private void LogError(Exception exception, string message, Business.Modules.Queue.Model.Queue queue, string processId)
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
        private void ProcessQueue(DeliveryQueue queue, DeliveryDetails details)
        {
            pbLogger.Trace("Agent:{0}-Job:{1}-Thread:{2}-Queue:{3}: Retrieving current airings that should be send to queue", details.Agent.AgentId, details.Job.JobName, Thread.CurrentThread.ManagedThreadId, queue.FriendlyName);
            var currentAirings = GetCurrentAirings(queue, details.Limit);
            pbLogger.Trace("Agent:{0}-Job:{1}-Thread:{2}-Queue:{3}: Successfully retrieved {4} current airings", details.Agent.AgentId, details.Job.JobName, Thread.CurrentThread.ManagedThreadId, queue.FriendlyName, currentAirings.Count);

            pbLogger.Trace("Agent:{0}-Job:{1}-Thread:{2}-Queue:{3}: Retrieving deleted airings that should be send to queue", details.Agent.AgentId, details.Job.JobName, Thread.CurrentThread.ManagedThreadId, queue.FriendlyName);
            var deletedAirings = GetDeletedAirings(queue, details.Limit);
            pbLogger.Trace("Agent:{0}-Job:{1}-Thread:{2}-Queue:{3}: Successfully retrieved {4} deleted airings", details.Agent.AgentId, details.Job.JobName, Thread.CurrentThread.ManagedThreadId, queue.FriendlyName, deletedAirings.Count);

            pbLogger.Trace("Agent:{0}-Job:{1}-Thread:{2}-Queue:{3}: Applying validation on all {4} current airings based on queue settings", details.Agent.AgentId, details.Job.JobName, Thread.CurrentThread.ManagedThreadId, queue.FriendlyName, currentAirings.Count);
            var validAirings = ValidateAirings(queue, currentAirings, details);
            pbLogger.Trace("Agent:{0}-Job:{1}-Thread:{2}-Queue:{3}: Completed validation on all {4} current airings, resulting in a total of {5} valid current airings", details.Agent.AgentId, details.Job.JobName, Thread.CurrentThread.ManagedThreadId, queue.FriendlyName, currentAirings.Count, validAirings.Count);

            pbLogger.Trace("Agent:{0}-Job:{1}-Thread:{2}-Queue:{3}: Applying validation on all {4} deleted airings", details.Agent.AgentId, details.Job.JobName, Thread.CurrentThread.ManagedThreadId, queue.FriendlyName, currentAirings.Count);
            var validDeletedAirings = ValidateDeletedAirings(queue, deletedAirings, details);
            pbLogger.Trace("Agent:{0}-Job:{1}-Thread:{2}-Queue:{3}: Completed validation on all {4} deleted airings, resulting in a total of {5} valid deleted airings", details.Agent.AgentId, details.Job.JobName, Thread.CurrentThread.ManagedThreadId, queue.FriendlyName, deletedAirings.Count, validDeletedAirings.Count);

            if (validAirings.Any())
            {
                //Creates Queue/Binding if not exists
                pbLogger.Trace("Agent:{0}-Job:{1}-Thread:{2}-Queue:{3}: Queue setup - started", details.Agent.AgentId, details.Job.JobName, Thread.CurrentThread.ManagedThreadId, queue.FriendlyName);
                _remoteQueueHandler.Create(queue);
                pbLogger.Trace("Agent:{0}-Job:{1}-Thread:{2}-Queue:{3}: Queue setup - completed", details.Agent.AgentId, details.Job.JobName, Thread.CurrentThread.ManagedThreadId, queue.FriendlyName);
            }

            pbLogger.Trace("Agent:{0}-Job:{1}-Thread:{2}-Queue:{3}: Preparing to distribute airings current:{4} deleted:{5} to queue", details.Agent.AgentId, details.Job.JobName, Thread.CurrentThread.ManagedThreadId, queue.FriendlyName, validAirings.Count, deletedAirings.Count);
            var envelopes = _envelopeStuffer.Generate(validAirings, queue, Action.Modify);
            envelopes.AddRange(_envelopeStuffer.Generate(validDeletedAirings, queue, Action.Delete));
            _envelopeDistributor.Distribute(envelopes, queue, details);
            pbLogger.Trace("Agent:{0}-Job:{1}-Thread:{2}-Queue:{3}: Successfully distributed airings current:{4} deleted:{5} to queue", details.Agent.AgentId, details.Job.JobName, Thread.CurrentThread.ManagedThreadId, queue.FriendlyName, validAirings.Count, deletedAirings.Count);

            pbLogger.Trace("Agent:{0}-Job:{1}-Thread:{2}-Queue:{3}: Update delivery status of all distributed airings", details.Agent.AgentId, details.Job.JobName, Thread.CurrentThread.ManagedThreadId, queue.FriendlyName);
            UpdateDeliveredTo(validAirings, validDeletedAirings, queue.Name);
            pbLogger.Trace("Agent:{0}-Job:{1}-Thread:{2}-Queue:{3}: Successfully updated delivery status of all distributed airings", details.Agent.AgentId, details.Job.JobName, Thread.CurrentThread.ManagedThreadId, queue.FriendlyName);
        }

        private List<Airing> GetDeletedAirings(DeliveryQueue queue, int limit)
        {
            var deletedAirings = new List<Airing>();
            deletedAirings.AddRange(_deletedAiringsQuery.GetBy(queue.Query, queue.HoursOut, new[] { queue.Name }, true).ToList());
            deletedAirings.AddRange(_deletedAiringsQuery.GetDeliverToBy(queue.Name, limit).ToList());
            return deletedAirings.Distinct(new AiringComparer()).ToList();
        }

        private List<Airing> GetCurrentAirings(DeliveryQueue queue, int limit)
        {
            var currentAirings = new List<Airing>();
            currentAirings.AddRange(_currentAiringsQuery.GetBy(queue.Query, queue.HoursOut, new[] { queue.Name }).ToList());
            currentAirings.AddRange(_currentAiringsQuery.GetDeliverToBy(queue.Name, limit).ToList());
            return currentAirings.Distinct(new AiringComparer()).ToList();
        }

        private List<Airing> ValidateAirings(DeliveryQueue queue, IEnumerable<Airing> airings, DeliveryDetails details)
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
                    pbLogger.Trace("Agent:{0}-Job:{1}-Thread:{2}-Queue:{3}: Validation error. {4} - {5}", details.Agent.AgentId, details.Job.JobName, Thread.CurrentThread.ManagedThreadId, queue.FriendlyName, airing.AssetId, result.Message);
                    pbLogger.Info("Agent:{0}-Job:{1}-Thread:{2}-Queue:{3}: Validation error. {4} - {5}", details.Agent.AgentId, details.Job.JobName, Thread.CurrentThread.ManagedThreadId, queue.FriendlyName, airing.AssetId, result.Message);
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
                pbLogger.Trace("Agent:{0}-Job:{1}-Thread:{2}-Queue:{3}:BIM  Found. {4} - {5}", details.Agent.AgentId, details.Job.JobName, Thread.CurrentThread.ManagedThreadId, queue.FriendlyName, airing.AssetId, bimFoundResult.Message);
                pbLogger.Info("Agent:{0}-Job:{1}-Thread:{2}-Queue:{3}:BIM  Found. {4} - {5}", details.Agent.AgentId, details.Job.JobName, Thread.CurrentThread.ManagedThreadId, queue.FriendlyName, airing.AssetId, bimFoundResult.Message);
                _reportStatusCommand.BimReport(queue, airing.AssetId, bimFoundResult.Message, bimFoundResult.StatusEnum);
            }
            if (bimNotFoundResult != null)
            {
                pbLogger.Trace("Agent:{0}-Job:{1}-Thread:{2}-Queue:{3}:BIM Not Found. {4} - {5}", details.Agent.AgentId, details.Job.JobName, Thread.CurrentThread.ManagedThreadId, queue.FriendlyName, airing.AssetId, bimNotFoundResult.Message);
                pbLogger.Info("Agent:{0}-Job:{1}-Thread:{2}-Queue:{3}:BIM Not Found. {4} - {5}", details.Agent.AgentId, details.Job.JobName, Thread.CurrentThread.ManagedThreadId, queue.FriendlyName, airing.AssetId, bimNotFoundResult.Message);
                _reportStatusCommand.BimReport(queue, airing.AssetId, bimNotFoundResult.Message, bimNotFoundResult.StatusEnum);
            }

            if (bimisMatch != null)
            {
                pbLogger.Trace("Agent:{0}-Job:{1}-Thread:{2}-Queue:{3}:BIM Mismatch. {4} - {5}", details.Agent.AgentId, details.Job.JobName, Thread.CurrentThread.ManagedThreadId, queue.FriendlyName, airing.AssetId, bimisMatch.Message);
                pbLogger.Info("Agent:{0}-Job:{1}-Thread:{2}-Queue:{3}:BIM Mismatch. {4} - {5}", details.Agent.AgentId, details.Job.JobName, Thread.CurrentThread.ManagedThreadId, queue.FriendlyName, airing.AssetId, bimisMatch.Message);
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

        private List<Airing> ValidateDeletedAirings(DeliveryQueue queue, List<Airing> airings, DeliveryDetails details)
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
                    pbLogger.Info("Agent:{0}-Job:{1}-Thread:{2}-Queue:{3}: Validation error. {4} - No 'Modify' action message delivered to the queue", details.Agent.AgentId, details.Job.JobName, Thread.CurrentThread.ManagedThreadId, queue.FriendlyName, airing.AssetId);
                    invalidArings.Add(airing.AssetId);
                }
            }


            if (invalidArings.Any())
            {
                UpdateIgnoreQueues(queue.Name, invalidArings, true);
            }

            return validAirings;
        }

        private List<IAiringValidatorStep> LoadValidators(DeliveryQueue queue)
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
