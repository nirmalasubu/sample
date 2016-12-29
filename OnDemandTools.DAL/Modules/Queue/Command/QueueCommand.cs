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

        public QueueCommand(IODTDatastore connection, IGetModifiedAiringQuery modifiedAiringQuery)
        {
            _database = connection.GetDatabase();
            _modifiedAiringQuery = modifiedAiringQuery;
            _currentAirings = _database.GetCollection<Airing>("currentassets");
            _deleteAirings = _database.GetCollection<Airing>("deletedasset");
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



        private IMongoQuery GetAiringIdsQueryByExactTitleMatch(IEnumerable<Airing> currentAirings, IEnumerable<string> titleIds)
        {

            var airingIds = (from airing in currentAirings
                             let airingTitleIds = airing.Title.TitleIds.Where(e => e.Authority == "Turner").Select(e => e.Value)
                             where airingTitleIds.SequenceEqual(titleIds)
                             select airing.AssetId).ToList();

            return !airingIds.Any() ? null : Query.In("AssetId", new BsonArray(airingIds));
        }

        private void Reset(string queueName, IMongoQuery filter, bool resetDeletedAirings = true)
        {
            _currentAirings.Update(filter, Update.Pull("DeliveredTo", queueName), UpdateFlags.Multi);

            if (resetDeletedAirings)
                _deleteAirings.Update(filter, Update.Pull("DeliveredTo", queueName), UpdateFlags.Multi);
        }

    }
}
