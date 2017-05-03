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
            CreateMap<BLModel.Queue, DeliveryQueueHubModel>()
                .ForSourceMember(d => d.RoutingKey, opt => opt.Ignore())
                .ForSourceMember(d => d.Query, opt => opt.Ignore())
                .ForSourceMember(d => d.ContactEmailAddress, opt => opt.Ignore())
                .ForSourceMember(d => d.HoursOut, opt => opt.Ignore())
                .ForSourceMember(d => d.Active, opt => opt.Ignore())
                .ForSourceMember(d => d.Report, opt => opt.Ignore())
                .ForSourceMember(d => d.AllowAiringsWithNoVersion, opt => opt.Ignore())
                 .ForSourceMember(d => d.BimRequired, opt => opt.Ignore())
                 .ForSourceMember(d => d.DetectTitleChanges, opt => opt.Ignore())
                 .ForSourceMember(d => d.DetectImageChanges, opt => opt.Ignore())
                  .ForSourceMember(d => d.DetectVideoChanges, opt => opt.Ignore())
                 .ForSourceMember(d => d.DetectPackageChanges, opt => opt.Ignore())
                  .ForSourceMember(d => d.IsProhibitResendMediaId, opt => opt.Ignore())
                    .ForSourceMember(d => d.StatusNames, opt => opt.Ignore())
                     .ForSourceMember(d => d.CreatedBy, opt => opt.Ignore())
                     .ForSourceMember(d => d.CreatedDateTime, opt => opt.Ignore())
                     .ForSourceMember(d => d.ModifiedBy, opt => opt.Ignore())
                     .ForSourceMember(d => d.ModifiedDateTime, opt => opt.Ignore());

                
    }
    }
}
