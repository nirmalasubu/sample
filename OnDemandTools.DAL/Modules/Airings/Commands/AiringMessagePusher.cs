using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Helpers;
using System;
using System.Collections.Generic;
using OnDemandTools.DAL.Modules.Queue.Model;
using DLModel = OnDemandTools.DAL.Modules.Airings.Model;

namespace OnDemandTools.DAL.Modules.Airings.Commands
{
    public class AiringMessagePusher : IAiringMessagePusher
    {
        private readonly MongoCollection<DLModel.Airing> _currentAirings;
        private readonly MongoCollection<DLModel.Airing> _deletedAirings;

        public AiringMessagePusher(IODTDatastore connection)
        {
            var database = connection.GetDatabase();

            _currentAirings = database.GetCollection<DLModel.Airing>(DataStoreConfiguration.CurrentAssetsCollection);
            _deletedAirings = database.GetCollection<DLModel.Airing>(DataStoreConfiguration.DeletedAssetsCollection);
        }

        public void PushBy(DeliverCriteria criteria)
        {
            var converter = new JsonToQueryConverter();

            IMongoQuery filter;

            switch (criteria.WindowOption)
            {
                case (int)WindowOption.StartDate:
                    filter = Query.And(converter.Convert(criteria.Query),
                       Query.GTE("Flights.Start", criteria.StartDateTime),
                       Query.LTE("Flights.Start", criteria.EndDateTime));
                    break;
                case (int)WindowOption.InFlightDuring:
                    filter = Query.And(converter.Convert(criteria.Query),
                       Query.LTE("Flights.Start", criteria.EndDateTime),
                       Query.GTE("Flights.End", criteria.StartDateTime));
                    break;
                default:
                    throw new Exception(string.Format("Invalid enumeration for WindowOption: {0}", criteria.WindowOption));
            }

            Push(criteria.Name, filter);
        }

        public void PushBy(string queueName, IList<string> airingIds)
        {
            var filter = Query.In("AssetId", new BsonArray(airingIds));

            Push(queueName, filter);
        }

        public void PushBy(string queueName, string query, IList<string> airingIds)
        {
            var converter = new JsonToQueryConverter();

            var filter = Query.And(
                converter.Convert(query),
                Query.In("AssetId", new BsonArray(airingIds)));

            Push(queueName, filter);
        }

        public void PushBy(string queueName, string query, int hoursOut, IList<string> airingIds)
        {
            var converter = new JsonToQueryConverter();

            var targetDate = DateTime.UtcNow.AddHours(hoursOut);

            var filter = Query.And(
                converter.Convert(query),

                Query.ElemMatch("Flights", Query.Or(
                    Query.GT("Start", targetDate),
                    Query.LT("End", DateTime.UtcNow))),

                Query.In("AssetId", new BsonArray(airingIds)));

            Push(queueName, filter);
        }

        private void Push(string queueName, IMongoQuery filter)
        {            
            _currentAirings.Update(filter, Update.Push("DeliverTo", queueName), UpdateFlags.Multi);
            _currentAirings.Update(filter, Update.Pull("DeliveredTo", queueName), UpdateFlags.Multi);
            _currentAirings.Update(filter, Update.Pull("IgnoredQueues", queueName), UpdateFlags.Multi);

            _deletedAirings.Update(filter, Update.Push("DeliverTo", queueName), UpdateFlags.Multi);
            _deletedAirings.Update(filter, Update.Pull("DeliveredTo", queueName), UpdateFlags.Multi);
            _deletedAirings.Update(filter, Update.Pull("IgnoredQueues", queueName), UpdateFlags.Multi);
        }

    }

    public enum WindowOption
    {
        StartDate, InFlightDuring
    }
}