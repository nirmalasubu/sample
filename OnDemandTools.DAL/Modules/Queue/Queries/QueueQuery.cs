using System;
using System.Linq;
using MongoDB.Driver;
using OnDemandTools.DAL.Database;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;

namespace OnDemandTools.DAL.Modules.Queue.Queries
{
    public class QueueQuery : IQueueQuery
    {
        private readonly MongoDatabase _database;

        public QueueQuery (IODTDatastore connection)
        {
            _database = connection.GetDatabase();
        }

        public IQueryable<Model.Queue> Get()
        {
            var queues = _database.GetCollection<Model.Queue>("DeliveryQueue").AsQueryable();

            return queues;
        }

        public Model.Queue Get(int id)
        {
            throw new NotImplementedException();
        }

        public Model.Queue GetByApiKey(string apiKey)
        {
            var query = Query.EQ("Name", apiKey);
            var queue = _database.GetCollection<Model.Queue>("DeliveryQueue").FindOne(query);

            return queue;
        }

        public IQueryable<Model.Queue> GetByStatus(bool active)
        {
            var queues = _database.GetCollection<Model.Queue>("DeliveryQueue").AsQueryable();
            return queues.Where(q => q.Active == active);
        }

        public IQueryable<Model.Queue> GetPackageQueues()
        {
            var queues = _database.GetCollection<Model.Queue>("DeliveryQueue").AsQueryable<Model.Queue>();

            return queues.Where(q => q.DetectPackageChanges);
        }

        public IQueryable<Model.Queue> GetStatusQueues()
        {
            var queues = _database.GetCollection<Model.Queue>("DeliveryQueue").AsQueryable<Model.Queue>();

            return queues.Where(q => q.DetectStatusChanges);
        }
    }
}
