using OnDemandTools.DAL.Modules.Airings;
using System;
using System.Collections.Generic;
using System.Linq;
using BLModel = OnDemandTools.Business.Modules.ModifiedTitles.Model;
using DQModel = OnDemandTools.DAL.Modules.Queue.Model;


namespace OnDemandTools.Business.Modules.ModifiedTitles
{
    public interface IModifiedTitlesService
    {
        /// <summary>
        /// Send title change notification to subscribed queues.
        /// </summary>
        /// <param name="queues">The Queues.</param>
        /// <param name="sinceTitleBSONId">Last Modified Title Id.</param>
        /// <param name="limit"></param>
        /// <returns></returns>
        String Update(IQueryable<DQModel.Queue> queues, String sinceTitleBSONId, int limit);

        /// <summary>
        /// Retrieve titles modified since the date on which the given titleid was modified.
        /// </summary>
        /// <param name="titleId">The title identifier.</param>
        /// <returns></returns>
        List<BLModel.UpdatedTitle> GetTitleIdsModifiedAfter(string titleId);

        /// <summary>
        /// Returns the max _id (title id) from the titles collection, 
        /// where the date time part of the _id is less than or equal to the 
        /// passed in date time UTC
        /// 
        /// This call to Flow will only return one Title object.
        /// </summary>
        /// <param name="lastTitleProcessedDateTime">Modified date time.</param>
        /// <returns></returns>
        string GetLastModifiedTitleIdOnOrBefore(DateTime lastTitleProcessedDateTime);

    }
}
