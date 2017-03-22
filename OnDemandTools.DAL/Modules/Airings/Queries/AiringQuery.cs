using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using OnDemandTools.DAL.Helpers;
using OnDemandTools.DAL.Modules.Airings.Model;

namespace OnDemandTools.DAL.Modules.Airings.Queries
{
    public class AiringQuery
    {
        protected readonly MongoCollection<Airing> Collection;

        public AiringQuery(MongoCollection<Airing> collection)
        {
            Collection = collection;
        }

        public List<string> GetAllAiringIds()
        {
           return Collection.FindAllAs<Airing>().Select(e => e.AssetId).ToList();
        }

        public IEnumerable<Airing> GetDeliverToBy(string queueName, int limit)
        {
            var query = Query.And(Query.NotIn("DeliveredTo", new BsonArray { queueName }),
                                  Query.In("DeliverTo", new BsonArray { queueName }),
                                  Query.NotIn("IgnoredQueues", new BsonArray { queueName }));

            var airings = Collection.FindAs<Airing>(query);

            airings.Limit = limit;

            return airings.AsEnumerable();
        }

        public bool IsAiringDistributed(string airingId, string queueName)
        {
            var airing = Collection.FindOne(Query.And(Query.EQ("AssetId", airingId), Query.EQ("DeliveredTo", queueName)));

            return airing != null;
        }

        public IEnumerable<Airing> GetBy(string jsonQuery, int hoursOut, IList<string> queueNames, bool includeEndDate = false)
        {
            var queryDoc = Create(jsonQuery);

            var targetDate = DateTime.UtcNow.AddHours(hoursOut);

            var query = Query.And(
                queryDoc,
                Query.NotIn("DeliveredTo", new BsonArray(queueNames)),
                Query.NotIn("DeliverTo", new BsonArray(queueNames)),
                Query.NotIn("IgnoredQueues", new BsonArray(queueNames)),
                Query.ElemMatch("Flights",
                                includeEndDate
                                    ? Query.And(Query.LTE("Start", targetDate), Query.GTE("End", DateTime.UtcNow))
                                    : Query.And(Query.LTE("Start", targetDate))));

            var airings = Collection.FindAs<Airing>(query);

            return airings.AsEnumerable();
        }

        public IEnumerable<Airing> GetBy(string jsonQuery, IList<string> airingIds)
        {
            var queryDoc = Create(jsonQuery);

            var airingIdQuery = Query.In("AssetId", new BsonArray().AddRange(airingIds));

            var query = Query.And(queryDoc, airingIdQuery);

            var airings = Collection.FindAs<Airing>(query);

            return airings.AsEnumerable();
        }

        protected static QueryDocument Create(string jsonQuery)
        {
            var converter = new JsonToQueryConverter();

            return converter.Convert(jsonQuery);
        }

        public void DeleteIgnoredQueue(string queueName)
        {

            var filter = Query.In("IgnoredQueues", new BsonArray { queueName });
            var update = new UpdateDocument
                             {
                                 {"$pull", new BsonDocument("IgnoredQueues", queueName)}
                             };

            Collection.Update(filter, update, UpdateFlags.Multi);
        }
    }
}