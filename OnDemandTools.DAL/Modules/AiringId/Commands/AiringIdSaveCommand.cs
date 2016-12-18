using System;
using System.Security.Principal;
using MongoDB.Driver;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.AiringId.Model;

namespace OnDemandTools.DAL.Modules.AiringId.Commands
{
    public class AiringIdSaveCommand : IAiringIdSaveCommand
    {  
        private readonly MongoDatabase _database;

        public AiringIdSaveCommand(IODTDatastore connection)
        {
            _database = connection.GetDatabase();
        }

        public CurrentAiringId Save(CurrentAiringId currentAiringId)
        {
            var collection = _database.GetCollection<CurrentAiringId>("CurrentAiringId");
            currentAiringId.ModifiedDateTime = DateTime.UtcNow;            
            collection.Save(currentAiringId);
            return currentAiringId;
        }
    }
}