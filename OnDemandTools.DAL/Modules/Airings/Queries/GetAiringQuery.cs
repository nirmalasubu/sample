using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.Common.Exceptions;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Airings.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnDemandTools.DAL.Modules.Airings.Queries
{
    public class GetAiringQuery : IGetAiringQuery, IGetModifiedAiringQuery
    {
        private readonly MongoCollection<Airing> _currentCollection;
        private readonly MongoCollection<Airing> _deletedCollection;
        private readonly MongoCollection<Airing> _expiredCollection;

        public GetAiringQuery(IODTDatastore connection)
        {
            var database = connection.GetDatabase();

            _currentCollection = database.GetCollection<Airing>(DataStoreConfiguration.CurrentAssetsCollection);
            _deletedCollection = database.GetCollection<Airing>(DataStoreConfiguration.DeletedAssetsCollection);
            _expiredCollection = database.GetCollection<Airing>(DataStoreConfiguration.ExpiredAssetsCollection);
        }

        public Airing GetBy(string assetId, AiringCollection getFrom = AiringCollection.CurrentOrExpiredCollection)
        {
            var query = Query.EQ("AssetId", assetId);

            Airing airing = null;

            switch (getFrom)
            {
                case AiringCollection.CurrentOrExpiredCollection:
                    airing = _currentCollection.FindOne(query) ?? GetFromExpiredCollection(query);
                    break;
                case AiringCollection.CurrentCollection:
                    airing = _currentCollection.FindOne(query);
                    break;
                case AiringCollection.ExpiredCollection:
                    airing = GetFromExpiredCollection(query);
                    break;
                case AiringCollection.DeletedCollection:
                    airing = _deletedCollection.FindOne(query);
                    break;
            }

            if (airing == null)
                throw new AiringNotFoundException(string.Format("Airing does not exist in {0} collection.",
                    getFrom == AiringCollection.DeletedCollection ? "deleted" : (getFrom == AiringCollection.ExpiredCollection ? "expired" : "current/expired")));

            return airing;
        }

        private Airing GetFromExpiredCollection(IMongoQuery query)
        {
            var airing = _expiredCollection.FindOne(query);

            return airing;
        }

        public bool IsAiringDeleted(string assetId)
        {
            var query = Query.EQ("AssetId", assetId);

            var airing = _deletedCollection.FindOne(query);

            return (airing != null);
        }

        public bool IsAiringExpired(string assetId)
        {
            var query = Query.EQ("AssetId", assetId);

            var airing = _expiredCollection.FindOne(query);

            return (airing != null);
        }

        public bool IsAiringExists(string assetId)
        {
            var query = Query.EQ("AssetId", assetId);

            var airing = _currentCollection.FindOne(query);

            return (airing != null);
        }

        public IQueryable<Airing> GetNonExpiredBy(int titleId, DateTime cutOffDateTime, bool isSeries = false)
        {
            IMongoQuery query;

            if (isSeries)
            {
                query = Query.And(
                   Query.EQ("Title.Series._id", titleId),
                   Query.GTE("Flights.End", cutOffDateTime.ToUniversalTime()));
            }
            else
            {
                query = Query.And(
                   Query.EQ("Title.TitleIds.Value", titleId.ToString()),
                   Query.EQ("Title.TitleIds.Authority", "Turner"),
                   Query.GTE("Flights.End", cutOffDateTime.ToUniversalTime()));
            }

            var airings = _currentCollection.Find(query).AsQueryable();

            return airings;
        }

        public IQueryable<Airing> GetNonExpiredBy(IList<int> titleIds, IQueryable<string> queueNames, DateTime cutOffDateTime)
        {
            if (!titleIds.Any())
                return new List<Airing>().AsQueryable();

            var titleIdsQuery = new List<IMongoQuery>();

            foreach (var titleId in titleIds)
            {
                var document = new BsonDocument();

                document.Add("Authority", "Turner");
                document.Add("Value", titleId.ToString());

                var titleIdQuery = Query.ElemMatch("Title.TitleIds", Query.And(
                    Query.EQ("Authority", "Turner"),
                    Query.EQ("Value", titleId.ToString())));

                titleIdsQuery.Add(titleIdQuery);
            }

            var query = Query.And(
                   Query.Or(titleIdsQuery),
                   Query.In("DeliveredTo", new BsonArray(queueNames)),
                   Query.GTE("Flights.End", cutOffDateTime.ToUniversalTime()));

            var airings = _currentCollection.Find(query).AsQueryable();

            return airings;
        }

        public IQueryable<Airing> GetNonExpiredBy(string destination, DateTime cutOffDateTime)
        {
            var query = Query.And(
                Query.EQ("Flights.Destinations.Name", destination),
                Query.GTE("Flights.End", cutOffDateTime.ToUniversalTime()));

            var airings = _currentCollection.Find(query).AsQueryable();

            return airings;
        }

        public IQueryable<Airing> GetNonExpiredBy(DateTime cutOffDateTime)
        {
            var query = Query.GTE("Flights.End", cutOffDateTime.ToUniversalTime());
            var airings = _currentCollection.Find(query).AsQueryable();
            return airings;
        }

        public List<Airing> GetBy(string brand, string destination, DateTime startDate, DateTime endDate, string airingStatus = "")
        {
            var query = Query.And(
               Query.EQ("Flights.Destinations.Name", destination),
               Query.EQ("Network", brand),
               Query.LTE("Flights.Start", endDate.ToUniversalTime()),
               Query.GTE("Flights.End", startDate.ToUniversalTime()));


            airingStatus = string.IsNullOrEmpty(airingStatus) ? "active" : airingStatus.ToLower();

            var airings = new List<Airing>();

            if (airingStatus.Contains("active"))
                airings.AddRange(_currentCollection.Find(query).AsQueryable());

            if (airingStatus.Contains("expired"))            
                airings.AddRange(_expiredCollection.Find(query).AsQueryable());
            
            return airings.ToList();
        }

        public IQueryable<Airing> GetByMediaId(string mediaId)
        {
            var query = Query.EQ("MediaId", mediaId);
            var airings = _currentCollection.Find(query).AsQueryable();

            if (airings == null||!airings.Any())
                throw new AiringNotFoundException(string.Format("Airing not found for media id {0}.", mediaId));

            return airings;
        }

        public IList<Airing> GetAiringsByMediaId(string mediaId, DateTime startDate, DateTime endDate)
        {
            var airings = new List<Airing>();
            var query = Query.And(Query.EQ("MediaId", mediaId));
            bool isAiringsExists = _currentCollection.Find(query).Any();

            if (!isAiringsExists)
            {
                throw new AiringNotFoundException(string.Format("No Airings found for media id {0}.", mediaId));
            }
            else
            {
                query = Query.And(Query.EQ("MediaId", mediaId),
                        Query.LTE("Flights.Start", endDate.ToUniversalTime()),
                        Query.GTE("Flights.End", startDate.ToUniversalTime()));

                airings.AddRange(_currentCollection.Find(query).AsQueryable());
                if (!airings.Any())
                {
                    throw new AiringNotFoundException(string.Format("No Airings found for media id {0} with selected date range.", mediaId));
                }
            }
            return airings.ToList();
        }
    }

 
   
}