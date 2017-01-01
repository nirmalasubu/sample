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
        /// Gets the non expired airings based given criteria.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="cutOffDateTime">The cut off date time.</param>
        /// <returns></returns>
        List<BLModel.Airing> GetNonExpiredBy(string destination, DateTime cutOffDateTime);

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

        /// <summary>
        /// Saves the specified airing.
        /// </summary>
        /// <param name="airing">The airing.</param>
        /// <param name="hasImmediateDelivery">if set to <c>true</c> [has immediate delivery].</param>
        /// <param name="updateHistorical">if set to <c>true</c> [update historical].</param>
        /// <returns></returns>
        BLModel.Airing Save(BLModel.Airing airing, bool hasImmediateDelivery, bool updateHistorical);

        /// <summary>
        /// Deletes the specified airing.
        /// </summary>
        /// <param name="airing">The airing.</param>
        /// <returns></returns>
        BLModel.Airing Delete(BLModel.Airing airing);


        /// <summary>
        /// Push the list of airings to the specified queue
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="airingIds">The airing ids.</param>
        void PushToQueue(string queueName, IList<string> airingIds);

        /// <summary>
        /// Push the list of airings to designated queues. Queue designation
        /// is determined through several factors, like queue criteria, airing flight window etc.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="airingIds">The airing ids.</param>
        void PushToQueues(IList<string> airingIds);

        /// <summary>
        /// Augments media identifier to the given airing
        /// </summary>
        /// <param name="airing">The airing.</param>
        void AugmentMediaId(ref BLModel.Airing airing);


        /// <summary>
        /// Updates each of the airing with the provided list of tasks
        /// </summary>
        /// <param name="airingIds">The airing ids.</param>
        /// <param name="tasks">The tasks.</param>
        void UpdateTask(List<string> airingIds, List<string> tasks);

        /// <summary>
        /// Gets the airings by media identifier. Further filtering is performed on the resulting airings based on flight window
        /// </summary>
        /// <param name="mediaId">The media identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        List<BLModel.Airing> GetAiringsByMediaId(string mediaId, DateTime startDate, DateTime endDate);


        /// <summary>
        /// Gets airings based on the given criteria
        /// </summary>
        /// <param name="brand">The brand.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="airingStatus">The airing status.</param>
        /// <returns></returns>
        List<BLModel.Airing> GetBy(string brand, string destination, DateTime startDate, DateTime endDate, string airingStatus = "");


        /// <summary>
        /// Append file information to the provided airing
        /// </summary>
        /// <param name="airing">The airing.</param>
        List<BLModel.Alternate.Long.File>  RetrieveFile(BLModel.Alternate.Long.Airing airing);
    }
}
