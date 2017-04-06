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

        #region PUBLIC METHODS
        /// <summary>
        /// Reset package queues with titieIds
        /// </summary>
        /// <param name="queueNames">The queue names.</param>
        /// <param name="titleIds">The title ids .</param>
        /// <param name="destinationCode">The destination code</param>
        public void ResetFor(IList<string> queueNames, IList<int> titleIds, string destinationCode)
        {
            var titleIdsInString = titleIds.Select(titleId => titleId.ToString()).ToList();

            foreach (var queueName in queueNames)
            {
                IMongoQuery query = Query.Or(Query.In("DeliveredTo", new BsonArray(new List<string> { queueName })),
                                            Query.In("ChangeNotifications.QueueName", new BsonArray(new List<string> { queueName })),
                                            Query.In("IgnoredQueues", new BsonArray(new List<string> { queueName })));

                query = titleIdsInString.Aggregate(query, (current, titleId) => Query.And(current, Query.EQ("Title.TitleIds.Value", BsonValue.Create(titleId))));

                if (!string.IsNullOrEmpty(destinationCode))
                {
                    query = Query.And(query, Query.EQ("Flights.Destinations.Name", destinationCode));
                }

                var currentAirings = _currentAirings.Find(query).ToList();

                if (!currentAirings.Any()) continue;

                var airingIdsQuery = GetAiringIdsQueryByExactTitleMatch(currentAirings, titleIdsInString);

                if (airingIdsQuery != null)
                {
                    ResetQueueInMongo(queueName, airingIdsQuery, ChangeNotificationType.Package.ToString(), false);
                }

            }
        }

        /// <summary>
        ///reset package queues with contentIds 
        /// </summary>
        /// <param name="queueNames">The queue names.</param>
        /// <param name="contentIds">The content ids .</param>
        /// <param name="destinationCode">The destination code</param>
        public void ResetFor(IList<string> queueNames, IList<string> contentIds, string destinationCode)
        {
            var contentIdsInString = contentIds.Select(cid => cid).ToList();

            foreach (var queueName in queueNames)
            {
                IMongoQuery query = Query.Or(Query.In("DeliveredTo", new BsonArray(new List<string> { queueName })),
                                            Query.In("ChangeNotifications.QueueName", new BsonArray(new List<string> { queueName })),
                                            Query.In("IgnoredQueues", new BsonArray(new List<string> { queueName })));


                query = contentIdsInString.Aggregate(query, (current, contentId) => Query.And(current, Query.EQ("Versions.ContentId", BsonValue.Create(contentId))));

                if (!string.IsNullOrEmpty(destinationCode))
                {
                    query = Query.And(query, Query.EQ("Flights.Destinations.Name", destinationCode));
                }

                var currentAirings = _currentAirings.Find(query).ToList();

                if (!currentAirings.Any()) continue;

                var airingIdsQuery = GetAiringIdsQueryByExactCIDMatch(currentAirings, contentIdsInString);

                if (airingIdsQuery != null)
                    ResetQueueInMongo(queueName, airingIdsQuery, ChangeNotificationType.Package.ToString(), false);
            }
        }

        /// <summary>
        /// reset package queues with airingId
        /// </summary>
        /// <param name="queueNames">The queue names.</param>
        /// <param name="airingId">The airing id.</param>
        /// <param name="destinationCode">The destination code</param>
        public void ResetFor(IList<string> queueNames, string airingId, string destinationCode)
        {
            IMongoQuery query = Query.EQ("AssetId", airingId);

            if (!string.IsNullOrEmpty(destinationCode))
            {
                query = Query.And(query, Query.EQ("Flights.Destinations.Name", destinationCode));
            }

            var currentAiring = _currentAirings.FindOne(query);
            if (currentAiring == null) return;

            IList<ChangeNotification> changeNotifications =GetChangeNotifications(queueNames, currentAiring);
            List<UpdateBuilder> updateValues = new List<UpdateBuilder>();
            updateValues.Add(Update.PullAllWrapped("DeliveredTo", changeNotifications.Select(e => e.QueueName)));
            updateValues.Add(Update.PullAllWrapped("IgnoredQueues", changeNotifications.Select(e => e.QueueName)));
            updateValues.Add(Update.PushAllWrapped("ChangeNotifications", changeNotifications.AsEnumerable()));

            IMongoUpdate update = Update.Combine(updateValues);
            _currentAirings.Update(query, update);
        }

        /// <summary>
        /// reset queues with titleIds
        /// </summary>
        /// <param name="queueNames">The queue names.</param>
        /// <param name="titleIds">The title ids.</param>
        /// <param name="changeNotificationType">file/title</param>
        public void ResetFor(IList<string> queueNames, IList<int> titleIds, ChangeNotificationType changeNotificationType)
        {
            if (!queueNames.Any() || !titleIds.Any())
                return;

            var airingIds = _modifiedAiringQuery
                    .GetNonExpiredBy(titleIds, queueNames.AsQueryable(), DateTime.Now.ToUniversalTime())
                    .Select(a => a.AssetId).ToList();


            foreach (var queueName in queueNames)
            {
                ResetQueue(queueName, airingIds, changeNotificationType);
            }
        }

        /// <summary>
        /// reset queues with airingIds
        /// </summary>
        /// <param name="queueNames">The queue names.</param>
        /// <param name="airingIds">The airing ids.</param>
        /// <param name="changeNotificationType">file/title</param>
        public void ResetFor(IList<string> queueNames, IList<string> airingIds, ChangeNotificationType changeNotificationType)
        {
            if (!queueNames.Any() || !airingIds.Any())
                return;

            foreach (var queueName in queueNames)
            {
                ResetQueue(queueName, airingIds, changeNotificationType);
            }
        }

        public void UpdateQueueProcessedTime(string name)
        {
            _queuesCollection.Update(Query.EQ("Name", name), Update.Set("ProcessedDateTime", DateTime.UtcNow));
        }
        #endregion

        #region PRIVATE METHODS

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
  
        private IList<ChangeNotification> GetChangeNotifications(IList<string> queueNames, Airing currentAiring)
        {
            IList<ChangeNotification> changeNotifications = new List<ChangeNotification>();
            foreach (var queueName in queueNames)
            {
                if (currentAiring.DeliveredTo.Contains(queueName) || currentAiring.ChangeNotifications.Select(x => x.QueueName).Contains(queueName)
                    || currentAiring.IgnoredQueues.Contains(queueName))
                {
                    ChangeNotification changenotification = new ChangeNotification();
                    changenotification.QueueName = queueName;
                    changenotification.ChangeNotificationType = ChangeNotificationType.Package.ToString();
                    changeNotifications.Add(changenotification);
                }
            }

            return changeNotifications;
        }

        private void ResetQueue(string queueName, IList<string> airingIds, ChangeNotificationType changeNotificationType)
        {
            IMongoQuery query = Query.Or(Query.In("DeliveredTo", new BsonArray(new List<string> { queueName })),
                                         Query.In("ChangeNotifications.QueueName", new BsonArray(new List<string> { queueName })),
                                         Query.In("IgnoredQueues", new BsonArray(new List<string> { queueName })));
            query = Query.And(query, Query.In("AssetId", new BsonArray(airingIds)));
            ResetQueueInMongo(queueName, query, changeNotificationType.ToString());
        }

        private void ResetQueueInMongo(string queueName, IMongoQuery filter, string changeNotificationType, bool resetDeletedAirings = true)
        {
            List<UpdateBuilder> updateAiringProperties = new List<UpdateBuilder>();
            updateAiringProperties.Add(Update.PullAllWrapped("DeliveredTo", queueName));
            updateAiringProperties.Add(Update.PullAllWrapped("IgnoredQueues", queueName));
            updateAiringProperties.Add(Update.PushAllWrapped("ChangeNotifications",
                                        new ChangeNotification { QueueName = queueName, ChangeNotificationType = changeNotificationType }));

            IMongoUpdate update = Update.Combine(updateAiringProperties);
            _currentAirings.Update(filter, update, UpdateFlags.Multi);
            if (resetDeletedAirings)
                _deleteAirings.Update(filter, Update.Pull("DeliveredTo", queueName), UpdateFlags.Multi);
        }

        #endregion

    }
}
