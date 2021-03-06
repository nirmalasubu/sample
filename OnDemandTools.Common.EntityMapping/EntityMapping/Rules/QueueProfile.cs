﻿using AutoMapper;
using MongoDB.Bson;
using BLModel = OnDemandTools.Business.Modules.Queue.Model;
using DLModel = OnDemandTools.DAL.Modules.Queue.Model;
using DMModel = OnDemandTools.DAL.Modules.QueueMessages.Model;


namespace OnDemandTools.Common.EntityMapping
{
    public class QueueProfile:Profile
    {
        public QueueProfile()
        {
            CreateMap<BLModel.Queue, DLModel.Queue>()
              .ForMember(d => d.Id, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Id) ? new ObjectId() : new ObjectId(s.Id)))
              .ForMember(d => d.Query, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Query) ? "{}" : s.Query));

            CreateMap<DLModel.Queue, BLModel.Queue>()
              .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id.ToString()));

            CreateMap<BLModel.DeliverCriteria, DLModel.DeliverCriteria>();
            CreateMap<BLModel.DeliverCriteria, DMModel.DeliverCriteria>();
        }
    }
}
