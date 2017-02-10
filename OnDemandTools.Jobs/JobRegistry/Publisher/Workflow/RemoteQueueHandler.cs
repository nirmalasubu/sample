using EasyNetQ;
using OnDemandTools.Business.Modules.Queue.Model;
using OnDemandTools.Common.Configuration;
using RabbitMQ.Client;

namespace OnDemandTools.Jobs.JobRegistry.Publisher
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

        public void Create(Queue queue)
        {

            using (var advancedBus = RabbitHutch.CreateBus(_connectionString).Advanced)
            {
                var newQueue = queue.IsPriorityQueue ? advancedBus.QueueDeclare(queue.Name, maxPriority: 10) : advancedBus.QueueDeclare(queue.Name);

                var exchange = advancedBus.ExchangeDeclare(_exchangeName, ExchangeType.Topic, true);

                advancedBus.Bind(exchange, newQueue, queue.RoutingKey);
            }

        }
    }

    public interface IRemoteQueueHandler
    {
        void Create(Queue queue);
    }
}