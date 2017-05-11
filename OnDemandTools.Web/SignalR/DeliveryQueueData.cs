using OnDemandTools.Business.Modules.Queue;
using System.Collections.Generic;
using OnDemandTools.Web.Models.DeliveryQueue;
using OnDemandTools.Business.Modules.HangFire;
using OnDemandTools.Business.Modules.Queue.Model;
using OnDemandTools.Common.Model;
using Microsoft.Extensions.Logging;
using System;

namespace OnDemandTools.Web.SignalR
{
    public class DeliveryQueueData: IDeliveryQueueData
    {
        public IQueueService _queueSvc;
        public IDeliveryQueueUpdater _deliveryQueueUpdater;
        private readonly IGetHangireServers _jobLastRunQuery;
        private readonly Serilog.ILogger _logger;
        public DeliveryQueueData(IQueueService queueSvc,
            IDeliveryQueueUpdater deliveryQueueUpdater,
            IGetHangireServers jobLastRunQuery,
            Serilog.ILogger logger)
        {
            _queueSvc = queueSvc;
            _deliveryQueueUpdater = deliveryQueueUpdater;
            _jobLastRunQuery = jobLastRunQuery;
            _logger = logger;
        }

        public  QueuesHubModel FetchQueueDeliveryCounts()
        {
            QueuesHubModel queues = new QueuesHubModel();
            try
            {
                List<Queue> deliveryqueues = _queueSvc.GetQueues();

                List<Queue> queuesWithPendingDeliveryCount = _queueSvc.PopulateMessageCounts(deliveryqueues);
                List<DeliveryQueueHubModel> updatedQueues = _deliveryQueueUpdater.PopulateMessageCounts(queuesWithPendingDeliveryCount).ToViewModel<List<Queue>, List<DeliveryQueueHubModel>>();

                var jobStatus = _jobLastRunQuery.GetStatus();

                queues.Queues = updatedQueues;
                queues.JobLastRun = jobStatus.LastHeartbeat;
                queues.JobCount = jobStatus.Count;
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Error in getting signalR Delviery Queue Data ");
            }
            return queues;
        }
    }

    public interface IDeliveryQueueData
    {
        QueuesHubModel FetchQueueDeliveryCounts();
    }
}
