using EasyNetQ;
using BLQueue = OnDemandTools.Business.Modules.Queue.Model;
using OnDemandTools.Common.Configuration;
using RabbitMQ.Client;
using System;

namespace OnDemandTools.Business.Modules.AiringPublisher.Workflow
{
    public class RemoteQueueHandler : IRemoteQueueHandler
    {
        private readonly string _connectionString;

        private readonly string _exchangeName;

        public RemoteQueueHandler(AppSettings appsettings)
        {
            if (appsettings.CloudQueue != null && appsettings.CloudQueue.MqUrl != null)
            {
                _connectionString = appsettings.CloudQueue.MqUrl;
                _exchangeName = appsettings.CloudQueue.MqExchange;
            }
        }

        public void Create(BLQueue.Queue queue, bool prioritySelectionChanged = false)
        {
            if (prioritySelectionChanged)
            {
                Delete(queue.Name);
            }

            using (var advancedBus = RabbitHutch.CreateBus(_connectionString).Advanced)
            {
                var newQueue = queue.IsPriorityQueue ? advancedBus.QueueDeclare(queue.Name, maxPriority: 10) : advancedBus.QueueDeclare(queue.Name);

                var exchange = advancedBus.ExchangeDeclare(_exchangeName, ExchangeType.Topic, true);

                advancedBus.Bind(exchange, newQueue, queue.RoutingKey);
            }

        }

        public void Delete(string remoteQueueName, bool isPriorityQueue = false)
        {
            try
            {
                using (var advancedBus = RabbitHutch.CreateBus(_connectionString).Advanced)
                {
                    var existingQueue = isPriorityQueue ? advancedBus.QueueDeclare(remoteQueueName, maxPriority: 10) : advancedBus.QueueDeclare(remoteQueueName);

                    advancedBus.QueueDelete(existingQueue);
                }
            }
            catch (Exception)
            {
                if (!isPriorityQueue)
                {
                    Delete(remoteQueueName, true);
                }
                else
                {
                    throw;
                }
            }
        }

        public void Purge(string remoteQueueName)
        {
            using (var advancedBus = RabbitHutch.CreateBus(_connectionString).Advanced)
            {
                var existingQueue = new EasyNetQ.Topology.Queue(remoteQueueName, false);

                advancedBus.QueuePurge(existingQueue);
            }
        }
    }

    public interface IRemoteQueueHandler
    {
        void Create(BLQueue.Queue queue, bool prioritySelectionChanged = false);
        void Purge(string remoteQueueName);
    }
}