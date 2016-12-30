using System;
using System.Collections.Generic;

namespace OnDemandTools.Business.Modules.Airing
{
    public interface IAiringService
    {
        /// <summary>
        /// Gets non expired airings based on given criteria.
        /// </summary>
        /// <param name="titleId">The title identifier.</param>
        /// <param name="cutOffDateTime">The cut off date time.</param>
        /// <param name="isSeries">if set to <c>true</c> [is series].</param>
        /// <returns></returns>
        List<Model.Airing> GetNonExpiredBy(int titleId, DateTime cutOffDateTime, bool isSeries = false);

        /// <summary>
        /// Gets airings by media identifier.
        /// </summary>
        /// <param name="mediaId">The media identifier.</param>
        /// <returns></returns>
        List<Model.Airing> GetByMediaId(string mediaId);
    }
}
