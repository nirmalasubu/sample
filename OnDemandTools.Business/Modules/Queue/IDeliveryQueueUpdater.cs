
using System.Collections.Generic;


namespace OnDemandTools.Business.Modules.Queue
{
    public interface IDeliveryQueueUpdater
    {
        List<Model.Queue> PopulateMessageCounts(List<Model.Queue> deliveryQueues);
    }
}
