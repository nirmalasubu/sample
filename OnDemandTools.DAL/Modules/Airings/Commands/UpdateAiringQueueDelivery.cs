using MongoDB.Bson;
using MongoDB.Driver;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Airings.Model;

namespace OnDemandTools.DAL.Modules.Airings.Commands
{
    public class UpdateAiringQueueDelivery : IUpdateAiringQueueDelivery
    {
        #region "PROPERTIES"
        private readonly MongoCollection<Airing> _airings;
        #endregion

        #region "CONSTRUCTOR"
        public UpdateAiringQueueDelivery(IODTDatastore connection, string collectionName)
        {
            var database = connection.GetDatabase();
            _airings = database.GetCollection<Airing>(collectionName);
        }

        public UpdateAiringQueueDelivery(IODTDatastore connection)
        {
            var database = connection.GetDatabase();
            _airings = database.GetCollection<Airing>(DataStoreConfiguration.CurrentAssetsCollection);
        }

        #endregion

        #region "PUBLIC METHODS"

        public void PushIgnoredQueueTo(string airingId, string queueName)
        {
            var query = new QueryDocument { { "AssetId", airingId } };
            DeleteField(queueName, query, "DeliverTo");
            DeleteField(queueName, query, "DeliveredTo");
            UpdateField(queueName, query, "IgnoredQueues");
        }

        public void PushDeliveredTo(string airingId, string queueName)
        {
            var query = new QueryDocument { { "AssetId", airingId } };
            DeleteField(queueName, query, "DeliverTo");
            DeleteField(queueName, query, "IgnoredQueues");
            UpdateField(queueName, query, "DeliveredTo");
        }

        #endregion

        #region "PRIVATE METHODS"
        private void DeleteField(string queueName, QueryDocument query, string property)
        {
            var update = new UpdateDocument
                             {
                                 {"$pull", new BsonDocument(property, queueName)}
                             };

            _airings.Update(query, update, UpdateFlags.Multi);
        }

        private void UpdateField(string queueName, QueryDocument query, string property)
        {
            var update = new UpdateDocument
                             {
                                 {"$addToSet", new BsonDocument(property, queueName)}
                             };

            _airings.Update(query, update, UpdateFlags.Multi);
        }
        #endregion

    }
}
