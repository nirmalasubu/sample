using AutoMapper;
using System;
using AiringRequestModel = OnDemandTools.API.v1.Models.Airing.Update;
using BLAiringModel = OnDemandTools.Business.Modules.Airing.Model;

namespace OnDemandTools.API.Helpers.MappingRules.Airing
{
    public class AiringRequestProfile : Profile
    {
        public AiringRequestProfile()
        {
            CreateMap<AiringRequestModel.AiringRequest, BLAiringModel.Airing>()
                .ForMember(d => d.AssetId, opt => opt.MapFrom(s => s.AiringId))
                .ForMember(d => d.ReleaseBy, opt => opt.MapFrom(s => s.ReleasedBy))
                .ForMember(d => d.Network, opt => opt.MapFrom(s => s.Brand))
                .ForMember(d => d.DisableTracking, opt => opt.MapFrom(s => s.Instructions.DisableTracking))
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.ReleaseOn, opt => opt.Ignore())
                .ForMember(d => d.MediaId, opt => opt.Ignore())
                .ForMember(d => d.Tasks, opt => opt.Ignore())
                .ForMember(d => d.DeliveredTo, opt => opt.Ignore())
                .ForMember(d => d.IgnoredQueues, opt => opt.Ignore());

            CreateMap<AiringRequestModel.AiringLink, BLAiringModel.AiringLink>();
            CreateMap<AiringRequestModel.Category, BLAiringModel.Category>();
            CreateMap<AiringRequestModel.ClosedCaptioning, BLAiringModel.ClosedCaptioning>();
            CreateMap<AiringRequestModel.Deliverable, BLAiringModel.Deliverable>();
            CreateMap<AiringRequestModel.Destination, BLAiringModel.Destination>();
            CreateMap<AiringRequestModel.Product, BLAiringModel.Product>();
            CreateMap<AiringRequestModel.Duration, BLAiringModel.Duration>();
            CreateMap<AiringRequestModel.Element, BLAiringModel.Element>();
            CreateMap<AiringRequestModel.Episode, BLAiringModel.Episode>();
            CreateMap<AiringRequestModel.Flags, BLAiringModel.Flags>();
            CreateMap<AiringRequestModel.Flight, BLAiringModel.Flight>()
                .ForMember(d => d.Start, opt => opt.MapFrom(s => DateTime.SpecifyKind(s.Start, DateTimeKind.Unspecified)))
                .ForMember(d => d.End, opt => opt.MapFrom(s => DateTime.SpecifyKind(s.End, DateTimeKind.Unspecified)));
            CreateMap<AiringRequestModel.Genre, BLAiringModel.Genre>();
            CreateMap<AiringRequestModel.GuideCategory, BLAiringModel.GuideCategory>();
            CreateMap<AiringRequestModel.Category, BLAiringModel.Category>();
            CreateMap<AiringRequestModel.Package, BLAiringModel.Package>();
            CreateMap<AiringRequestModel.Participant, BLAiringModel.Participant>();
            CreateMap<AiringRequestModel.PlayItem, BLAiringModel.PlayItem>();
            CreateMap<AiringRequestModel.ProductCode, BLAiringModel.ProductCode>();
            CreateMap<AiringRequestModel.ProgramType, BLAiringModel.ProgramType>();
            CreateMap<AiringRequestModel.Property, BLAiringModel.Property>();
            CreateMap<AiringRequestModel.ProviderContentTier, BLAiringModel.ProviderContentTier>();
            CreateMap<AiringRequestModel.Season, BLAiringModel.Season>();
            CreateMap<AiringRequestModel.Series, BLAiringModel.Series>();
            CreateMap<AiringRequestModel.Status, BLAiringModel.Status>();
            CreateMap<AiringRequestModel.Story, BLAiringModel.Story>();
            CreateMap<AiringRequestModel.Rating, BLAiringModel.TVRating>();
            CreateMap<AiringRequestModel.Title, BLAiringModel.Title>()
                .ForMember(d => d.TVRating, opt => opt.MapFrom(s => s.Rating))
                .ForMember(d => d.Genre, opt => opt.Ignore());

            CreateMap<AiringRequestModel.TitleId, BLAiringModel.TitleId>();
            CreateMap<AiringRequestModel.Version, BLAiringModel.Version>();
            CreateMap<AiringRequestModel.Turniverse, BLAiringModel.Turniverse>();
            CreateMap<AiringRequestModel.Feed, BLAiringModel.Feed>();
        }
    }
}
