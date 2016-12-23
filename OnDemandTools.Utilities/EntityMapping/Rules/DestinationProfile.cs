using System;
using AutoMapper;
using MongoDB.Bson;
using BLModel = OnDemandTools.Business.Modules.Destination.Model;
using DLModel = OnDemandTools.DAL.Modules.Destination.Model;

namespace OnDemandTools.Utilities.EntityMapping.Rules
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
        }
    }
}
