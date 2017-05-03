using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR.Json;
using OnDemandTools.API.Utilities.Serialization;
using OnDemandTools.Business.Modules.HangFire;
using OnDemandTools.Business.Modules.Queue;
using OnDemandTools.Business.Modules.Queue.Model;
using OnDemandTools.Common.Model;
using OnDemandTools.Web.Models.DeliveryQueue;
using System.Collections.Generic;

namespace OnDemandTools.Web.SignalR
{
    [HubName("deliveryQueueCountHub")]
    public class DeliveryQueueCountHub : Hub
    {
        public IQueueService _queueSvc;
        public IDeliveryQueueUpdater _deliveryQueueUpdater;
        private readonly IGetHangireServers _jobLastRunQuery;
        public DeliveryQueueCountHub(IQueueService queueSvc,
            IDeliveryQueueUpdater deliveryQueueUpdater,
            IGetHangireServers jobLastRunQuery)
        {
            _queueSvc = queueSvc;
            _deliveryQueueUpdater = deliveryQueueUpdater;
            _jobLastRunQuery =jobLastRunQuery;
        }

        public void FetchQueueDeliveryCounts()
        {
            List<Queue> deliveryqueues = _queueSvc.GetQueues();
            List <DeliveryQueueHubModel> updatedQueues = _deliveryQueueUpdater.PopulateMessageCounts(deliveryqueues).ToViewModel<List<Queue>, List<DeliveryQueueHubModel>>();

            var jobStatus = _jobLastRunQuery.GetStatus();
            QueuesHubModel queues = new QueuesHubModel
            {
                Queues= updatedQueues,
                JobLastRun = jobStatus.LastHeartbeat,
                JobCount = jobStatus.Count
            };
            Clients.All.GetQueueDeliveryCount(queues);
        }
    }
}
