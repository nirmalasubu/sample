using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnDemandTools.DAL.Modules.Queue.Model;
using MongoDB.Driver;
using OnDemandTools.DAL.Database;
using MongoDB.Driver.Linq;

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
            throw new NotImplementedException();
        }

        public Model.Queue Get(int id)
        {
            throw new NotImplementedException();
        }

        public Model.Queue GetByApiKey(string apiKey)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Model.Queue> GetByStatus(bool active)
        {
            var queues = _database.GetCollection<Model.Queue>("DeliveryQueue").AsQueryable();
            return queues.Where(q => q.Active == active);
        }

        public IQueryable<Model.Queue> GetPackageQueues()
        {
            throw new NotImplementedException();
        }
    }
}
