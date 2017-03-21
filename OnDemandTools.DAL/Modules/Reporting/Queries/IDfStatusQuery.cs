using System;
using System.Linq;
using OnDemandTools.DAL.Modules.Reporting.Model;

namespace OnDemandTools.DAL.Modules.Reporting.Queries
{
    public interface IDfStatusQuery
    {
        /// <summary>
        ///     Get's Top 1000 DF Status by modified date time.
        /// </summary>
        /// <param name="modifedTime">the modified time</param>
        /// <returns></returns>
        IQueryable<DF_Status> GetDfStatusEnumByModifiedDate(DateTime modifedTime);

        /// <summary>
        ///     Get's the DF status by given airingId
        /// </summary>
        /// <param name="airingId">the airingId</param>
        /// <returns></returns>
        IQueryable<DF_Status> GetDfStatuses(string airingId);
    }
}