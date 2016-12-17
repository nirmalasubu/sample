using System;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Airings.Model;

namespace OnDemandTools.DAL.Modules.Airings.Commands
{
    public class AiringUpdateLinearCommand
    {
        private readonly MongoCollection<Airing> _collection;

        public AiringUpdateLinearCommand(IODTDatastore connection)
        {
            var database = connection.GetDatabase();

            _collection = database.GetCollection<Airing>("currentassets");
        }

        public void UpdateLinear(string airingId, DateTime linearAirDateTime)
        {
            var query = Query.And(Query.EQ("AssetId", airingId), Query.EQ("Airings.Linked", true));
            var set = Update.Set("Airings.$.Date", linearAirDateTime);

            _collection.Update(query, set);
        }
    }

    public interface IMarkQueueProcessedCommand
    {
        void Mark(string name);
    }
}