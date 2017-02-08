using OnDemandTools.DAL.Modules.Airings.Model;
using OnDemandTools.DAL.Modules.Queue.Model;

namespace OnDemandTools.Jobs.JobRegistry.Publisher
{
    public interface IMessagePriorityCalculator
    {
        byte? Calculate(Queue queue, Airing airing);
    }
}