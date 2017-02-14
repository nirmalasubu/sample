using OnDemandTools.Common.Model;
using OnDemandTools.DAL.Modules.Queue.Command;
using OnDemandTools.DAL.Modules.Queue.Queries;
using System.Collections.Generic;
using System.Linq;
using BLModel = OnDemandTools.Business.Modules.Queue.Model;
using DLModel = OnDemandTools.DAL.Modules.Queue.Model;
using System;

namespace OnDemandTools.Business.Modules.Queue
{
    public class QueueService : IQueueService
    {

        IQueueQuery queueQueryHelper;
        IQueueCommand queueCommandHelper;
        IQueueLocker queueLocker;

        public QueueService(IQueueQuery queueQueryHelper, IQueueCommand queueCommandHelper, IQueueLocker queueLocker)
        {
            this.queueQueryHelper = queueQueryHelper;
            this.queueCommandHelper = queueCommandHelper;
            this.queueLocker = queueLocker;
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
    }
}
