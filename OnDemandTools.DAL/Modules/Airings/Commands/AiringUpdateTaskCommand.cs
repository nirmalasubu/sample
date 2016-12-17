using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Airings.Model;
using System.Collections.Generic;

namespace OnDemandTools.DAL.Modules.Airings.Commands
{
    public class AiringUpdateTaskCommand : ITaskUpdater
    {
        private readonly MongoCollection<Airing> _currentAirings;

        public AiringUpdateTaskCommand(IODTDatastore connection)
        {
            var database = connection.GetDatabase();

            _currentAirings = database.GetCollection<Airing>("currentassets");
        }

        public void UpdateFor(List<string> airingIds, List<string> tasks)
        {
            var filter = Query.In("AssetId", new BsonArray(airingIds));

            foreach (var task in tasks)
            {
                _currentAirings.Update(filter, Update.AddToSet("Tasks", task), UpdateFlags.Multi);
            }
        }
    }
}