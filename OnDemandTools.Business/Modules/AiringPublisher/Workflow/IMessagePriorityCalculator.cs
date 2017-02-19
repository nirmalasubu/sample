using BLQueue=OnDemandTools.Business.Modules.Queue.Model;
using BLAiring=OnDemandTools.Business.Modules.Airing.Model;

namespace OnDemandTools.Business.Modules.AiringPublisher.Workflow
{
    public interface IMessagePriorityCalculator
    {
        byte? Calculate(BLQueue.Queue queue, BLAiring.Airing airing);
    }
}