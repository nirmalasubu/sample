using AutoMapper;
using BLAiringModel = OnDemandTools.Business.Modules.Airing.Model;
using VMAiringLongModel = OnDemandTools.API.v1.Models.Airing.Long;

namespace OnDemandTools.API.Helpers.MappingRules.Airing
{
    public class AiringLongProfile: Profile
    {
        public AiringLongProfile()
        {
            CreateMap<BLAiringModel.Airing, VMAiringLongModel.Airing>()
                .ForMember(d => d.Brand, opt => opt.MapFrom(s => s.Network))
                .ForMember(d => d.AiringId, opt => opt.MapFrom(s => s.AssetId))
                .ForMember(d => d.ReleasedBy, opt => opt.MapFrom(s => s.ReleaseBy))
                .ForMember(d => d.ReleasedOn, opt => opt.MapFrom(s => s.ReleaseOn));

            CreateMap<BLAiringModel.AiringLink, VMAiringLongModel.AiringLink>();
            CreateMap<BLAiringModel.Category, VMAiringLongModel.Category>();
            CreateMap<BLAiringModel.ClosedCaptioning, VMAiringLongModel.ClosedCaptioning>();
            CreateMap<BLAiringModel.Destination, VMAiringLongModel.Destination>();
            CreateMap<BLAiringModel.Duration, VMAiringLongModel.Duration>();           
            CreateMap<BLAiringModel.Episode, VMAiringLongModel.Episode>();

            CreateMap<BLAiringModel.Flags, VMAiringLongModel.Flags>();
            CreateMap<BLAiringModel.Flight, VMAiringLongModel.Flight>();
            CreateMap<BLAiringModel.Genre, VMAiringLongModel.Genre>();
            CreateMap<BLAiringModel.GuideCategory, VMAiringLongModel.GuideCategory>();
            CreateMap<BLAiringModel.Category, VMAiringLongModel.Category>();
            CreateMap<BLAiringModel.Package, VMAiringLongModel.Package>();
            CreateMap<BLAiringModel.Participant, VMAiringLongModel.Participant>();
            CreateMap<BLAiringModel.PlayItem, VMAiringLongModel.PlayItem>();
            CreateMap<BLAiringModel.ProductCode, VMAiringLongModel.ProductCode>();
            CreateMap<BLAiringModel.ProgramType, VMAiringLongModel.ProgramType>();
            CreateMap<BLAiringModel.ProviderContentTier, VMAiringLongModel.ProviderContentTier>();
            CreateMap<BLAiringModel.Season, VMAiringLongModel.Season>();
            CreateMap<BLAiringModel.Series, VMAiringLongModel.Series>();
            CreateMap<BLAiringModel.Story, VMAiringLongModel.Story>();
            CreateMap<BLAiringModel.TVRating, VMAiringLongModel.Rating>();            
            CreateMap<BLAiringModel.Title, VMAiringLongModel.Title>()
                .ForMember(d => d.Rating, opt => opt.MapFrom(s => s.TVRating))
                .ForMember(d => d.Episode, opt => opt.MapFrom(s => s.Episode ?? Mapper.Map<BLAiringModel.Element, BLAiringModel.Episode>(s.Element)));

            CreateMap<BLAiringModel.TitleId, VMAiringLongModel.TitleId>();
            CreateMap<BLAiringModel.Version, VMAiringLongModel.Version>();
            CreateMap<BLAiringModel.Destination, VMAiringLongModel.Destination>();


            CreateMap<BLAiringModel.Element, BLAiringModel.Episode>();
        }
    }
}