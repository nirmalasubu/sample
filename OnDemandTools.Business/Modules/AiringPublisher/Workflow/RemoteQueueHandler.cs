﻿using EasyNetQ;
using BLQueue = OnDemandTools.Business.Modules.Queue.Model;
using OnDemandTools.Common.Configuration;
using RabbitMQ.Client;

namespace OnDemandTools.Business.Modules.AiringPublisher.Workflow
{
    public class RemoteQueueHandler : IRemoteQueueHandler
    {
        private readonly string _connectionString;

        private readonly string _exchangeName;

        public RemoteQueueHandler(AppSettings appsettings)
        {
            _connectionString = appsettings.CloudQueue.MqUrl;
            _exchangeName = appsettings.CloudQueue.MqExchange;
        }

        public void Create(BLQueue.Queue queue)
        {

            using (var advancedBus = RabbitHutch.CreateBus(_connectionString).Advanced)
            {
                var newQueue = queue.IsPriorityQueue ? advancedBus.QueueDeclare(queue.Name, maxPriority: 10) : advancedBus.QueueDeclare(queue.Name);

                var exchange = advancedBus.ExchangeDeclare(_exchangeName, ExchangeType.Topic, true);

                advancedBus.Bind(exchange, newQueue, queue.RoutingKey);
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
        void Create(BLQueue.Queue queue);
        void Purge(string remoteQueueName);
    }
}