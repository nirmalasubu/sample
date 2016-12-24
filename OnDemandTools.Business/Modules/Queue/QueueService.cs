using OnDemandTools.Common.Model;
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

        IQueueQuery queueHelper;

        public QueueService(IQueueQuery queueHelper)
        {
            this.queueHelper = queueHelper;
        }


        /// <summary>
        /// Retrieves those queues that match the provided
        /// active flag (true/false)
        /// </summary>
        /// <param name="active">if set to <c>true</c> [active].</param>
        /// <returns></returns>
        public List<Model.Queue> GetQueueByStatus(bool active)
        {
            return
            (queueHelper.GetByStatus(active).ToList<DLModel.Queue>()
                .ToBusinessModel<List<DLModel.Queue>, List<BLModel.Queue>>());
            
        }
    }
}
