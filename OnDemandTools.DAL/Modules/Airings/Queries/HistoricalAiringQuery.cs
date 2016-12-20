using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Linq;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Airings.Model;

namespace OnDemandTools.DAL.Modules.Airings.Queries
{
    public class ChangeHistoricalAiringQuery : IChangeHistoricalAiringQuery
    {
        private readonly MongoCollection<Airing> _collection;

        public ChangeHistoricalAiringQuery(IODTDatastore connection)
        {
            var database = connection.GetDatabase();

            _collection = database.GetCollection<Airing>(DataStoreConfiguration.HistoricalAssetsCollection);
        }

        public IEnumerable<Airing> Find(IEnumerable<string> assetIdList)
        {
            var assetIds = assetIdList.ToList();

            var query = Query<Airing>.In(x => x.AssetId, assetIds);

            var dbAssets = _collection.Find(query);

            return dbAssets;
        }

        public IQueryable<Airing> Query()
        {
            return _collection.AsQueryable();
        }
    }
}