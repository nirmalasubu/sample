using AutoMapper;
using OnDemandTools.API.v1.Models.Destination;
using OnDemandTools.Business.Modules.Destination.Model;

namespace OnDemandTools.API.Helpers.MappingRules
{
    public class DestinationProfile: Profile
    {
        public DestinationProfile()
        {
            CreateMap<Destination, DestinationViewModel>();
            CreateMap<Property, PropertyViewModel>();
            CreateMap<Deliverable, DeliverableViewModel>();
            CreateMap<Content, ContentViewModel>();


            CreateMap<DestinationViewModel, Destination>();
            CreateMap<PropertyViewModel, Property>();
            CreateMap<DeliverableViewModel, Deliverable>();
            CreateMap<ContentViewModel, Content>();
        }
    }
}
