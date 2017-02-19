using OnDemandTools.Business.Modules.AiringPublisher.Models;
using BLAiring = OnDemandTools.Business.Modules.Airing.Model;

namespace OnDemandTools.Business.Modules.AiringPublisher.Workflow
{
    public class QueuePackager : IPackager
    {
        public QueueAiring Package(BLAiring.Airing airing, Action action)
        {
            var queueAiring = new QueueAiring();
            queueAiring.AiringId = airing.AssetId;
            queueAiring.Action = action.ToString();
            return queueAiring;
        }
    }
}