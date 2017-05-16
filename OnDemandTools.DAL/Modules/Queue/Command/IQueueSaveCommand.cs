using QueueModel= OnDemandTools.DAL.Modules.Queue.Model;

namespace OnDemandTools.DAL.Modules.Queue.Command
{
    public interface IQueueSaveCommand
    {
        QueueModel.Queue Save(QueueModel.Queue queue);
    }
}
