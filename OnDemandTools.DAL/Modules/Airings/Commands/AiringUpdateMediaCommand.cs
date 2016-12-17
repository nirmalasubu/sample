using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Airings.Model;

namespace OnDemandTools.DAL.Modules.Airings.Commands
{
    public class AiringUUpdateMediaCommand
    {
        private readonly MongoCollection<Airing> _collection;

        public AiringUUpdateMediaCommand(IODTDatastore connection)
        {
            var database = connection.GetDatabase();

            _collection = database.GetCollection<Airing>("currentassets");
        }

        public void Update(string airingId, string mediaId)
        {
            var query = Query.EQ("AssetId", airingId);
            var set = MongoDB.Driver.Builders.Update
                .Set("MediaId", mediaId);

            _collection.Update(query, set);
        }
    }
}