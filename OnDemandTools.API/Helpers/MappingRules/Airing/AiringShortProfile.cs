using AutoMapper;
using BLAiringModel = OnDemandTools.Business.Modules.Airing.Model;
using VMAiringShortModel = OnDemandTools.API.v1.Models.Airing.Short;

namespace OnDemandTools.API.Helpers.MappingRules.Airing
{
    public class AiringShortProfile:Profile
    {
        public AiringShortProfile()
        {
            CreateMap<BLAiringModel.Airing, VMAiringShortModel.Airing>()
               .ForMember(d => d.Brand, opt => opt.MapFrom(s => s.Network))
               .ForMember(d => d.AiringId, opt => opt.MapFrom(s => s.AssetId))
               .ForMember(d => d.ReleasedBy, opt => opt.MapFrom(s => s.ReleaseBy))
               .ForMember(d => d.ReleasedOn, opt => opt.MapFrom(s => s.ReleaseOn));

            CreateMap<BLAiringModel.TitleId, VMAiringShortModel.TitleId>();
            CreateMap<BLAiringModel.Destination, VMAiringShortModel.Destination>();
            CreateMap<BLAiringModel.Property, VMAiringShortModel.Property>();
            CreateMap<BLAiringModel.Deliverable, VMAiringShortModel.Deliverable>();
            CreateMap<BLAiringModel.Duration, VMAiringShortModel.Duration>();           
            CreateMap<BLAiringModel.Episode, VMAiringShortModel.Episode>();
            CreateMap<BLAiringModel.Flags, VMAiringShortModel.Flags>();
            CreateMap<BLAiringModel.Flight, VMAiringShortModel.Flight>();
            CreateMap<BLAiringModel.Season, VMAiringShortModel.Season>();
            CreateMap<BLAiringModel.Series, VMAiringShortModel.Series>();
            CreateMap<BLAiringModel.Story, VMAiringShortModel.Story>();
            CreateMap<BLAiringModel.TVRating, VMAiringShortModel.Rating>();
            CreateMap<BLAiringModel.Title, VMAiringShortModel.Title>()
                .ForMember(d => d.Rating, opt => opt.MapFrom(s => s.TVRating))
                .ForMember(d => d.Episode, opt => opt.MapFrom(s => s.Episode ?? Mapper.Map<BLAiringModel.Element, BLAiringModel.Episode>(s.Element)));
                  
        }
    }
}
