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
    }
}
