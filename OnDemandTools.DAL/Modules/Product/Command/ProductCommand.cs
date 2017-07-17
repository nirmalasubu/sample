using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using DLModel = OnDemandTools.DAL.Modules.Product.Model;
using System.Collections.Generic;

namespace OnDemandTools.DAL.Modules.Product.Command
{
    public class ProductCommand : IProductCommand
    {
        private readonly MongoDatabase _database;

        public ProductCommand(IODTDatastore connection)
        {
            _database = connection.GetDatabase();
        }

        public void Delete(string id)
        {
            var collection = _database.GetCollection<DLModel.Product>("Product");

            var query = Query<DLModel.Product>.EQ(e => e.Id, new ObjectId(id));

            collection.Remove(query);
        }
    }
}