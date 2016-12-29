using AutoMapper;
using OnDemandTools.API.v1.Models.Queue;
using OnDemandTools.Business.Modules.Queue.Model;

namespace OnDemandTools.Utilities.EntityMapping.Rules
{
    public class QueueViewModelProfile:Profile
    {
        public QueueViewModelProfile()
        {
            CreateMap<Queue, QueueViewModel>();

            CreateMap<QueueViewModel, Queue>();            

        }
    }
}
