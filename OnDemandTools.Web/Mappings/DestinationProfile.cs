using AutoMapper;
using OnDemandTools.Web.Models.Destination;
using BLModel = OnDemandTools.Business.Modules.Destination.Model;


namespace OnDemandTools.Web.Mappings
{
    public class DestinationProfile : Profile
    {
        public DestinationProfile()
        {
            CreateMap<DestinationViewModel, BLModel.Destination>();
            CreateMap<Property, BLModel.Property>();
            CreateMap<Deliverable, BLModel.Deliverable>();
            CreateMap<Content, BLModel.Content>();

            CreateMap<BLModel.Destination, DestinationViewModel>();
            CreateMap<BLModel.Property, Property>();
            CreateMap<BLModel.Deliverable, Deliverable>();
            CreateMap<BLModel.Content, Content>();
        }
    }
}
