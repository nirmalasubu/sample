using AutoMapper;
using BLAiringModel = OnDemandTools.Business.Modules.Airing.Model;
using VMAiringLongModel = OnDemandTools.API.v1.Models.Airing.Long;
using BLAiringLongModel = OnDemandTools.Business.Modules.Airing.Model.Alternate.Long;

namespace OnDemandTools.API.Helpers.MappingRules.Airing
{
    public class AiringLongProfile: Profile
    {
        public AiringLongProfile()
        {
            // BL to VM
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



            // VM to long BL
            CreateMap<VMAiringLongModel.Airing, BLAiringLongModel.Airing>();
            CreateMap<VMAiringLongModel.AiringLink, BLAiringLongModel.AiringLink>();
            CreateMap<VMAiringLongModel.Category, BLAiringLongModel.Category>();
            CreateMap<VMAiringLongModel.ClosedCaptioning, BLAiringLongModel.ClosedCaptioning>();
            CreateMap<VMAiringLongModel.Destination, BLAiringLongModel.Destination>();
            CreateMap<VMAiringLongModel.Duration, BLAiringLongModel.Duration>();
            CreateMap<VMAiringLongModel.Episode, BLAiringLongModel.Episode>();
            CreateMap<VMAiringLongModel.Flags, BLAiringLongModel.Flags>();
            CreateMap<VMAiringLongModel.Flight, BLAiringLongModel.Flight>();
            CreateMap<VMAiringLongModel.Genre, BLAiringLongModel.Genre>();
            CreateMap<VMAiringLongModel.GuideCategory, BLAiringLongModel.GuideCategory>();
            CreateMap<VMAiringLongModel.Category, BLAiringLongModel.Category>();
            CreateMap<VMAiringLongModel.Package, BLAiringLongModel.Package>();
            CreateMap<VMAiringLongModel.Participant, BLAiringLongModel.Participant>();
            CreateMap<VMAiringLongModel.PlayItem, BLAiringLongModel.PlayItem>();
            CreateMap<VMAiringLongModel.ProductCode, BLAiringLongModel.ProductCode>();
            CreateMap<VMAiringLongModel.Options, BLAiringLongModel.Options > ();
            CreateMap<VMAiringLongModel.ProgramType, BLAiringLongModel.ProgramType>();
            CreateMap<VMAiringLongModel.ProviderContentTier, BLAiringLongModel.ProviderContentTier>();
            CreateMap<VMAiringLongModel.Season, BLAiringLongModel.Season>();
            CreateMap<VMAiringLongModel.Series, BLAiringLongModel.Series>();
            CreateMap<VMAiringLongModel.Story, BLAiringLongModel.Story>();
            CreateMap<VMAiringLongModel.Rating, BLAiringLongModel.Rating>();
            CreateMap<VMAiringLongModel.Title, BLAiringLongModel.Title>();
            CreateMap<VMAiringLongModel.TitleId, BLAiringLongModel.TitleId>();
            CreateMap<VMAiringLongModel.Version, BLAiringLongModel.Version>();
            CreateMap<VMAiringLongModel.Destination, BLAiringLongModel.Destination>();
            CreateMap<VMAiringLongModel.Status, BLAiringLongModel.Status>();


            // long BL to VM
            CreateMap<BLAiringLongModel.Airing, VMAiringLongModel.Airing>();
            CreateMap<BLAiringLongModel.AiringLink, VMAiringLongModel.AiringLink>();
            CreateMap<BLAiringLongModel.Category, VMAiringLongModel.Category>();
            CreateMap<BLAiringLongModel.ClosedCaptioning, VMAiringLongModel.ClosedCaptioning>();
            CreateMap<BLAiringLongModel.Destination, VMAiringLongModel.Destination>();
            CreateMap<BLAiringLongModel.Duration, VMAiringLongModel.Duration>();
            CreateMap<BLAiringLongModel.Episode, VMAiringLongModel.Episode>();
            CreateMap<BLAiringLongModel.Flags, VMAiringLongModel.Flags>();
            CreateMap<BLAiringLongModel.Flight, VMAiringLongModel.Flight>();
            CreateMap<BLAiringLongModel.Genre, VMAiringLongModel.Genre>();
            CreateMap<BLAiringLongModel.GuideCategory, VMAiringLongModel.GuideCategory>();
            CreateMap<BLAiringLongModel.Category, VMAiringLongModel.Category>();
            CreateMap<BLAiringLongModel.Package, VMAiringLongModel.Package>();
            CreateMap<BLAiringLongModel.Participant, VMAiringLongModel.Participant>();
            CreateMap<BLAiringLongModel.PlayItem, VMAiringLongModel.PlayItem>();
            CreateMap<BLAiringLongModel.ProductCode, VMAiringLongModel.ProductCode>();
            CreateMap<BLAiringLongModel.Options, VMAiringLongModel.Options>();
            CreateMap<BLAiringLongModel.ProgramType, VMAiringLongModel.ProgramType>();
            CreateMap<BLAiringLongModel.ProviderContentTier, VMAiringLongModel.ProviderContentTier>();
            CreateMap<BLAiringLongModel.Season, VMAiringLongModel.Season>();
            CreateMap<BLAiringLongModel.Series, VMAiringLongModel.Series>();
            CreateMap<BLAiringLongModel.Story, VMAiringLongModel.Story>();
            CreateMap<BLAiringLongModel.Rating, VMAiringLongModel.Rating>();
            CreateMap<BLAiringLongModel.Title, VMAiringLongModel.Title>();
            CreateMap<BLAiringLongModel.TitleId, VMAiringLongModel.TitleId>();
            CreateMap<BLAiringLongModel.Version, VMAiringLongModel.Version>();
            CreateMap<BLAiringLongModel.Destination, VMAiringLongModel.Destination>();
            CreateMap<BLAiringLongModel.Status, VMAiringLongModel.Status>();
            CreateMap<BLAiringLongModel.File, VMAiringLongModel.File>();
            CreateMap<BLAiringLongModel.Content, VMAiringLongModel.Content>();
            CreateMap<BLAiringLongModel.Media, VMAiringLongModel.Media>();
            CreateMap<BLAiringLongModel.Caption, VMAiringLongModel.Caption>();
            CreateMap<BLAiringLongModel.ContentSegment, VMAiringLongModel.ContentSegment>();
            CreateMap<BLAiringLongModel.PlayList, VMAiringLongModel.PlayList>();
            CreateMap<BLAiringLongModel.Url, VMAiringLongModel.Url>();

            CreateMap<BLAiringModel.Element, BLAiringModel.Episode>();
        }
    }
}