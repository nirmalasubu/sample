using MongoDB.Driver;
using MongoDB.Driver.Linq;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.AiringId;
using OnDemandTools.DAL.Modules.AiringId.Model;
using System.Linq;

namespace OnDemandTools.Components.AiringId.Queries
{
    public class GetAiringIdsQuery : IGetAiringIdsQuery
    {
        private readonly MongoDatabase _database;

        public GetAiringIdsQuery(IODTDatastore connection)
        {
            _database = connection.GetDatabase();
        }

        public IQueryable<CurrentAiringId> Get()
        {
            return _database.GetCollection<CurrentAiringId>("CurrentAiringId").AsQueryable<CurrentAiringId>();
        }

        public CurrentAiringId Get(string prefix)
        {
            return _database.GetCollection<CurrentAiringId>("CurrentAiringId")
                .AsQueryable<CurrentAiringId>()
                .FirstOrDefault(a => a.Prefix == prefix);
        }
    }
}