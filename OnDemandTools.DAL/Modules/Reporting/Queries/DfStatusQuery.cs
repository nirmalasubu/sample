using System;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.Common.Configuration;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Reporting.Model;

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
        ///     Get the top 10000 records by modified time
        /// </summary>
        /// <param name="modifedTime"></param>
        /// <returns></returns>
        public IQueryable<DF_Status> GetDfStatusEnumByModifiedDate(DateTime modifedTime)
        {
            var query = Query.LTE("ModifiedDate", modifedTime);

            return _database.GetCollection<DF_Status>("DFStatus")
                .Find(query)
                .SetLimit(10000)
                .SetSortOrder(SortBy.Descending("ModifiedDate"))
                .AsQueryable();
        }

        /// <summary>
        ///     Get's the DF status by given airingId
        /// </summary>
        /// <param name="airingId">the airingId</param>
        /// <returns></returns>
        public IQueryable<DF_Status> GetDfStatuses(string airingId)
        {
            var query = Query.EQ("AssetID", airingId);

            return _database.GetCollection<DF_Status>("DFStatus")
                .Find(query).AsQueryable();
        }
    }
}