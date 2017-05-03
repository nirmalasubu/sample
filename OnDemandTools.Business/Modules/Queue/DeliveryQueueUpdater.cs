using OnDemandTools.Common.Configuration;
using System;
using System.Collections.Generic;
using RabbitMQ.Client;
using EasyNetQ;

namespace OnDemandTools.Business.Modules.Queue
{
    public class DeliveryQueueUpdater: IDeliveryQueueUpdater
    {
        private readonly string _connectionString;

        private readonly string _exchangeName;

        public DeliveryQueueUpdater(AppSettings appsettings)
        {
            _connectionString = appsettings.CloudQueue.MqUrl;
            _exchangeName = appsettings.CloudQueue.MqExchange;
        }

        public List<Model.Queue> PopulateMessageCounts(List<Model.Queue> deliveryQueues)
        {
            try
            {
                using (var advancedBus = RabbitHutch.CreateBus(_connectionString).Advanced)
                {
                    foreach (var queue in deliveryQueues)
                    {
                        var existingQueue = new EasyNetQ.Topology.Queue(queue.Name, false);

                        queue.MessageCount = advancedBus.MessageCount(existingQueue);
                    }
                }
                return deliveryQueues;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
