using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using OnDemandTools.DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnDemandTools.DAL.Modules.Destination.Queries
{
    public class DestinationQuery : IDestinationQuery
    {
        private readonly MongoDatabase _database;

        public DestinationQuery(IODTDatastore connection)
        {
            _database = connection.GetDatabase();
        }

        public IQueryable<Model.Destination> Get()
        {
            var destinations = _database
                .GetCollection<Model.Destination>("Destination")
                .AsQueryable();

            return destinations.AsQueryable();
        }

        public List<Model.Destination> GetByDestinationNames(List<string> names)
        {
            var destinations = _database
                .GetCollection<Model.Destination> ("Destination")
                .AsQueryable();

            return destinations.Where(d => names.Contains(d.Name)).ToList();
        }

        public IQueryable<Model.Destination> GetByMappingId(int mappingId)
        {
            var productDestinations = _database
                .GetCollection<Product.Model.Product>("Product")
                .Find(Query.EQ("MappingId", mappingId))
                .First()
                .Destinations;

            var destinations = _database
                .GetCollection<Model.Destination>("Destination")
                .Find(Query.In("Name", new BsonArray(productDestinations)))
                .AsQueryable();

            return destinations.Distinct(new Comparer.DestinationDataModelComparer());
        }

        public IQueryable<Model.Destination> GetByProductId(Guid productId)
        {
            var productDestinations = _database
                .GetCollection<Product.Model.Product>("Product")
                .Find(Query.EQ("ExternalId", productId))
                .First()
                .Destinations;

            var destinations = _database
                .GetCollection<Model.Destination>("Destination")
                .Find(Query.In("Name", new BsonArray(productDestinations)))
                .AsQueryable();

            return destinations.Distinct(new Comparer.DestinationDataModelComparer());
        }

        public IQueryable<Model.Destination> GetByProductIds(IList<Guid> productIds)
        {
            var products = _database
                .GetCollection<Product.Model.Product>("Product")
                .Find(Query.In("ExternalId", new BsonArray(productIds)));

            var destinationNames = products.SelectMany(p => p.Destinations);

            var destinations = _database
                .GetCollection<Model.Destination>("Destination")
                .Find(Query.In("Name", new BsonArray(destinationNames)))
                .AsQueryable();

            return destinations.Distinct(new Comparer.DestinationDataModelComparer());
        }
    }
}
