using AutoMapper;
using BLAiringModel = OnDemandTools.Business.Modules.Airing.Model;
using VMAiringLongModel = OnDemandTools.API.v1.Models.Airing.Long;
using VMAiringPostResponseModel = OnDemandTools.API.v1.Models.Airing.Long;
using BLAiringLongModel = OnDemandTools.Business.Modules.Airing.Model.Alternate.Long;
using OnDemandTools.API.v1.Models.Airing.Change;
using OnDemandTools.API.v1.Models.Airing.Title;
using OnDemandTools.Common.Extensions;

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
            CreateMap<BLAiringModel.ClosedCaptioning, VMAiringLongModel.ClosedCaptioning>();
            CreateMap<BLAiringModel.Destination, VMAiringLongModel.Destination>();
            CreateMap<BLAiringModel.Duration, VMAiringLongModel.Duration>();           
            CreateMap<BLAiringModel.Episode, VMAiringLongModel.Episode>();

            CreateMap<BLAiringModel.Flags, VMAiringLongModel.Flags>();
            CreateMap<BLAiringModel.Flight, VMAiringLongModel.Flight>();
            CreateMap<BLAiringModel.Genre, VMAiringLongModel.Genre>();
            CreateMap<BLAiringModel.GuideCategory, VMAiringLongModel.GuideCategory>();
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
            CreateMap<BLAiringModel.Property, VMAiringLongModel.Property>();
            CreateMap<BLAiringModel.Deliverable, VMAiringLongModel.Deliverable>();


            // BL to long BL
            CreateMap<BLAiringModel.Airing, BLAiringLongModel.Airing>()
                .ForMember(d => d.Brand, opt => opt.MapFrom(s => s.Network))
                .ForMember(d => d.AiringId, opt => opt.MapFrom(s => s.AssetId))
                .ForMember(d => d.ReleasedBy, opt => opt.MapFrom(s => s.ReleaseBy))
                .ForMember(d => d.ReleasedOn, opt => opt.MapFrom(s => s.ReleaseOn));

            CreateMap<BLAiringModel.AiringLink, BLAiringLongModel.AiringLink>();
            CreateMap<BLAiringModel.ClosedCaptioning, BLAiringLongModel.ClosedCaptioning>();
            CreateMap<BLAiringModel.Destination, BLAiringLongModel.Destination>();
            CreateMap<BLAiringModel.Duration, BLAiringLongModel.Duration>();
            CreateMap<BLAiringModel.Episode, BLAiringLongModel.Episode>();

            CreateMap<BLAiringModel.Flags, BLAiringLongModel.Flags>();
            CreateMap<BLAiringModel.Flight, BLAiringLongModel.Flight>();
            CreateMap<BLAiringModel.Genre, BLAiringLongModel.Genre>();
            CreateMap<BLAiringModel.GuideCategory, BLAiringLongModel.GuideCategory>();
            CreateMap<BLAiringModel.Package, BLAiringLongModel.Package>();
            CreateMap<BLAiringModel.Participant, BLAiringLongModel.Participant>();
            CreateMap<BLAiringModel.PlayItem, BLAiringLongModel.PlayItem>();
            CreateMap<BLAiringModel.ProductCode, BLAiringLongModel.ProductCode>();
            CreateMap<BLAiringModel.ProgramType, BLAiringLongModel.ProgramType>();
            CreateMap<BLAiringModel.ProviderContentTier, BLAiringLongModel.ProviderContentTier>();
            CreateMap<BLAiringModel.Season, BLAiringLongModel.Season>();
            CreateMap<BLAiringModel.Series, BLAiringLongModel.Series>();
            CreateMap<BLAiringModel.Story, BLAiringLongModel.Story>();
            CreateMap<BLAiringModel.TVRating, BLAiringLongModel.Rating>();
            CreateMap<BLAiringModel.Title, BLAiringLongModel.Title>()
                .ForMember(d => d.Rating, opt => opt.MapFrom(s => s.TVRating))
                .ForMember(d => d.Episode, opt => opt.MapFrom(s => s.Episode ?? Mapper.Map<BLAiringModel.Element, BLAiringModel.Episode>(s.Element)));
            CreateMap<BLAiringModel.TitleId, BLAiringLongModel.TitleId>();
            CreateMap<BLAiringModel.Version, BLAiringLongModel.Version>();
            CreateMap<BLAiringModel.Destination, BLAiringLongModel.Destination>();
            CreateMap<BLAiringModel.Property, BLAiringLongModel.Property>();
            CreateMap<BLAiringModel.Deliverable, BLAiringLongModel.Deliverable>();

            // VM to long BL
            CreateMap<VMAiringLongModel.Airing, BLAiringLongModel.Airing>();
            CreateMap<VMAiringLongModel.AiringLink, BLAiringLongModel.AiringLink>();
            CreateMap<VMAiringLongModel.ClosedCaptioning, BLAiringLongModel.ClosedCaptioning>();
            CreateMap<VMAiringLongModel.Destination, BLAiringLongModel.Destination>();
            CreateMap<VMAiringLongModel.Duration, BLAiringLongModel.Duration>();
            CreateMap<VMAiringLongModel.Episode, BLAiringLongModel.Episode>();
            CreateMap<VMAiringLongModel.Flags, BLAiringLongModel.Flags>();
            CreateMap<VMAiringLongModel.Flight, BLAiringLongModel.Flight>();
            CreateMap<VMAiringLongModel.Genre, BLAiringLongModel.Genre>();
            CreateMap<VMAiringLongModel.GuideCategory, BLAiringLongModel.GuideCategory>();
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
            CreateMap<BLAiringLongModel.ClosedCaptioning, VMAiringLongModel.ClosedCaptioning>();
            CreateMap<BLAiringLongModel.Destination, VMAiringLongModel.Destination>();
            CreateMap<BLAiringLongModel.Duration, VMAiringLongModel.Duration>();
            CreateMap<BLAiringLongModel.Episode, VMAiringLongModel.Episode>();
            CreateMap<BLAiringLongModel.Flags, VMAiringLongModel.Flags>();
            CreateMap<BLAiringLongModel.Flight, VMAiringLongModel.Flight>();
            CreateMap<BLAiringLongModel.Genre, VMAiringLongModel.Genre>();
            CreateMap<BLAiringLongModel.GuideCategory, VMAiringLongModel.GuideCategory>();
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
             CreateMap<BLAiringLongModel.Deliverable, VMAiringLongModel.Deliverable>();
            CreateMap<BLAiringLongModel.Property, VMAiringLongModel.Property>();

            CreateMap<BLAiringLongModel.Status, VMAiringLongModel.Status>();
            CreateMap<BLAiringLongModel.File, VMAiringLongModel.File>();
            CreateMap<BLAiringLongModel.Content, VMAiringLongModel.Content>();
            CreateMap<BLAiringLongModel.Media, VMAiringLongModel.Media>();
            CreateMap<BLAiringLongModel.Caption, VMAiringLongModel.Caption>();
            CreateMap<BLAiringLongModel.ContentSegment, VMAiringLongModel.ContentSegment>();
            CreateMap<BLAiringLongModel.PlayList, VMAiringLongModel.PlayList>();
            CreateMap<BLAiringLongModel.Url, VMAiringLongModel.Url>();

            CreateMap<BLAiringModel.Element, BLAiringModel.Episode>();

            // Mapping BL to Model when options=change
            CreateMap<BLAiringModel.Alternate.Change.FieldChange, Change>()
                .ForMember(d => d.Previous, opt => opt.MapFrom(s => s.Details.Previous))
                .ForMember(d => d.Current, opt => opt.MapFrom(s => s.Details.Current));

            CreateMap<BLAiringModel.Alternate.Change.ChangeValue, ChangeValue>();

            CreateMap<BLAiringModel.Alternate.Change.DeletionChange, Change>();
            CreateMap<BLAiringModel.Alternate.Change.NewReleaseChange, Change>();

            CreateMap<BLAiringModel.Alternate.Title.Title, Title>()
                          .ForMember(dest => dest.ExternalSources, opt => opt.Condition(src => (!src.ExternalSources.IsNullOrEmpty() && src.ExternalSources.Count > 0)));
            CreateMap<BLAiringModel.Alternate.Title.ExternalSource, ExternalSource>();


            // BL to VM  for Post Airing ResponseModel
            CreateMap<BLAiringModel.Airing, VMAiringPostResponseModel.PostResponseAiring>()
                .ForMember(d => d.Brand, opt => opt.MapFrom(s => s.Network))
                .ForMember(d => d.AiringId, opt => opt.MapFrom(s => s.AssetId))
                .ForMember(d => d.ReleasedBy, opt => opt.MapFrom(s => s.ReleaseBy))
                .ForMember(d => d.ReleasedOn, opt => opt.MapFrom(s => s.ReleaseOn));

            CreateMap<BLAiringModel.AiringLink, VMAiringPostResponseModel.AiringLink>();
            CreateMap<BLAiringModel.ClosedCaptioning, VMAiringPostResponseModel.ClosedCaptioning>();

            CreateMap<BLAiringModel.Duration, VMAiringPostResponseModel.Duration>();
            CreateMap<BLAiringModel.Episode, VMAiringPostResponseModel.Episode>();

            CreateMap<BLAiringModel.Flags, VMAiringPostResponseModel.Flags>();
            CreateMap<BLAiringModel.Flight, VMAiringPostResponseModel.PostResponseFlight>();
            CreateMap<BLAiringModel.Genre, VMAiringPostResponseModel.Genre>();
            CreateMap<BLAiringModel.GuideCategory, VMAiringPostResponseModel.GuideCategory>();
            CreateMap<BLAiringModel.Package, VMAiringPostResponseModel.Package>();
            CreateMap<BLAiringModel.Participant, VMAiringPostResponseModel.Participant>();
            CreateMap<BLAiringModel.PlayItem, VMAiringPostResponseModel.PlayItem>();
            CreateMap<BLAiringModel.ProductCode, VMAiringPostResponseModel.ProductCode>();
            CreateMap<BLAiringModel.ProgramType, VMAiringPostResponseModel.ProgramType>();
            CreateMap<BLAiringModel.ProviderContentTier, VMAiringPostResponseModel.ProviderContentTier>();
            CreateMap<BLAiringModel.Season, VMAiringPostResponseModel.Season>();
            CreateMap<BLAiringModel.Series, VMAiringPostResponseModel.Series>();
            CreateMap<BLAiringModel.Story, VMAiringPostResponseModel.Story>();
            CreateMap<BLAiringModel.TVRating, VMAiringPostResponseModel.Rating>();
            CreateMap<BLAiringModel.Title, VMAiringPostResponseModel.Title>()
                .ForMember(d => d.Rating, opt => opt.MapFrom(s => s.TVRating))
                .ForMember(d => d.Episode, opt => opt.MapFrom(s => s.Episode ?? Mapper.Map<BLAiringModel.Element, BLAiringModel.Episode>(s.Element)));

            CreateMap<BLAiringModel.TitleId, VMAiringPostResponseModel.TitleId>();
            CreateMap<BLAiringModel.Version, VMAiringPostResponseModel.Version>();
            CreateMap<BLAiringModel.Destination, VMAiringPostResponseModel.PostResponseDestination>()
                 .ForMember(d => d.Properties, opt => opt.Ignore())
                 .ForMember(d => d.Deliverables, opt => opt.Ignore());


            CreateMap<BLAiringModel.Property, VMAiringPostResponseModel.Property>();
            CreateMap<BLAiringModel.Deliverable, VMAiringPostResponseModel.Deliverable>();
        }
    }
}