using OnDemandTools.DAL.Modules.QueueMessages.Model;

namespace OnDemandTools.DAL.Modules.QueueMessages.Commands
{
    public interface IQueueMessageRecorder
    {
        void Record(HistoricalMessage record);
        void Remove(string mediaId);
    }
}
