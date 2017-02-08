using OnDemandTools.Business.Modules.Queue.Model;
using OnDemandTools.DAL.Modules.Airings.Model;

namespace OnDemandTools.Jobs.JobRegistry.Publisher
{
    public interface IMessagePriorityCalculator
    {
        byte? Calculate(Queue queue, Airing airing);
    }
}