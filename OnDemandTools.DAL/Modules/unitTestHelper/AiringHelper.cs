using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Airings.Model;
using OnDemandTools.DAL.Modules.QueueMessages.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.DAL.Modules.unitTestHelper
{
    public class AiringHelper :IAiringHelper
    {
        private readonly MongoDatabase _database;

        private readonly MongoCollection<Airing> _airingCollection;
        private readonly MongoCollection<HistoricalMessage> _history;

        public AiringHelper(IODTDatastore connection)
        {
            _database = connection.GetDatabase();
            _airingCollection = _database.GetCollection<Airing>("currentassets");
            _history = _database.GetCollection<HistoricalMessage>("MessageHistory");
        }

       public void  UpdateAiringRelasedDateAndFlightEndDate(string airingId, DateTime releasedon)
        {
            var query = Query.EQ("AssetId", airingId);
            var set = Update .Set("ReleaseOn", releasedon)
                .Set("Flights.0.End", DateTime.UtcNow.AddDays(-2));

            _airingCollection.Update(query, set);

            
        }

        public void RemoveMediaIdFromHistory(string mediaId)
        {
            _history.Remove(Query.EQ("MediaId", mediaId));
        }
    }
}
