using OnDemandTools.Business.Modules.Airing.Model;
using OnDemandTools.Jobs.Models;

namespace OnDemandTools.Jobs.JobRegistry.Publisher
{
    public interface IPackager
    {
        QueueAiring Package(Airing airing, Action action);
    }
}