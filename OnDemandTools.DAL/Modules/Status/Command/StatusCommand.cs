using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using DLModel = OnDemandTools.DAL.Modules.Status.Model;

namespace OnDemandTools.DAL.Modules.Status.Command
{
    public class StatusCommand : IStatusCommand
    {

        private readonly MongoDatabase _database;

        public StatusCommand(IODTDatastore connection)
        {
            _database = connection.GetDatabase();
        }

        public void Delete(string id)
        {
            var collection = _database.GetCollection<DLModel.Status>("airingstatus");

            var query = Query<DLModel.Status>.EQ(e => e.Id, new ObjectId(id));

            collection.Remove(query);
        }
    }
}
