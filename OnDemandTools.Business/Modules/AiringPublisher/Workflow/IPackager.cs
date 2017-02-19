using OnDemandTools.Business.Modules.AiringPublisher.Models;
using BLAiring = OnDemandTools.Business.Modules.Airing.Model;

namespace OnDemandTools.Business.Modules.AiringPublisher.Workflow
{
    public interface IPackager
    {
        QueueAiring Package(BLAiring.Airing airing, Action action);
    }
}