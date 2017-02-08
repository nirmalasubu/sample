using AutoMapper;
using OnDemandTools.DAL.Modules.Airings.Model;
using OnDemandTools.DAL.Modules.Airings.Model.Queues;

namespace OnDemandTools.Jobs.JobRegistry.Publisher
{
    public class QueuePackager : IPackager
    {
        public QueueAiring Package(Airing airing, Action action)
        {
            var queueAiring = Mapper.Map<Airing, QueueAiring>(airing);
            queueAiring.Action = action.ToString();
            return queueAiring;
        }
    }
}