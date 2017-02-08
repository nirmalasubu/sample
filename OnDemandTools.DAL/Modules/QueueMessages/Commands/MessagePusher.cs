using OnDemandTools.DAL.Modules.Queue.Queries;
using System.Collections.Generic;

namespace OnDemandTools.DAL.Modules.QueueMessages.Commands
{
    public class MessagePusher : IMessagePusherAiringApi
    {
        private readonly IQueueQuery _getQueuesQuery;
        private readonly IAiringMessagePusherMessagePusher _messagePusher;

        public MessagePusher(IQueueQuery getQueuesQuery, IAiringMessagePusherMessagePusher messagePusher)
        {
            _getQueuesQuery = getQueuesQuery;
            _messagePusher = messagePusher;
        }

        public void PushBy(IList<string> airingIds)
        {
            var queues = _getQueuesQuery.GetByStatus(true);
                
            foreach (var deliveryQueue in queues)
            {
                _messagePusher.PushBy(deliveryQueue.Name, deliveryQueue.Query, deliveryQueue.HoursOut, airingIds);
            }
        }
    }   
}