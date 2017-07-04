using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.AiringId.Model;
using System.Collections.Generic;

namespace OnDemandTools.DAL.Modules.Destination.Command
{
    public class DestinationCommand : IDestinationCommand
    {
        private readonly MongoDatabase _database;

        public DestinationCommand(IODTDatastore connection)
        {
            _database = connection.GetDatabase();
        }

        public DAL.Modules.Destination.Model.Destination Save(DAL.Modules.Destination.Model.Destination destination)
        {
            var collection = _database.GetCollection<DAL.Modules.Destination.Model.Destination>("Destination");

            collection.Save(destination);

            return destination;
        }

        public void Delete(string id)
        {
            var collection = _database.GetCollection<CurrentAiringId>("Destination");

            var query = Query<DAL.Modules.Destination.Model.Destination>.EQ(e => e.Id, new ObjectId(id));

            collection.Remove(query);
        }

        public void DeleteCategoryByName(string name)
        {

            var collection = _database.GetCollection<DAL.Modules.Destination.Model.Destination>("Destination");
            var filter = Query.EQ("Categories.Name", name);
            collection.Update(filter,Update.Pull("Categories", new BsonDocument() { { "Name", name } }), UpdateFlags.Multi);
            
            
        }
    }
}
;