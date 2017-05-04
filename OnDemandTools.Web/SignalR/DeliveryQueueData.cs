using OnDemandTools.Business.Modules.Queue;
using System.Collections.Generic;
using OnDemandTools.Web.Models.DeliveryQueue;
using OnDemandTools.Business.Modules.HangFire;
using OnDemandTools.Business.Modules.Queue.Model;
using OnDemandTools.Common.Model;

namespace OnDemandTools.Web.SignalR
{
    public class DeliveryQueueData: IDeliveryQueueData
    {
        public IQueueService _queueSvc;
        public IDeliveryQueueUpdater _deliveryQueueUpdater;
        private readonly IGetHangireServers _jobLastRunQuery;
        public DeliveryQueueData(IQueueService queueSvc,
            IDeliveryQueueUpdater deliveryQueueUpdater,
            IGetHangireServers jobLastRunQuery)
        {
            _queueSvc = queueSvc;
            _deliveryQueueUpdater = deliveryQueueUpdater;
            _jobLastRunQuery = jobLastRunQuery;
        }

        public  QueuesHubModel FetchQueueDeliveryCounts()
        {
            List<Queue> deliveryqueues = _queueSvc.GetQueues();

            List<Queue> queuesWithPendingDeliveryCount = _queueSvc.PopulateMessageCounts(deliveryqueues);
            List<DeliveryQueueHubModel> updatedQueues = _deliveryQueueUpdater.PopulateMessageCounts(queuesWithPendingDeliveryCount).ToViewModel<List<Queue>, List<DeliveryQueueHubModel>>();

            var jobStatus = _jobLastRunQuery.GetStatus();
            QueuesHubModel queues = new QueuesHubModel
            {
                Queues = updatedQueues,
                JobLastRun = jobStatus.LastHeartbeat,
                JobCount = jobStatus.Count
            };

            return queues;
        }
    }

    public interface IDeliveryQueueData
    {
        QueuesHubModel FetchQueueDeliveryCounts();
    }
}
