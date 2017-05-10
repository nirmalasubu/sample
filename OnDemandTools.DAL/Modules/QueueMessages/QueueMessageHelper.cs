using OnDemandTools.DAL.Modules.QueueMessages.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnDemandTools.DAL.Modules.QueueMessages
{
    public interface IGetQueueMessagesQuery
    {
        long GetPendingDeliveryCountBy(string queueName);

        IQueryable<HistoricalMessage> GetBy(string remoteQueueName, DateTime since, int limit);
        IQueryable<HistoricalMessage> GetBy(string remoteQueueName, int limit);
        IQueryable<HistoricalMessage> GetBy(string remoteQueueName, string airingId);
        IQueryable<HistoricalMessage> GetByMediaId(string remoteQueueName, string mediaId);
    }

    public interface IAiringMessagePusherQueueApi
    {
        void PushBy(DeliverCriteria criteria);
    }

    public interface IMessagePusherAiringApi
    {
        void PushBy(IList<string> airingIds);
    }

    public interface IAiringMessagePusherAiringApi
    {
        void PushBy(string queueName, IList<string> airingIds);
    }

    public interface IAiringMessagePusherMessagePusher
    {
        void PushBy(string queueName, string query, int hoursOut, IList<string> airingIds);
    }

    public interface IQueueGetPendingMessagesQuery
    {
        long GetPendingDeliveryCountBy(string queueName);
    }
}
