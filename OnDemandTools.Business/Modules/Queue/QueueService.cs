using OnDemandTools.Common.Model;
using OnDemandTools.DAL.Modules.Queue.Command;
using OnDemandTools.DAL.Modules.Queue.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using BLModel = OnDemandTools.Business.Modules.Queue.Model;
using DLModel = OnDemandTools.DAL.Modules.Queue.Model;

namespace OnDemandTools.Business.Modules.Queue
{
    public class QueueService : IQueueService
    {

        IQueueQuery queueQueryHelper;
        IQueueCommand queueCommandHelper;

        public QueueService(IQueueQuery queueQueryHelper, IQueueCommand queueCommandHelper)
        {
            this.queueQueryHelper = queueQueryHelper;
            this.queueCommandHelper = queueCommandHelper;
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
    }
}
