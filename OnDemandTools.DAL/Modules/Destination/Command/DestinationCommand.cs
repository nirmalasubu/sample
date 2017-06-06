using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.AiringId.Model;

namespace OnDemandTools.DAL.Modules.Destination.Command
{
    public class DestinationCommand : IDestinationCommand
    {
        private readonly MongoDatabase _database;

        public DestinationCommand(IODTDatastore connection)
        {
            _database = connection.GetDatabase();
        }

        public void Delete(string id)
        {
            var collection = _database.GetCollection<CurrentAiringId>("Destination");

            var query = Query<DAL.Modules.Destination.Model.Destination>.EQ(e => e.Id, new ObjectId(id));

            collection.Remove(query);
        }
    }
}
