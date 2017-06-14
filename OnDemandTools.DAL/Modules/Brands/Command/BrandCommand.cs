using MongoDB.Bson;
using MongoDB.Driver;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Brands.Comparer;
using System.Collections.Generic;
using System.Linq;

namespace OnDemandTools.DAL.Modules.Brands.Command
{
    public class BrandCommand : IBrandSaveCommand
    {
        private readonly MongoDatabase _database;

        public BrandCommand(IODTDatastore connection)
        {
            _database = connection.GetDatabase();
        }

        public void Save(List<Model.Brand> brands)
        {
            //existing brands collection
            var _brandsCollection = _database.GetCollection<Model.Brand>("Brands");

            //we want to only insert subset of brands that does not exist in _brandsCollection
            var allExistingBrands = _brandsCollection.FindAll().AsQueryable();

            var brandsToAdd = brands.Except(allExistingBrands, new BrandComparer());
            if (brandsToAdd.Count() > 0)
            {
                foreach (var b in brandsToAdd)
                    b.Id = ObjectId.Empty;

                _brandsCollection.InsertBatch(brandsToAdd);
            }

        }
    }
}
