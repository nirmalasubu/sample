using AutoMapper;
using System;
using BLModel = OnDemandTools.Business.Modules.Pathing.Model;
using DLModel = OnDemandTools.DAL.Modules.Pathing.Model;

namespace OnDemandTools.API.Utilities.EntityMapping.Rules
{
    /// <summary>
    /// Mapping rules for Pathtranslation and related entities
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class PathTranslationProfile : Profile
    {

        public PathTranslationProfile()
        {            
            CreateMap<DLModel.PathTranslation, BLModel.PathTranslation>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
            CreateMap<BLModel.PathTranslation, DLModel.PathTranslation>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => (src.Id != null) ? (new MongoDB.Bson.ObjectId(src.Id)) : (MongoDB.Bson.ObjectId.Empty)));

            CreateMap<DLModel.PathInfo, BLModel.PathInfo>();           
            CreateMap<BLModel.PathInfo, DLModel.PathInfo>()
                .ForMember(dest => dest.Brand, opt => opt.Ignore())
                .AfterMap((src, dest) => {
                    if (String.IsNullOrWhiteSpace(src.Brand))
                        dest.Brand = null;
                    else
                        dest.Brand = src.Brand;
                })
                .ForMember(dest => dest.ProtectionType, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    if (String.IsNullOrWhiteSpace(src.ProtectionType))
                        dest.ProtectionType = null;
                    else
                        dest.ProtectionType = src.ProtectionType;
                });
        }
    }
}
