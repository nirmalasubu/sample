using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Airings.Model;
using System;
using System.Linq;

namespace OnDemandTools.DAL.Modules.Airings.Commands
{
    public class AiringSaveCommand : IAiringSaveCommand
    {
        private readonly MongoDatabase _database;
        private readonly IGetAiringQueryPrimaryDb _getAiringQuery;

        public AiringSaveCommand(IODTDatastore connection, IGetAiringQueryPrimaryDb getAiringQueryPrimaryDb)
        {
            _database = connection.GetDatabase();

            _getAiringQuery = getAiringQueryPrimaryDb;
        }

        public Airing Save(Airing airing, bool hasImmediateDelivery, bool updateHistorical)
        {
            var currentCollection = _database.GetCollection<Airing>(DataStoreConfiguration.CurrentAssetsCollection);
            var expiredCollection = _database.GetCollection<Airing>(DataStoreConfiguration.ExpiredAssetsCollection);
            var historicalCollection = _database.GetCollection<Airing>(DataStoreConfiguration.HistoricalAssetsCollection);

            if (updateHistorical)
            {
                historicalCollection.Save(airing);
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