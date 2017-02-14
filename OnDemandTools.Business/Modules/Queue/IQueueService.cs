﻿using System.Collections.Generic;

namespace OnDemandTools.Business.Modules.Queue
{
    public interface IQueueService
    {


        /// <summary>
        /// Retrieves those queues that match the provided
        /// active flag (true/false)
        /// </summary>
        /// <param name="active">if set to <c>true</c> [active].</param>
        /// <returns></returns>
        List<Model.Queue> GetByStatus(bool active);

        /// <summary>
        /// Retrieves those queues that are subscribed to receive package notification
        /// </summary>
        /// <returns></returns>
        List<Model.Queue> GetPackageNotificationSubscribers();

        /// <summary>
        /// Flags the given list of queues for redelivery. Assets selected for delivery
        /// depends on 'titleIds' & 'destinationCode'
        /// </summary>
        /// <param name="queueNames">The queue names.</param>
        /// <param name="titleIds">The title ids.</param>
        /// <param name="destinationCode">The destination code.</param>
        void FlagForRedelivery(IList<string> queueNames, IList<int> titleIds, string destinationCode);

        /// <summary>
        /// Flags the given list of queues for redelivery. Assets selected for delivery
        /// depends on 'titleIds'
        /// </summary>
        /// <param name="queueNames">The queue names.</param>
        /// <param name="titleIds">The title ids.</param>
        void FlagForRedelivery(IList<string> queueNames, IList<int> titleIds);

        /// <summary>
        /// Flags the given list of queues for redelivery. Assets selected for delivery
        /// depends on 'airingIds'
        /// </summary>
        /// <param name="queueNames">The queue names.</param>
        /// <param name="airingIds">The airing ids.</param>
        void FlagForRedelivery(IList<string> queueNames, IList<string> airingIds);

        /// <summary>
        /// Gets queue by API key.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <returns></returns>
        Model.Queue GetByApiKey(string apiKey);

        /// <summary>
        /// Updates Queue procesing date with Current time 
        /// </summary>
        /// <param name="name"></param>
        void UpdateQueueProcessedTime(string name);


        /// <summary>
        /// Locks the queue
        /// </summary>
        /// <param name="queueName">queue name to lock</param>
        /// <param name="processorId">processor id to lock</param>
        /// <returns>Returns true if locked sucessfully</returns>
        bool Lock(string queueName, string processorId);

        /// <summary>
        /// Unlocks the queue
        /// </summary>
        /// <param name="queueName">queue name to lock</param>
        /// <param name="processorId">processor id to lock</param>
        void Unlock(string queueName, string processorId);

        /// <summary>
        /// Check and returns any message delived for given Queue and MediaId
        /// </summary>
        /// <param name="mediaId">media id to check</param>
        /// <param name="queueName">queue name to check</param>
        bool AnyMessageDeliveredForMediaId(string mediaId, string queueName);

        /// <summary>
        /// Check and returns any message delived for given Queue and AiringId
        /// </summary>
        /// <param name="airingId">airing id to check</param>
        /// <param name="queueName">queue name to check</param>
        bool AnyMessageDeliveredForAiringId(string airingId, string queueName);

        /// <summary>
        /// Adds the historical message for the queue delivery
        /// </summary>
        /// <param name="airingId">the airing id</param>
        /// <param name="mediaId">the media id</param>
        /// <param name="message">the message</param>
        /// <param name="remoteQueueName">the queue name</param>
        /// <param name="messagePriority">message priority</param>
        void AddHistoricalMessage(string airingId, string mediaId, string message, string remoteQueueName, byte? messagePriority);
    }
}
