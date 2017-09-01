using AutoMapper;
using MongoDB.Bson;
using OnDemandTools.DAL.Modules.Product.Model;
using BLAiringLongModel = OnDemandTools.Business.Modules.Airing.Model.Alternate;
using BLAiringModel = OnDemandTools.Business.Modules.Airing.Model;
using BLModel = OnDemandTools.Business.Modules.Destination.Model;
using DLAiringModel = OnDemandTools.DAL.Modules.Airings.Model;
using DLModel = OnDemandTools.DAL.Modules.Destination.Model;

namespace OnDemandTools.Common.EntityMapping
{
    public class DestinationProfile : Profile
    {
        public DestinationProfile()
        {
            CreateMap<BLModel.Destination, DLModel.Destination>()
              .ForMember(d => d.Id, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Id) ? new ObjectId() : new ObjectId(s.Id)));
            CreateMap<BLModel.Property, DLModel.Property>();
            CreateMap<BLModel.Deliverable, DLModel.Deliverable>();
            CreateMap<BLModel.Category, DLModel.Category>()
              .ForMember(d => d.Id, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Id) ? ObjectId.GenerateNewId() : new ObjectId(s.Id)));
            CreateMap<BLModel.Content, DLModel.Content>();

            CreateMap<DLModel.Destination, BLModel.Destination>()
             .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id.ToString()));
            CreateMap<DLModel.Property, BLModel.Property>();
            CreateMap<DLModel.Deliverable, BLModel.Deliverable>();
            CreateMap<DLModel.Category, BLModel.Category>();
            CreateMap<DLModel.Content, BLModel.Content>();


            CreateMap<DLModel.Destination, BLAiringLongModel.Destination.Destination>();
            CreateMap<DLModel.Property, BLAiringLongModel.Destination.Property>();
            CreateMap<DLModel.Deliverable, BLAiringLongModel.Destination.Deliverable>();
            CreateMap<DLModel.Category, BLAiringLongModel.Destination.Category>();
            CreateMap<DLModel.Content, BLAiringLongModel.Destination.Content>();

            CreateMap<DLModel.Destination, DLAiringModel.Destination>();
            CreateMap<DLModel.Property, DLAiringModel.Property>();
            CreateMap<DLModel.Deliverable, DLAiringModel.Deliverable>();
            CreateMap<DLModel.Property, DLAiringModel.Property>();
            CreateMap<DLModel.Deliverable, DLAiringModel.Deliverable>();
            CreateMap<DLAiringModel.Destination, BLAiringModel.Destination>();

            // Mapping betweeen business models in different domain
            CreateMap<BLModel.Destination, BLAiringModel.Destination>();
            CreateMap<BLModel.Property, BLAiringModel.Property>();

            CreateMap<BLModel.Category, BLModel.Property>().
                 ForMember(d => d.Value, opt => opt.MapFrom(s => s.Name)).
                ForMember(d => d.Name, opt => opt.UseValue<string>("Category"));

            CreateMap<BLModel.Category, BLAiringModel.Property>().
                ForMember(d => d.Value, opt => opt.MapFrom(s => s.Name)).
                ForMember(d => d.Name, opt => opt.UseValue<string>("Category"));

            CreateMap<DLModel.Category, DLModel.Property>().
               ForMember(d => d.Value, opt => opt.MapFrom(s => s.Name)).
               ForMember(d => d.Name, opt => opt.UseValue<string>("Category"));

            CreateMap<ContentTier, DLModel.Property>().
                 ForMember(d => d.Value, opt => opt.MapFrom(s => s.Name)).
               ForMember(d => d.Name, opt => opt.UseValue<string>("ContentTier"));

            CreateMap<BLModel.Deliverable, BLAiringModel.Deliverable>();

        }
    }
}
