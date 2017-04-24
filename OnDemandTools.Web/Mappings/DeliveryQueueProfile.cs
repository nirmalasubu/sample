using AutoMapper;
using OnDemandTools.Web.Models.DeliveryQueue;
using BLModel = OnDemandTools.Business.Modules.Queue.Model;


namespace OnDemandTools.Web.Mappings
{
    public class DeliveryQueueProfile : Profile
    {
        public DeliveryQueueProfile()
        {
            CreateMap<DeliveryQueueModel, BLModel.Queue>();
            CreateMap<BLModel.Queue,DeliveryQueueModel >();
        }
    }
}
