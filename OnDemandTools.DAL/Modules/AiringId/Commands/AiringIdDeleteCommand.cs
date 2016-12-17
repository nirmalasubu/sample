using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.AiringId.Model;

namespace OnDemandTools.DAL.Modules.AiringId.Commands
{
    public class AiringIdDeleteCommand : IAiringIdDeleteCommand
    {  
        private readonly MongoDatabase _database;

        public AiringIdDeleteCommand(IODTDatastore connection)
        {
            _database = connection.GetDatabase();
        }

        public void Delete(ObjectId Id)
        {
            var collection = _database.GetCollection<CurrentAiringId>("CurrentAiringId");

            var query = Query<CurrentAiringId>.EQ(e => e.Id, Id);

            collection.Remove(query);
        }
    }
}