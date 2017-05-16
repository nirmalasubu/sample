using System;
using MongoDB.Driver;
using OnDemandTools.DAL.Database;
using QueueModel = OnDemandTools.DAL.Modules.Queue.Model;

namespace OnDemandTools.DAL.Modules.Queue.Command
{
    public class QueueSaveCommand : IQueueSaveCommand
    {
        private readonly MongoDatabase _database;

        public QueueSaveCommand(IODTDatastore connection)
        {
            _database = connection.GetDatabase();
        }

        public QueueModel.Queue Save(QueueModel.Queue queue)
        {
            var collection = _database.GetCollection<QueueModel.Queue>("DeliveryQueue");           
            collection.Save(queue);
            return queue;
        }
    }
}