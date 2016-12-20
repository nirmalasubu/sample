using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Airings.Model;
using System.Collections.Generic;
using System.Linq;

namespace OnDemandTools.DAL.Modules.Airings.Queries
{
    public class DeletedAiringQuery : IChangeDeletedAiringQuery
    {
        private readonly MongoCollection<Airing> _collection;

        public DeletedAiringQuery(IODTDatastore connection)
        {
            var database = connection.GetDatabase();

            _collection = database.GetCollection<Airing>(DataStoreConfiguration.DeletedAssetsCollection);
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