using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;

namespace ODTPOCHarbor.Models
{
    public class ProductRepository : IProductRepository
    {

        private readonly IMongoDatabase database;
        IMongoCollection<Product> products;        
        public ProductRepository()
        {
            var client = new MongoClient("mongodb://appFulfillment:!TURner2016$_DOWNSTREAM@ds039775-a0.mongolab.com:39775,ds039775-a1.mongolab.com:39775/fulfillment-dev");
            database = client.GetDatabase("fulfillment-dev");
            products = database.GetCollection<Product>("Product-JJ");
        }


        public void Add(Product item)
        {
            products.InsertOne(item);
        }

        public IEnumerable<Product> GetAll()
        {
            return products.Find(new BsonDocument()).ToList();            
        }
    }
}
