﻿using OnDemandTools.DAL.Modules.Airings;
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
        /// Get's the Example Airing by Query
        /// </summary>
        /// <param name="mongoQuery">the mongo query</param>
        /// <returns></returns>
        BLModel.Airing GetExampleBy(string mongoQuery);

        /// <summary>
        /// Gets the airings by deliveredTo and Queue name
        /// </summary>
        /// <param name="queueName">queue name</param>
        /// <param name="limit">not of rows to return</param>
        /// <param name="getFrom">Current or Delete collections</param>
        /// <returns></returns>
        IEnumerable<BLModel.Airing> GetDeliverToBy(string queueName, int limit, AiringCollection getFrom = AiringCollection.CurrentCollection);

        /// <summary>
        /// Checks given airing airingid distributed or not
        /// </summary>
        /// <param name="airingId">the airing id</param>
        /// <param name="queueName">the queue name</param>
        /// <returns></returns>
        bool IsAiringDistributed(string airingId, string queueName, AiringCollection getFrom = AiringCollection.CurrentCollection);

        /// <summary>
        /// Gets the airing by given queue query
        /// </summary>
        /// <param name="jsonQuery">query to retrive airings</param>
        /// <param name="hoursOut">no of hours before</param>
        /// <param name="queueNames">queue name to retrieve</param>
        /// <param name="includeEndDate">Include end date filter?</param>
        /// <param name="getFrom">Current or delete airng</param>
        /// <returns></returns>
        IEnumerable<BLModel.Airing> GetBy(string jsonQuery, int hoursOut, IList<string> queueNames, bool includeEndDate = false, AiringCollection getFrom = AiringCollection.CurrentCollection);
       
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
        /// 
        /// </summary>
        /// <param name="airingId"></param>
        /// <param name="queueName"></param>
        void PushDeliveredTo(string airingId, string queueName, AiringCollection getFrom = AiringCollection.CurrentCollection);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="airingId"></param>
        /// <param name="queueName"></param>
        void PushIgnoredQueueTo(string airingId, string queueName, AiringCollection getFrom = AiringCollection.CurrentCollection);

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
        /// Append file information to the provided airing.
        /// Match is based on INCLUSIVE or - TitleId or Airing or MediaId
        /// </summary>
        /// <param name="airing">The airing.</param>
        void  AppendFile(ref BLModel.Alternate.Long.Airing airing);

        /// <summary>
        /// Append file information to the provided airing.
        /// Match is based on series id
        /// </summary>
        /// <param name="airing">The airing.</param>
        void AppendFileBySeriesId(ref BLModel.Alternate.Long.Airing airing);

        /// <summary>
        /// Gets the title information from Flow and appends it 
        /// to the airing
        /// </summary>
        /// <param name="titleId">The title identifier.</param>
        /// <returns></returns>
        void AppendTitle(ref BLModel.Alternate.Long.Airing airing);

        /// <summary>
        /// Get premiere information from Flow and appends it to the airing
        /// </summary>
        /// <param name="titleId">The title identifier.</param>
        /// <returns></returns>
        void AppendPremiere(ref BLModel.Alternate.Long.Airing airing);

        /// <summary>
        /// Gets series information from Flow and appends it to the
        /// airing
        /// </summary>
        /// <param name="airing">The airing.</param>
        void AppendSeries(ref BLModel.Alternate.Long.Airing airing);


        /// <summary>
        /// Appends destination (formatted) information to the airing. This will
        /// also include destination specific meta data
        /// </summary>
        /// <param name="airing">The airing.</param>
        void AppendDestinations(ref BLModel.Alternate.Long.Airing airing);

        /// <summary>
        /// Get version information from Flow and appends it to the airing
        /// </summary>
        /// <param name="airing">The airing.</param>
        /// <returns></returns>
        void AppendVersion(ref BLModel.Alternate.Long.Airing airing);

        /// <summary>
        /// Appends the video status as true if this airing has at least one video
        /// file registered with it.
        /// 
        /// Here are the conditions:
        ///   1) if there is one record in Files collection that match either the airingid or mediaid (to which this airing belongs), and
        ///   2) the file record has Video=true
        ///   
        /// </summary>
        /// <param name="airing">The airing.</param>
        void AppendStatus(ref BLModel.Alternate.Long.Airing airing);


        /// <summary>
        /// Appends package information to the airing
        /// </summary>
        /// <param name="airing">The airing.</param>
        void AppendPackage(ref BLModel.Alternate.Long.Airing airing, IEnumerable<Tuple<string, decimal>> acceptHeaders);


        /// <summary>
        /// Appends the airing changes
        /// </summary>
        /// <param name="airing">The airing.</param>
        void AppendChanges(ref BLModel.Alternate.Long.Airing airing);

        /// <summary>
        /// Deport Expired airing from current asset
        /// </summary>
        /// <param name="airingDeportGraceDays">The airingDeportGraceDays</param>
        void Deport(int airingDeportGraceDays);

        /// <summary>
        /// Deletes the package mapped to airing.
        /// </summary>
        /// <param name="airingId">The package.</param>
        /// <param name="updateHistorical">if set to <c>true</c> [update historical].</param>
        /// <returns>true/false</returns>
        bool DeleteAiringMappedPackages(string airingId, bool updateHistorical = true);

        /// <summary>
        /// Purge unit test airings
        /// </summary>
        /// <param name="airingIds">airings to bury</param>
        /// <returns></returns>
        void PurgeUnitTestAirings(List<string> airingIds);

      
        /// <summary>
        /// Create's the change notification for status
        /// </summary>
        /// <param name="assetId">the airingId</param>
        /// <param name="changeNotifications">the changeNotifications</param>
        void CreateNotificationForStatusChange(string assetId, List<BLModel.ChangeNotification> changeNotifications);
    }
}
