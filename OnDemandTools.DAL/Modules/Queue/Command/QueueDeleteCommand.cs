using System;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using MongoDB.Bson;
using QueueModel = OnDemandTools.DAL.Modules.Queue.Model;

namespace OnDemandTools.DAL.Modules.Queue.Command
{
    public class QueueDeleteCommand : IQueueDeleteCommand
    {
        private readonly MongoDatabase _database;

        public QueueDeleteCommand(IODTDatastore connection)
        {
            _database = connection.GetDatabase();
        }

        public void Delete(ObjectId id)
        {
            var collection = _database.GetCollection<QueueModel.Queue>("DeliveryQueue");

            collection.Remove(Query<QueueModel.Queue>.EQ(q => q.Id, id));
        }
    }
}