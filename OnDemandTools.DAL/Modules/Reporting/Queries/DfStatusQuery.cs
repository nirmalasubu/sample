using System;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Modules.Reporting.Model;
using OnDemandTools.DAL.Database;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using OnDemandTools.Common.Configuration;

namespace OnDemandTools.DAL.Modules.Reporting.Queries
{
    public class DfStatusQuery : IDfStatusQuery
    {
        private readonly MongoDatabase _database;

        public DfStatusQuery(AppSettings configuration)
        {
            _database = new ODTDatastore(configuration).GetDatabase();
        }

        /// <summary>
        /// Get the top 1000 records by modified time
        /// </summary>
        /// <param name="modifedTime"></param>
        /// <returns></returns>
        public IQueryable<DF_Status> GetDfStatusEnumByModifiedDate(DateTime modifedTime)
        {
            var query = Query.LTE("ModifiedDate", modifedTime);

            return _database.GetCollection<DF_Status>("DFStatus")
                .Find(query)
                .SetLimit(1000)
                .SetSortOrder(SortBy.Descending("ModifiedDate"))
                .AsQueryable();
        }
    }
}
