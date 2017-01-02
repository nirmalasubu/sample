using AutoMapper;
using OnDemandTools.API.v1.Models.Destination;
using OnDemandTools.Business.Modules.Destination.Model;
using BLAiringLongModel = OnDemandTools.Business.Modules.Airing.Model.Alternate;
using VMAiringModel  = OnDemandTools.API.v1.Models.Airing;

namespace OnDemandTools.API.Helpers.MappingRules
{
    public class DestinationViewModelProfile: Profile
    {
        public DestinationViewModelProfile()
        {
            CreateMap<Destination, DestinationViewModel>();
            CreateMap<Property, PropertyViewModel>();
            CreateMap<Deliverable, DeliverableViewModel>();
            CreateMap<Content, ContentViewModel>();

            CreateMap<DestinationViewModel, Destination>();
            CreateMap<PropertyViewModel, Property>();
            CreateMap<DeliverableViewModel, Deliverable>();
            CreateMap<ContentViewModel, Content>();

            CreateMap<BLAiringLongModel.Destination.Destination, VMAiringModel.Destination.Destination>();
            CreateMap<BLAiringLongModel.Destination.Property, VMAiringModel.Destination.Property>();
            CreateMap<BLAiringLongModel.Destination.Deliverable, VMAiringModel.Destination.Deliverable>();
            CreateMap<BLAiringLongModel.Destination.Content, VMAiringModel.Destination.Content>();
        }
    }
}
