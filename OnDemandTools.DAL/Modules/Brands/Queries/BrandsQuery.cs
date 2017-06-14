using System.Linq;
using MongoDB.Driver;
using OnDemandTools.DAL.Database;
using System.Collections.Generic;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Modules.Brands.Comparer;

namespace OnDemandTools.DAL.Modules.Brands.Queries
{
    public class BrandsQuery : IBrandQuery, IGetBrandByNameQuery
    {
        private readonly MongoDatabase _database;

        public BrandsQuery(IODTDatastore connection)
        {
            _database = connection.GetDatabase();
        }

        public IQueryable<string> Get()
        {
            var airingCollection = _database.GetCollection<OnDemandTools.DAL.Modules.Airings.Model.Airing>("currentassets");
            var brandCollection = _database.GetCollection<Model.Brand>("Brands");
            IEnumerable<Model.Brand> uniqueExistingBrands = new List<Model.Brand>();
            IEnumerable<Model.Brand> airingBrands = new List<Model.Brand>();

            var airingBrandStrings = airingCollection.Distinct("Network")
                                            .Where(b => !b.IsBsonNull)
                                            .Select(b => b.AsString);

            airingBrands = airingBrandStrings.Select(bString => new Model.Brand(bString));
            uniqueExistingBrands = brandCollection.Distinct("Name")
                                                  .Where(b => !b.IsBsonNull)
                                                  .Select(b => new Model.Brand(b.AsString));

            return airingBrands.Union(uniqueExistingBrands, new BrandComparer())
                               .Select(ab => ab.Name)
                               .AsQueryable();
        }


        public Model.Brand GetBy(string name)
        {
            var brands = _database.GetCollection<Model.Brand>("Brands");
            var query = Query.EQ("Name", name);

            return brands.FindOne(query);
        }
    }
}
