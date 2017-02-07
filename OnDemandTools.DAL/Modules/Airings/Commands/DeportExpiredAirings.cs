using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Airings.Model;
using System;
using OnDemandTools.Common.Configuration;
using System.Linq;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace OnDemandTools.DAL.Modules.Airings.Commands
{
   

    public class DeportExpiredAiring : IDeportExpiredAiring
    {
        private readonly MongoCollection<Airing> _currentCollection;
        private readonly MongoCollection<Airing> _expiredCollection;
      
        public DeportExpiredAiring(IODTDatastore connection)
        {
           
            var database = connection.GetDatabase();

            _currentCollection = database.GetCollection<Airing>(DataStoreConfiguration.CurrentAssetsCollection);
            _expiredCollection = database.GetCollection<Airing>(DataStoreConfiguration.ExpiredAssetsCollection);
        }


        // Retrieve airings whose flight end date is less than or equal
        // to cutOffDateTime. If there are multiple flight windows, flight end
        // date for all such windows should be less than or equal to cutOffDateTime
        /// <summary>
        /// Gets the expired airings.
        /// </summary>
        public void Deport(int airingDeportGraceDays)
        {
            DateTime cutOffDateTime = DateTime.UtcNow.AddDays(-airingDeportGraceDays);

            var strQuery =
                "{ 'Flights.End': { $lte: ISODate('" + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                + "') }, $and: [ { 'ReleaseOn': { $lte: ISODate('"
                + cutOffDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") + "') } } ] }";

            BsonDocument document = MongoDB.Bson.Serialization.BsonSerializer
                .Deserialize<BsonDocument>(strQuery);


            QueryDocument queryDoc = new QueryDocument(document);
            MongoCursor results = _currentCollection.Find(queryDoc);
            foreach (Airing ar in results)
            {
                // verify that there are no other active flight windows for this airing
                if (!ar.Flights.Any(c => c.End > DateTime.UtcNow))
                {
                    // First save to expired collection
                    ar.Id = ObjectId.Empty;
                    var query = Query.EQ("AssetId", ar.AssetId);
                    _expiredCollection.Update(query, Update.Replace(ar), UpdateFlags.Upsert);

                    // Then remove from current collecton
                    _currentCollection.Remove(query);
                }
            }
        }

    }
}