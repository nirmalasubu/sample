﻿using AutoMapper;
using MongoDB.Bson;
using BLModel = OnDemandTools.Business.Modules.Product.Model;
using DLModel = OnDemandTools.DAL.Modules.Product.Model;

namespace OnDemandTools.Utilities.EntityMapping.Rules
{

    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<BLModel.Product, DLModel.Product>()
              .ForMember(d => d.Id, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Id) ? new ObjectId() : new ObjectId(s.Id)));
           
            CreateMap<DLModel.Product, BLModel.Product>()
             .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id.ToString()));           
        }
    }
}
