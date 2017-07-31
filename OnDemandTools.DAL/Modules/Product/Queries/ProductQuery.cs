using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using OnDemandTools.DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnDemandTools.DAL.Modules.Product.Queries
{
    public class ProductQuery : IProductQuery
    {
        private readonly MongoDatabase _database;

        public ProductQuery(IODTDatastore connection)
        {
            _database = connection.GetDatabase();
        }

        public IQueryable<Model.Product> Get()
        {
            return _database
                .GetCollection<Model.Product>("Product")
                .AsQueryable();
        }

        public Model.Product GetById(string externalId)
        {
            return _database
             .GetCollection<Model.Product>("Product").AsQueryable()
             .FirstOrDefault(e=>e.ExternalId.ToString() == externalId);
        }

        public IQueryable<Model.Product> GetByProductIds(List<Guid> productIds)
        {
            return _database
             .GetCollection<Model.Product>("Product")
             .Find(Query.In("ExternalId", new BsonArray(productIds)))
             .AsQueryable();
        }

        public IQueryable<Model.Product> GetByTags(List<string> tags)
        {
            return _database
                .GetCollection<Model.Product>("Product")
                .Find(Query.All("Tags", new BsonArray(tags)))
                .AsQueryable();
        }
    }
}
