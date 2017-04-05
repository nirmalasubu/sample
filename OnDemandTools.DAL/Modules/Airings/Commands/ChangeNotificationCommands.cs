using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Airings.Model;
using System.Collections.Generic;
using System.Linq;

namespace OnDemandTools.DAL.Modules.Airings.Commands
{
    public class ChangeNotificationCommands : IChangeNotificaitonCommands
    {
        private readonly MongoCollection<Airing> _collection;

        public ChangeNotificationCommands(IODTDatastore connection)
        {
            MongoDatabase database = connection.GetDatabase();

            _collection = database.GetCollection<Airing>("currentassets");
        }

        public void Save(string airingId, IEnumerable<ChangeNotification> changeNotifications)
        {
            IMongoQuery query = Query.EQ("AssetId", airingId);

            List<UpdateBuilder> updateValues = new List<UpdateBuilder>();
            updateValues.Add(Update.PullAllWrapped("DeliveredTo", changeNotifications.Select(e => e.QueueName)));
            updateValues.Add(Update.PullAllWrapped("IgnoredQueues", changeNotifications.Select(e => e.QueueName)));
            updateValues.Add(Update.PushAllWrapped("ChangeNotifications", changeNotifications));
         

            IMongoUpdate update = Update.Combine(updateValues);
            _collection.Update(query, update);
        }
    }

    public interface IChangeNotificaitonCommands
    {
        void Save(string airingId, IEnumerable<ChangeNotification> changeNotifications);
    }
}
