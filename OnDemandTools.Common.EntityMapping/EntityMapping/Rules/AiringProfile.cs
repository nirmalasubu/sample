using AutoMapper;
using MongoDB.Bson;
using BLModel = OnDemandTools.Business.Modules.Airing.Model;
using DLModel = OnDemandTools.DAL.Modules.Airings.Model;
using BLAiringLongModel = OnDemandTools.Business.Modules.Airing.Model.Alternate.Long;

namespace OnDemandTools.Common.EntityMapping
{
    public class AiringProfile : Profile
    {
        public AiringProfile()
        {
            CreateMap<BLModel.Airing, DLModel.Airing>()
             .ForMember(d => d.Id, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Id) ? new ObjectId() : new ObjectId(s.Id)));
            CreateMap<BLModel.AiringLink, DLModel.AiringLink>();
            CreateMap<BLModel.ClosedCaptioning, DLModel.ClosedCaptioning>();
            CreateMap<BLModel.Deliverable, DLModel.Deliverable>();
            CreateMap<BLModel.Destination, DLModel.Destination>();
            CreateMap<BLModel.Duration, DLModel.Duration>();
            CreateMap<BLModel.Element, DLModel.Element>();
            CreateMap<BLModel.Episode, DLModel.Episode>();
            CreateMap<BLModel.Feed, DLModel.Feed>();
            CreateMap<BLModel.Flags, DLModel.Flags>();
            CreateMap<BLModel.Flight, DLModel.Flight>();
            CreateMap<BLModel.Genre, DLModel.Genre>();
            CreateMap<BLModel.GuideCategory, DLModel.GuideCategory>();
            CreateMap<BLModel.Package, DLModel.Package>();
            CreateMap<BLModel.Participant, DLModel.Participant>();
            CreateMap<BLModel.PlayItem, DLModel.PlayItem>();
            CreateMap<BLModel.Product, DLModel.Product>();
            CreateMap<BLModel.ProductCode, DLModel.ProductCode>();
            CreateMap<BLModel.ProgramType, DLModel.ProgramType>();
            CreateMap<BLModel.Property, DLModel.Property>();
            CreateMap<BLModel.ProviderContentTier, DLModel.ProviderContentTier>();
            CreateMap<BLModel.Season, DLModel.Season>();
            CreateMap<BLModel.Series, DLModel.Series>();
            CreateMap<BLModel.Status, DLModel.Status>();
            CreateMap<BLModel.Story, DLModel.Story>();
            CreateMap<BLModel.Task, DLModel.Task>();
            CreateMap<BLModel.Title, DLModel.Title>();
            CreateMap<BLModel.TitleId, DLModel.TitleId>();
            CreateMap<BLModel.Turniverse, DLModel.Turniverse>();
            CreateMap<BLModel.TVRating, DLModel.TVRating>();
            CreateMap<BLModel.Version, DLModel.Version>();
            CreateMap<BLModel.ChangeNotification, DLModel.ChangeNotification>();



