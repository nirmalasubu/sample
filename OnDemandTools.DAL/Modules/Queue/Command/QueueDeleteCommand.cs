using System;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using MongoDB.Bson;
using QueueModel = OnDemandTools.DAL.Modules.Queue.Model;
using OnDemandTools.DAL.Modules.UserPermissions.Command;
using OnDemandTools.DAL.Modules.UserPermissions.Model;
using OnDemandTools.DAL.Modules.Queue.Queries;

namespace OnDemandTools.DAL.Modules.Queue.Command
{
    public class QueueDeleteCommand : IQueueDeleteCommand
    {
        private readonly MongoDatabase _database;
        private readonly IQueueQuery queueQuery;

        public QueueDeleteCommand(IODTDatastore connection, IQueueQuery queueQuery)
        {
            _database = connection.GetDatabase();
            this.queueQuery = queueQuery;
        }

        public void Delete(ObjectId id)
        {
            var collection = _database.GetCollection<QueueModel.Queue>("DeliveryQueue");

            var queue = queueQuery.Get(id);

            collection.Remove(Query<QueueModel.Queue>.EQ(q => q.Id, id));

            DeleteQueueIdFromPermission(queue.Name);
        }

        private void DeleteQueueIdFromPermission(string name)
        {
            var collection = _database.GetCollection<UserPermission>("UserPermission");            

            UpdateBuilder ub = Update.Unset("Portal.DeliveryQueuePermissions." + name);

            MongoUpdateOptions options = new MongoUpdateOptions
            {
                Flags = UpdateFlags.Multi
            };

            collection.Update(Query<UserPermission>.Exists(e => e.Id), ub, options);
        }
    }
}