using System.Linq;
using AutoMapper;
using MongoDB.Bson;
using OnDemandTools.Web.Models.PathTranslation;
using BLModel = OnDemandTools.Business.Modules.Pathing.Model;


namespace OnDemandTools.Web.Mappings
{
    public class PathTranslationProfile : Profile
    {
        public PathTranslationProfile()
        {
            CreateMap<PathTranslationViewModel, BLModel.PathTranslation>();
            CreateMap<PathInfo, BLModel.PathInfo>();

            CreateMap<BLModel.PathTranslation, PathTranslationViewModel>();
            CreateMap<BLModel.PathInfo, PathInfo>();
        }
    }
}