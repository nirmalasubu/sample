using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Airings.Model;
using System.Collections.Generic;

namespace OnDemandTools.DAL.Modules.Airings.Commands
{
    public class BaseUpdateAiringQueueDelivery
    {
        #region "PROPERTIES"
        private readonly MongoCollection<Airing> _airings;
        #endregion

        #region "CONSTRUCTOR"
        public BaseUpdateAiringQueueDelivery(IODTDatastore connection, string collectionName)
        {
            var database = connection.GetDatabase();
            _airings = database.GetCollection<Airing>(collectionName);
        }

        public BaseUpdateAiringQueueDelivery(IODTDatastore connection)
        {
            var database = connection.GetDatabase();
            _airings = database.GetCollection<Airing>(DataStoreConfiguration.CurrentAssetsCollection);
        }

        #endregion

        #region "PUBLIC METHODS"

        public void PushIgnoredQueueTo(string airingId, string queueName)
        {
            var query = new QueryDocument { { "AssetId", airingId } };

            var pullDeliverTo = Update.PullAllWrapped("DeliverTo", queueName);
            var pushIgnoredQueues = Update.PushAllWrapped("IgnoredQueues", queueName);
            var pullDeliveredTo = Update.PullAllWrapped("DeliveredTo", queueName);
            var pullNotificationUpdate = Update.Pull("ChangeNotifications",
                new BsonDocument() { { "QueueName", queueName } });

            List<UpdateBuilder> updateValues = new List<UpdateBuilder>
            {
                pullDeliverTo,
                pushIgnoredQueues,
                pullDeliveredTo,
                pullNotificationUpdate
            };

            IMongoUpdate allUpdate = Update.Combine(updateValues);

            _airings.Update(query, allUpdate, UpdateFlags.Multi);
        }

        public void PushDeliveredTo(string airingId, string queueName)
        {
            var query = new QueryDocument { { "AssetId", airingId } };

            var pullDeliverTo = Update.PullAllWrapped("DeliverTo", queueName);
            var pullIgnoredQueues = Update.PullAllWrapped("IgnoredQueues", queueName);
            var pushDeliveredTo = Update.PushAllWrapped("DeliveredTo", queueName);
            var pullNotificationUpdate = Update.Pull("ChangeNotifications",
                new BsonDocument() { { "QueueName", queueName } });

            List<UpdateBuilder> updateValues = new List<UpdateBuilder>
            {
                pullDeliverTo,
                pullIgnoredQueues,
                pushDeliveredTo,
                pullNotificationUpdate
            };

            IMongoUpdate allUpdate = Update.Combine(updateValues);

            _airings.Update(query, allUpdate, UpdateFlags.Multi);

        }

        #endregion


    }
}
