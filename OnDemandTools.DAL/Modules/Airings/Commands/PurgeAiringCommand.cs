using MongoDB.Bson;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Airings.Model;
using OnDemandTools.DAL.Modules.QueueMessages.Model;
using OnDemandTools.DAL.Modules.Reporting.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.DAL.Modules.Airings.Commands
{
    public class PurgeAiringCommand : IPurgeAiringCommand
    {
        IODTDatastore _connection;

        public PurgeAiringCommand(IODTDatastore connection)
        {
            _connection = connection;
        }

        public void PurgeAirings(List<string> airingIds)
        {
            var database = _connection.GetDatabase();

            var query = Query.In("AssetId", new BsonArray(airingIds));
            database.GetCollection<Airing>("currentassets").Remove(query);
            database.GetCollection<Airing>("deletedasset").Remove(query);
            database.GetCollection<Airing>("expiredassets").Remove(query);
            database.GetCollection<Airing>("assethistory").Remove(query);

            query = Query.In("AssetID", new BsonArray(airingIds));
            database.GetCollection<DF_Status>("DFStatus").Remove(query);
            database.GetCollection<DF_Status>("DFExpiredStatus").Remove(query);

            query = Query.In("AiringId", new BsonArray(airingIds));
            database.GetCollection<HistoricalMessage>("MessageHistory").Remove(query);

        }
    }

    public interface IPurgeAiringCommand
    {
        void PurgeAirings(List<string> airingIds);
    }
}
