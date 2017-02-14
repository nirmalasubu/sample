using MongoDB.Bson;
using MongoDB.Driver;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Airings.Model;

namespace OnDemandTools.DAL.Modules.Airings.Commands
{
    public class UpdateAiringQueueDelivery : BaseUpdateAiringQueueDelivery, IUpdateAiringQueueDelivery
    {
        public UpdateAiringQueueDelivery(IODTDatastore connection)
            : base(connection, DataStoreConfiguration.CurrentAssetsCollection)
        {

        }
    }
}