            CreateMap<DLModel.Airing, BLModel.Airing>()
           .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id.ToString()));
            CreateMap<DLModel.AiringLink, BLModel.AiringLink>();
            CreateMap<DLModel.ClosedCaptioning, BLModel.ClosedCaptioning>();
            CreateMap<DLModel.Deliverable, BLModel.Deliverable>();
            CreateMap<DLModel.Destination, BLModel.Destination>();
            CreateMap<DLModel.Duration, BLModel.Duration>();
            CreateMap<DLModel.Element, BLModel.Element>();
            CreateMap<DLModel.Episode, BLModel.Episode>();
            CreateMap<DLModel.Feed, BLModel.Feed>();
            CreateMap<DLModel.Flags, BLModel.Flags>();
            CreateMap<DLModel.Flight, BLModel.Flight>();
            CreateMap<DLModel.Genre, BLModel.Genre>();
            CreateMap<DLModel.GuideCategory, BLModel.GuideCategory>();
            CreateMap<DLModel.Package, BLModel.Package>();
            CreateMap<DLModel.Participant, BLModel.Participant>();
            CreateMap<DLModel.PlayItem, BLModel.PlayItem>();
            CreateMap<DLModel.Product, BLModel.Product>();
            CreateMap<DLModel.ProductCode, BLModel.ProductCode>();
            CreateMap<DLModel.ProgramType, BLModel.ProgramType>();
            CreateMap<DLModel.Property, BLModel.Property>();
            CreateMap<DLModel.Deliverable, BLModel.Deliverable>();
            CreateMap<DLModel.ProviderContentTier, BLModel.ProviderContentTier>();
            CreateMap<DLModel.Season, BLModel.Season>();
            CreateMap<DLModel.Series, BLModel.Series>();
            CreateMap<DLModel.Status, BLModel.Status>();
            CreateMap<DLModel.Story, BLModel.Story>();
            CreateMap<DLModel.Task, BLModel.Task>();
            CreateMap<DLModel.Title, BLModel.Title>();
            CreateMap<DLModel.TitleId, BLModel.TitleId>();
            CreateMap<DLModel.Turniverse, BLModel.Turniverse>();
            CreateMap<DLModel.TVRating, BLModel.TVRating>();
            CreateMap<DLModel.Version, BLModel.Version>();
            CreateMap<DLModel.ChangeNotification, BLModel.ChangeNotification>();

            // DAL to long BL
            CreateMap<DLModel.Airing, BLAiringLongModel.Airing>()
                .ForMember(d => d.Brand, opt => opt.MapFrom(s => s.Network))
                .ForMember(d => d.AiringId, opt => opt.MapFrom(s => s.AssetId))
                .ForMember(d => d.ReleasedBy, opt => opt.MapFrom(s => s.ReleaseBy))
                .ForMember(d => d.ReleasedOn, opt => opt.MapFrom(s => s.ReleaseOn));

            CreateMap<DLModel.AiringLink, BLAiringLongModel.AiringLink>();
            CreateMap<DLModel.ClosedCaptioning, BLAiringLongModel.ClosedCaptioning>();
            CreateMap<DLModel.Destination, BLAiringLongModel.Destination>();
            CreateMap<DLModel.Duration, BLAiringLongModel.Duration>();
            CreateMap<DLModel.Episode, BLAiringLongModel.Episode>();

            CreateMap<DLModel.Flags, BLAiringLongModel.Flags>();
            CreateMap<DLModel.Flight, BLAiringLongModel.Flight>();
            CreateMap<DLModel.Genre, BLAiringLongModel.Genre>();
            CreateMap<DLModel.GuideCategory, BLAiringLongModel.GuideCategory>();
            CreateMap<DLModel.Package, BLAiringLongModel.Package>();
            CreateMap<DLModel.Participant, BLAiringLongModel.Participant>();
            CreateMap<DLModel.PlayItem, BLAiringLongModel.PlayItem>();
            CreateMap<DLModel.ProductCode, BLAiringLongModel.ProductCode>();
            CreateMap<DLModel.ProgramType, BLAiringLongModel.ProgramType>();
            CreateMap<DLModel.ProviderContentTier, BLAiringLongModel.ProviderContentTier>();
            CreateMap<DLModel.Season, BLAiringLongModel.Season>();
            CreateMap<DLModel.Series, BLAiringLongModel.Series>();
            CreateMap<DLModel.Story, BLAiringLongModel.Story>();
            CreateMap<DLModel.TVRating, BLAiringLongModel.Rating>();
            CreateMap<DLModel.Title, BLAiringLongModel.Title>()
                .ForMember(d => d.Rating, opt => opt.MapFrom(s => s.TVRating))
                .ForMember(d => d.Episode, opt => opt.MapFrom(s => s.Episode ?? Mapper.Map<DLModel.Element, DLModel.Episode>(s.Element)));
            CreateMap<DLModel.TitleId, BLAiringLongModel.TitleId>();
            CreateMap<DLModel.Version, BLAiringLongModel.Version>();
            CreateMap<DLModel.Destination, BLAiringLongModel.Destination>();
            CreateMap<DLModel.Property, BLAiringLongModel.Property>();
            CreateMap<DLModel.Deliverable, BLAiringLongModel.Deliverable>();


        }
    }
}
