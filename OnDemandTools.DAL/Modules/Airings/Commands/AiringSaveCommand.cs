using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Airings.Model;
using System;
using System.Linq;
using Newtonsoft.Json;
using OnDemandTools.DAL.Modules.Reporting.Command;

namespace OnDemandTools.DAL.Modules.Airings.Commands
{
    public class AiringSaveCommand : IAiringSaveCommand
    {
        private readonly MongoDatabase _database;
        private readonly IGetAiringQuery _getAiringQuery;
        private readonly IDfStatusMover _dfStatusMover;

        public AiringSaveCommand(IODTDatastore connection, IGetAiringQuery getAiringQueryPrimaryDb, IDfStatusMover dfStatusMover)
        {
            _database = connection.GetDatabase();
            _getAiringQuery = getAiringQueryPrimaryDb;
            _dfStatusMover = dfStatusMover;
        }

        public Airing Save(Airing airing, bool hasImmediateDelivery, bool updateHistorical)
        {
            var currentCollection = _database.GetCollection<Airing>(DataStoreConfiguration.CurrentAssetsCollection);
            var expiredCollection = _database.GetCollection<Airing>(DataStoreConfiguration.ExpiredAssetsCollection);
            var historicalCollection = _database.GetCollection<Airing>(DataStoreConfiguration.HistoricalAssetsCollection);

            if (updateHistorical)
            {
                var airingInString = JsonConvert.SerializeObject(airing);
                var historyAiring = JsonConvert.DeserializeObject<Airing>(airingInString);
                historyAiring.Id = ObjectId.Empty;
                historicalCollection.Save(historyAiring);
            }

            airing.Id = ObjectId.Empty;

            var query = Query.EQ("AssetId", airing.AssetId);

            bool hasActiveFlights = airing.Flights.Any(e => e.End > DateTime.UtcNow);

            var currentAsset = currentCollection.FindOne(query);
            var expiredAsset = expiredCollection.FindOne(query);

            if (currentAsset != null || hasActiveFlights || hasImmediateDelivery)
            {

                currentCollection.Update(query,
                                         Update.Replace(airing),
                                         UpdateFlags.Upsert);

                if (expiredAsset != null)
                {
                    expiredCollection.Remove(query);
                    _dfStatusMover.MoveToCurrentCollection(airing.AssetId);
                }

                return _getAiringQuery.GetBy(airing.AssetId);
            }

            expiredCollection.Update(query,
                                         Update.Replace(airing),
                                         UpdateFlags.Upsert);

            return _getAiringQuery.GetBy(airing.AssetId, AiringCollection.ExpiredCollection);
        }
    }
}