using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Modules.Reporting.Model;
using OnDemandTools.DAL.Database;
using Microsoft.Extensions.Configuration;
using OnDemandTools.Common.Configuration;

namespace OnDemandTools.DAL.Modules.Reporting.Queries
{
    public class StatusEnumsQuery
    {
        private readonly MongoDatabase _database;

        public StatusEnumsQuery(AppSettings configuration)
        {
            _database = new ODTDatastore(configuration).GetDatabase();
        }

        public IQueryable<DF_StatusEnum> CreateGetStatusEnumsQuery()
        {
            return _database.GetCollection<DF_StatusEnum>("DFStatusEnum").FindAll().AsQueryable();
        }

        public IQueryable<DF_StatusEnum> CreateGetStatusEnumQuery(string value)
        {
            return _database.GetCollection<DF_StatusEnum>("DFStatusEnum").Find(Query.EQ("Value", value)).AsQueryable();
        }
    }
}
