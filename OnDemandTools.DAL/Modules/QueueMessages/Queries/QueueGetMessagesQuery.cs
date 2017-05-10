using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Airings.Model;
using System;
using System.Linq;
using OnDemandTools.DAL.Modules.QueueMessages.Model;

namespace OnDemandTools.DAL.Modules.QueueMessages.Queries
{
    public class QueueGetMessagesQuery : IGetQueueMessagesQuery, IQueueGetPendingMessagesQuery
    {
        private readonly MongoCollection<HistoricalMessage> _collection;
        private readonly MongoCollection<Airing> _currentAiringCollection;

        public QueueGetMessagesQuery(IODTDatastore connection)
        {
            var database = connection.GetDatabase();

            _collection = database.GetCollection<HistoricalMessage>("MessageHistory");
            _currentAiringCollection = database.GetCollection<Airing>("currentassets");
        }

        public IQueryable<HistoricalMessage> GetBy(string remoteQueueName, DateTime since, int limit)
        {
            var query = Query.And(Query.EQ("RemoteQueueName", remoteQueueName),
                Query.GTE("DateTime", since));

            var messages = _collection.Find(query).SetSortOrder(SortBy.Descending("DateTime")).Take(limit);

            return messages.AsQueryable();
        }

        public IQueryable<HistoricalMessage> GetBy(string remoteQueueName, string airingId)
        {
            var query = Query.And(Query.EQ("RemoteQueueName", remoteQueueName), Query.EQ("AiringId", airingId));

            var messages = _collection.Find(query);

            return messages.AsQueryable();
        }

        public IQueryable<HistoricalMessage> GetBy(string remoteQueueName, int limit)
        {
            var query = Query.EQ("RemoteQueueName", remoteQueueName);
            
            var messages = _collection.Find(query);

            return messages.AsQueryable().OrderByDescending(e=>e.AiringId).Take(limit);
        }

        public IQueryable<HistoricalMessage> GetByMediaId(string mediaId, string remoteQueueName)
        {
            var query = Query.And(Query.EQ("RemoteQueueName", remoteQueueName), Query.EQ("MediaId", mediaId));

            var messages = _collection.Find(query);

            return messages.AsQueryable();
        }

        public long GetPendingDeliveryCountBy(string queueName)
        {
            var query = Query.In("DeliverTo", new BsonArray() { queueName });

            return _currentAiringCollection.Count(query);
        }
    }
}