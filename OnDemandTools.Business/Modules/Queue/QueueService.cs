using OnDemandTools.Common.Model;
using OnDemandTools.DAL.Modules.Queue.Command;
using OnDemandTools.DAL.Modules.Queue.Queries;
using System.Collections.Generic;
using System.Linq;
using BLModel = OnDemandTools.Business.Modules.Queue.Model;
using DLModel = OnDemandTools.DAL.Modules.Queue.Model;
using System;
using OnDemandTools.DAL.Modules.QueueMessages;
using OnDemandTools.DAL.Modules.QueueMessages.Model;
using OnDemandTools.DAL.Modules.QueueMessages.Commands;

namespace OnDemandTools.Business.Modules.Queue
{
    public class QueueService : IQueueService
    {

        IQueueQuery queueQueryHelper;
        IQueueCommand queueCommandHelper;
        IQueueLocker queueLocker;
        IGetQueueMessagesQuery queueMessages;
        IQueueMessageRecorder historyRecorder;


        public QueueService(
            IQueueQuery queueQueryHelper,
            IQueueCommand queueCommandHelper,
            IQueueLocker queueLocker,
            IGetQueueMessagesQuery queueMessages,
            IQueueMessageRecorder historyRecorder)
        {
            this.queueQueryHelper = queueQueryHelper;
            this.queueCommandHelper = queueCommandHelper;
            this.queueLocker = queueLocker;
            this.queueMessages = queueMessages;
            this.historyRecorder = historyRecorder;
        }

        /// <summary>
        /// Flags the given list of queues for redelivery
        /// </summary>
        /// <param name="queueNames">The queue names.</param>
        /// <param name="titleIds">The title ids.</param>
        /// <param name="destinationCode">The destination code.</param>
        public void FlagForRedelivery(IList<string> queueNames, IList<int> titleIds, string destinationCode)
        {
            queueCommandHelper.ResetFor(queueNames, titleIds, destinationCode);
        }

        /// <summary>
        /// Updates Queue processing date with current time
        /// </summary>
        /// <param name="name"></param>
        public void UpdateQueueProcessedTime(string name)
        {
            queueCommandHelper.UpdateQueueProcessedTime(name);
        }


        /// <summary>
        /// Retrieves those queues that match the provided
        /// active flag (true/false)
        /// </summary>
        /// <param name="active">if set to <c>true</c> [active].</param>
        /// <returns></returns>
        public List<Model.Queue> GetByStatus(bool active)
        {
            return
            (queueQueryHelper.GetByStatus(active).ToList<DLModel.Queue>()
                .ToBusinessModel<List<DLModel.Queue>, List<BLModel.Queue>>());

        }

        /// <summary>
        /// Retrieves those queues that are subscribed to receive package notification
        /// </summary>
        /// <returns></returns>
        public List<Model.Queue> GetPackageNotificationSubscribers()
        {
            return
                (queueQueryHelper.GetPackageQueues().ToList<DLModel.Queue>()
                .ToBusinessModel<List<DLModel.Queue>, List<BLModel.Queue>>());
        }

        /// <summary>
        /// Flags the given list of queues for redelivery. Assets selected for delivery
        /// depends on 'titleIds'
        /// </summary>
        /// <param name="queueNames">The queue names.</param>
        /// <param name="titleIds">The title ids.</param>
        public void FlagForRedelivery(IList<string> queueNames, IList<int> titleIds)
        {
            queueCommandHelper.ResetFor(queueNames, titleIds);
        }

        /// <summary>
        /// Flags the given list of queues for redelivery. Assets selected for delivery
        /// depends on 'airingIds'
        /// </summary>
        /// <param name="queueNames">The queue names.</param>
        /// <param name="airingIds">The airing ids.</param>
        public void FlagForRedelivery(IList<string> queueNames, IList<string> airingIds)
        {
            queueCommandHelper.ResetFor(queueNames, airingIds);
        }

        public BLModel.Queue GetByApiKey(string apiKey)
        {
            return
            queueQueryHelper.GetByApiKey(apiKey)
                .ToBusinessModel<DLModel.Queue, BLModel.Queue>();

        }

        /// <summary>
        /// Locks the queue
        /// </summary>
        /// <param name="queueName">queue name to lock</param>
        /// <param name="processorId">processor id to lock</param>
        /// <returns>Returns true if locked sucessfully</returns>
        public bool Lock(string queueName, string processId)
        {
            var qLock = queueLocker.AquireLockFor(queueName, processId);
            return qLock.IsLockedBy(processId);
        }

        /// <summary>
        /// Unlocks the queue
        /// </summary>
        /// <param name="queueName">queue name to lock</param>
        /// <param name="processorId">processor id to lock</param>
        public void Unlock(string queueName, string processId)
        {
            queueLocker.ReleaseLockFor(queueName, processId);
        }


        /// <summary>
        /// Check and returns any message delived for given Queue and MediaId
        /// </summary>
        /// <param name="mediaId">media id to check</param>
        /// <param name="queueName">queue name to check</param>
        public bool AnyMessageDeliveredForMediaId(string mediaId, string queueName)
        {
            return queueMessages.GetByMediaId(mediaId, queueName).Any();
        }

        /// <summary>
        /// Check and returns any message delived for given Queue and AiringId
        /// </summary>
        /// <param name="airingId">media id to check</param>
        /// <param name="queueName">queue name to check</param>
        public bool AnyMessageDeliveredForAiringId(string airingId, string queueName)
        {
            return queueMessages.GetBy(queueName, airingId).Any();
        }

        /// <summary>
        /// Adds the historical message for the queue delivery
        /// </summary>
        /// <param name="airingId">the airing id</param>
        /// <param name="mediaId">the media id</param>
        /// <param name="message">the message</param>
        /// <param name="remoteQueueName">the queue name</param>
        /// <param name="messagePriority">message priority</param>
        public void AddHistoricalMessage(string airingId, string mediaId, string message, string remoteQueueName, byte? messagePriority)
        {
            var historicalMessage = new HistoricalMessage(airingId, mediaId, message, remoteQueueName, messagePriority);

            historyRecorder.Record(historicalMessage);
        }
    }
}
