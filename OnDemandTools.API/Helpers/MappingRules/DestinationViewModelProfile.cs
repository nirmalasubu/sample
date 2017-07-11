using AutoMapper;
using ADModel = OnDemandTools.API.v1.Models.Destination;
using OnDemandTools.Business.Modules.Destination.Model;
using BLAiringLongModel = OnDemandTools.Business.Modules.Airing.Model.Alternate;
using VMAiringModel  = OnDemandTools.API.v1.Models.Airing;

namespace OnDemandTools.API.Helpers.MappingRules
{
    public class DestinationViewModelProfile: Profile
    {
        public DestinationViewModelProfile()
        {
            CreateMap<Destination, ADModel.Destination>();
            CreateMap<Property, ADModel.Property>();
            CreateMap<Deliverable, ADModel.Deliverable>();
            CreateMap<Content, ADModel.Content>();

            CreateMap<ADModel.Destination, Destination>();
            CreateMap<ADModel.Property, Property>();
            CreateMap<ADModel.Deliverable, Deliverable>();
            CreateMap<ADModel.Content, Content>();

            CreateMap<BLAiringLongModel.Destination.Destination, VMAiringModel.Destination.Destination>();
            CreateMap<BLAiringLongModel.Destination.Property, VMAiringModel.Destination.Property>();
            CreateMap<BLAiringLongModel.Destination.Deliverable, VMAiringModel.Destination.Deliverable>();
            CreateMap<BLAiringLongModel.Destination.Content, VMAiringModel.Destination.Content>();
        }
    }
}
