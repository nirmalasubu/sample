using System;
using AutoMapper;
using MongoDB.Bson;
using BLModel = OnDemandTools.Business.Modules.Destination.Model;
using DLModel = OnDemandTools.DAL.Modules.Destination.Model;
using DLAiringModel = OnDemandTools.DAL.Modules.Airings.Model;
using BLAiringModel = OnDemandTools.Business.Modules.Airing.Model;
using BLAiringLongModel = OnDemandTools.Business.Modules.Airing.Model.Alternate;

namespace OnDemandTools.API.Utilities.EntityMapping.Rules
{
    public class DestinationProfile: Profile
    {
        public DestinationProfile()
        {
            CreateMap<BLModel.Destination, DLModel.Destination>()
              .ForMember(d => d.Id, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Id) ? new ObjectId() : new ObjectId(s.Id)));
            CreateMap<BLModel.Property, DLModel.Property>();
            CreateMap<BLModel.Deliverable, DLModel.Deliverable>();
            CreateMap<BLModel.Content, DLModel.Content>();

            CreateMap<DLModel.Destination, BLModel.Destination>()
             .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id.ToString()));
            CreateMap<DLModel.Property, BLModel.Property>();
            CreateMap<DLModel.Deliverable, BLModel.Deliverable>();
            CreateMap<DLModel.Content, BLModel.Content>();


            CreateMap<DLModel.Destination, BLAiringLongModel.Destination.Destination>();
            CreateMap<DLModel.Property, BLAiringLongModel.Destination.Property>();
            CreateMap<DLModel.Deliverable, BLAiringLongModel.Destination.Deliverable>();
            CreateMap<DLModel.Content, BLAiringLongModel.Destination.Content>();

            CreateMap<DLModel.Destination, DLAiringModel.Destination>();
            CreateMap<DLAiringModel.Destination, BLAiringModel.Destination>();
        }
    }
}
