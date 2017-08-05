using AutoMapper;
using System;
using BLModel = OnDemandTools.Business.Modules.Pathing.Model;
using DLModel = OnDemandTools.DAL.Modules.Pathing.Model;

namespace OnDemandTools.Common.EntityMapping
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
                .ForMember(pth => pth.Id, opt => opt.MapFrom(src => src.Id.ToString()));
            CreateMap<DLModel.PathInfo, BLModel.PathInfo>();



            CreateMap<BLModel.PathTranslation, DLModel.PathTranslation>()
               .ForMember(pth => pth.Id, opt => opt.MapFrom(src => (!string.IsNullOrEmpty(src.Id)) ? (new MongoDB.Bson.ObjectId(src.Id)) : (MongoDB.Bson.ObjectId.Empty)));

            CreateMap<BLModel.PathInfo, DLModel.PathInfo>()
                .ForMember(pth => pth.Brand, opt => opt.Ignore())
                .AfterMap((src, pth) =>
                {
                    if (String.IsNullOrWhiteSpace(src.Brand))
                        pth.Brand = null;
                    else
                        pth.Brand = src.Brand;
                })
                .ForMember(pth => pth.ProtectionType, opt => opt.Ignore())
                .AfterMap((src, pth) =>
                {
                    if (String.IsNullOrWhiteSpace(src.ProtectionType))
                        pth.ProtectionType = null;
                    else
                        pth.ProtectionType = src.ProtectionType;
                })
                .ForMember(pth => pth.UrlType, opt => opt.Ignore())
                .AfterMap((src, pth) =>
                {
                    if (String.IsNullOrWhiteSpace(src.UrlType))
                        pth.UrlType = null;
                    else
                        pth.UrlType = src.UrlType;
                });;
        }
    }
}
