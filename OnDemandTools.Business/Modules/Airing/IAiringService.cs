using OnDemandTools.DAL.Modules.Airings;
using System;
using System.Collections.Generic;
using BLModel = OnDemandTools.Business.Modules.Airing.Model;

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
        List<BLModel.Airing> GetNonExpiredBy(int titleId, DateTime cutOffDateTime, bool isSeries = false);

        /// <summary>
        /// Gets airings by media identifier.
        /// </summary>
        /// <param name="mediaId">The media identifier.</param>
        /// <returns></returns>
        List<BLModel.Airing> GetByMediaId(string mediaId);


        /// <summary>
        /// Gets airings by airing identifier.
        /// </summary>
        /// <param name="assetId">The asset identifier.</param>
        /// <param name="getFrom">The get from.</param>
        /// <returns></returns>
        BLModel.Airing GetBy(string assetId, AiringCollection getFrom = AiringCollection.CurrentOrExpiredCollection);

        /// <summary>
        /// Determines whether [is airing exists] [the specified asset identifier].
        /// </summary>
        /// <param name="assetId">The asset identifier.</param>
        /// <returns>
        ///   <c>true</c> if [is airing exists] [the specified asset identifier]; otherwise, <c>false</c>.
        /// </returns>
        bool IsAiringExists(string assetId);
    }
}
