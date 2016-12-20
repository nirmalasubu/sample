using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.AiringId.Model;
using System;
using System.Linq;

namespace OnDemandTools.DAL.Modules.AiringId.Queries
{
    public class GetLastAiringIdQuery : IGetLastAiringIdQuery
    {
        private readonly MongoDatabase _database;

        public GetLastAiringIdQuery(IODTDatastore connection)
        {
            _database = connection.GetDatabase();
        }

        public CurrentAiringId Get(string prefix)
        {
            var query = _database.GetCollection<CurrentAiringId>("CurrentAiringId").AsQueryable<CurrentAiringId>();

            try
            {
                return query.First(a => a.Prefix == prefix);
            }
            catch (Exception ex)
            {
                var message = string.Format("An airing id prefix does not exist for '{0}'. You must create and airing id prefix before sending this request.", prefix);

                throw new Exception(message, ex); ;
            }
        }
    }
}