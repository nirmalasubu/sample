using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Airings;
using OnDemandTools.DAL.Modules.Airings.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnDemandTools.DAL.Modules.Queue.Command
{
    public class QueueCommand : IQueueCommand
    {
        private readonly MongoDatabase _database;
        private readonly IGetModifiedAiringQuery _modifiedAiringQuery;
        private readonly MongoCollection<Airing> _currentAirings;
        private readonly MongoCollection<Airing> _deleteAirings;
        private readonly MongoCollection<Model.Queue> _queuesCollection;

        public QueueCommand(IODTDatastore connection, IGetModifiedAiringQuery modifiedAiringQuery)
        {
            _database = connection.GetDatabase();
            _modifiedAiringQuery = modifiedAiringQuery;
            _currentAirings = _database.GetCollection<Airing>("currentassets");
            _deleteAirings = _database.GetCollection<Airing>("deletedasset");

            _queuesCollection = _database.GetCollection<Model.Queue>("DeliveryQueue");
        }

        private IMongoQuery GetAiringIdsQueryByExactTitleMatch(IEnumerable<Airing> currentAirings, IEnumerable<string> titleIds)
        {

            var airingIds = (from airing in currentAirings
                             let airingTitleIds = airing.Title.TitleIds.Where(e => e.Authority == "Turner").Select(e => e.Value)
                             where airingTitleIds.SequenceEqual(titleIds)
                             select airing.AssetId).ToList();

            return !airingIds.Any() ? null : Query.In("AssetId", new BsonArray(airingIds));
        }

        private IMongoQuery GetAiringIdsQueryByExactCIDMatch(IEnumerable<Airing> currentAirings, IEnumerable<string> contentIds)
        {

            var airingIds = (from airing in currentAirings
                             let airingContentIds = airing.Versions.Select(e => e.ContentId)
                             where airingContentIds.SequenceEqual(contentIds)
                             select airing.AssetId).ToList();

            return !airingIds.Any() ? null : Query.In("AssetId", new BsonArray(airingIds));
        }

        private void Reset(string queueName, IMongoQuery filter, bool resetDeletedAirings = true)
        {
            _currentAirings.Update(filter, Update.Pull("DeliveredTo", queueName), UpdateFlags.Multi);

            if (resetDeletedAirings)
                _deleteAirings.Update(filter, Update.Pull("DeliveredTo", queueName), UpdateFlags.Multi);
        }

        private void ResetFor(string queueName, IList<string> airingIds)
        {
            var filter = Query.In("AssetId", new BsonArray(airingIds));

            Reset(queueName, filter);
        }


        public void ResetFor(IList<string> queueNames, IList<int> titleIds, string destinationCode)
        {
            var titleIdsInString = titleIds.Select(titleId => titleId.ToString()).ToList();

            foreach (var queueName in queueNames)
            {
                var query = Query.In("DeliveredTo", new BsonArray(new List<string> { queueName }));

                query = titleIdsInString.Aggregate(query, (current, titleId) => Query.And(current, Query.EQ("Title.TitleIds.Value", BsonValue.Create(titleId))));

                if (!string.IsNullOrEmpty(destinationCode))
                {
                    query = Query.And(query, Query.EQ("Flights.Destinations.Name", destinationCode));
                }

                var currentAirings = _currentAirings.Find(query).ToList();

                if (!currentAirings.Any()) continue;

                var airingIdsQuery = GetAiringIdsQueryByExactTitleMatch(currentAirings, titleIdsInString);

                if (airingIdsQuery != null)
                    Reset(queueName, airingIdsQuery, false);
            }
        }
        public void ResetFor(IList<string> queueNames, IList<string> contentIds, string destinationCode)
        {
            var contentIdsInString = contentIds.Select(cid => cid).ToList();

            foreach (var queueName in queueNames)
            {
                var query = Query.In("DeliveredTo", new BsonArray(new List<string> { queueName }));

                query = contentIdsInString.Aggregate(query, (current, contentId) => Query.And(current, Query.EQ("Versions.ContentId", BsonValue.Create(contentId))));

                if (!string.IsNullOrEmpty(destinationCode))
                {
                    query = Query.And(query, Query.EQ("Flights.Destinations.Name", destinationCode));
                }

                var currentAirings = _currentAirings.Find(query).ToList();

                if (!currentAirings.Any()) continue;

                var airingIdsQuery = GetAiringIdsQueryByExactCIDMatch(currentAirings, contentIdsInString);

                if (airingIdsQuery != null)
                    Reset(queueName, airingIdsQuery, false);
            }
        }


        public void ResetFor(IList<string> queueNames, string airingId, string destinationCode)
        {
            foreach (var queueName in queueNames)
            {
                var query = Query.In("DeliveredTo", new BsonArray(new List<string> { queueName }));

                query = Query.EQ("AssetId", airingId);

                if (!string.IsNullOrEmpty(destinationCode))
                {
                    query = Query.And(query, Query.EQ("Flights.Destinations.Name", destinationCode));
                }

                var currentAirings = _currentAirings.Find(query).ToList();

                if (!currentAirings.Any()) continue;
                else
                    Reset(queueName, query, false);
            }
        }


        public void ResetFor(IList<string> queueNames, IList<int> titleIds)
        {
            if (!queueNames.Any() || !titleIds.Any())
                return;

            var airingIds = _modifiedAiringQuery
                    .GetNonExpiredBy(titleIds, queueNames.AsQueryable(), DateTime.Now.ToUniversalTime())
                    .Select(a => a.AssetId).ToList();

            foreach (var queueName in queueNames)
            {
                ResetFor(queueName, airingIds);
            }
        }

        public void ResetFor(IList<string> queueNames, IList<string> airingIds)
        {
            if (!queueNames.Any() || !airingIds.Any())
                return;

            foreach (var queueName in queueNames)
            {
                ResetFor(queueName, airingIds);
            }
        }

        public void UpdateQueueProcessedTime(string name)
        {
            _queuesCollection.Update(Query.EQ("Name", name), Update.Set("ProcessedDateTime", DateTime.UtcNow));
        }

       
    }
}
