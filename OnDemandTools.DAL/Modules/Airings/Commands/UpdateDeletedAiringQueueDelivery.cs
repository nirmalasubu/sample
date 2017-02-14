using OnDemandTools.DAL.Database;

namespace OnDemandTools.DAL.Modules.Airings.Commands
{
    public class UpdateDeletedAiringQueueDelivery : BaseUpdateAiringQueueDelivery, IUpdateDeletedAiringQueueDelivery
    {
        public UpdateDeletedAiringQueueDelivery(IODTDatastore connection)
            : base(connection, DataStoreConfiguration.DeletedAssetsCollection)
        {

        }

    }
}
