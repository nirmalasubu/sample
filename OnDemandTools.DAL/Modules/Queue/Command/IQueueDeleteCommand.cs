using MongoDB.Bson;

namespace OnDemandTools.DAL.Modules.Queue.Command
{
    public interface IQueueDeleteCommand
    {
        void Delete(ObjectId id);
    }
}
