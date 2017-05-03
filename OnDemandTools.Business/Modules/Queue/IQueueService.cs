using OnDemandTools.Business.Modules.Queue.Model;
using System.Collections.Generic;

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
        /// Retrieves those queues that are subscribed to receive status notification
        /// </summary>
        /// <returns></returns>
        List<Model.Queue> GetStatusNotificationSubscribers();

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
        /// depends on 'contentIds' & 'destinationCode'
        /// </summary>
        /// <param name="queueNames">The queue names.</param>
        /// <param name="contentIds">The content ids.</param>
        /// <param name="destinationCode">The destination code.</param>
        void FlagForRedelivery(IList<string> queueNames, IList<string> contentIds, string destinationCode);

        /// <summary>
        /// Flags the given list of queues for redelivery. Assets selected for delivery
        /// depends on 'airingId' & 'destinationCode 
        /// </summary>
        /// <param name="queueNames">The queue names</param>
        /// <param name="airingId">The airing id</param>
        /// <param name="destinationCode">The destination code</param>
        void FlagForRedelivery(IList<string> queueNames, string airingId, string destinationCode);

        /// <summary>
        /// Flags the given list of queues for redelivery. Assets selected for delivery
        /// depends on 'titleIds'
        /// </summary>
        /// <param name="queueNames">The queue names.</param>
        /// <param name="titleIds">The title ids.</param>
        /// <param name="changeNotificationType">the changenotification type file/title</param>
        void FlagForRedelivery(IList<string> queueNames, IList<int> titleIds, ChangeNotificationType changeNotificationType);

        /// <summary>
        /// Flags the given list of queues for redelivery. Assets selected for delivery
        /// depends on 'airingIds'
        /// </summary>
        /// <param name="queueNames">The queue names.</param>
        /// <param name="airingIds">The airing ids.</param>
        /// <param name="changeNotificationType">the changenotification type file/title</param>
        void FlagForRedelivery(IList<string> queueNames, IList<string> airingIds, ChangeNotificationType changeNotificationType);

        /// <summary>
        /// Flags the given list of queue for redelivery. Assets selected for delivery
        /// depends on 'queueName'
        /// </summary>
        /// <param name="queueName">The queue name.</param>
        void FlagForRedelivery(string queueName);

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
        /// Unlocks the queue
        /// </summary>
        /// <param name="name">queue name</param>
        void Unlock(string name);

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

        /// <summary>
        /// Deletes messages by media id
        /// </summary>
        /// <param name="mediaId"></param>
        void DeleteHistoricalMessage(string mediaId);


        /// <summary>
        ///  returns any message delived for given Queue and AiringId
        /// </summary>
        /// <param name="airingId">airing id to check</param>
        /// <param name="queueName">queue name to check</param>
        HistoricalMessage GetMessageDeliveredForAiringId(string airingId, string queueName);


        /// <summary>
        ///  returns all message's delived for given Queue and AiringId
        /// </summary>
        /// <param name="airingId">airing id to check</param>
        /// <param name="queueName">queue name to check</param>
        List<HistoricalMessage> GetAllMessagesDeliveredForAiringId(string airingId, string queueName);

        /// <summary>
        /// Get all the queues
        /// </summary>
        /// <returns>list of queues</returns>
        List<Model.Queue> GetQueues();
    }
}
