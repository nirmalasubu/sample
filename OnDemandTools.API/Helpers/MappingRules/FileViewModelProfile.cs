using AutoMapper;
using OnDemandTools.API.v1.Models.File;
using OnDemandTools.Business.Modules.File.Model;
using OnDemandTools.Common.Extensions;
using RQModel = OnDemandTools.API.v1.Models.File;
using BLModel = OnDemandTools.Business.Modules.File.Model;
using VMAiringLongFileModel = OnDemandTools.API.v1.Models.Airing.Long;

namespace OnDemandTools.API.Helpers.MappingRules
{
    public class FileViewModelProfile: Profile
    {
        public FileViewModelProfile()
        {
            CreateMap<RQModel.FileViewModel, BLModel.File>()
               .ForMember(dest => dest.Contents, opt => opt.Condition(src => (!src.Contents.IsNullOrEmpty() && src.Contents.Count > 0)));
            CreateMap<RQModel.FileContentViewModel, BLModel.Content>()
                .ForMember(dest => dest.MediaCollection, opt => opt.MapFrom(src => src.Media));
            CreateMap<RQModel.FileMediaViewModel, BLModel.Media>();
            CreateMap<RQModel.FileCaptionViewModel, BLModel.Caption>();
            CreateMap<RQModel.FileContentSegmentViewModel, BLModel.ContentSegment>();
            CreateMap<RQModel.FilePlayListViewModel, BLModel.PlayList>();
            CreateMap<RQModel.FileUrlViewModel, BLModel.Url>();


            CreateMap<BLModel.File, RQModel.FileViewModel>();
            CreateMap<BLModel.Content, RQModel.FileContentViewModel>()
                .ForMember(dest => dest.Media, opt => opt.MapFrom(src => src.MediaCollection));
            CreateMap<BLModel.Media, RQModel.FileMediaViewModel>();
            CreateMap<BLModel.Caption, RQModel.FileCaptionViewModel>();
            CreateMap<BLModel.ContentSegment, RQModel.FileContentSegmentViewModel>();
            CreateMap<BLModel.PlayList, RQModel.FilePlayListViewModel>();
            CreateMap<BLModel.Url, RQModel.FileUrlViewModel>();

            // BL to Airing  long File Model
            CreateMap<BLModel.File, VMAiringLongFileModel.File>();
            CreateMap<BLModel.Content, VMAiringLongFileModel.Content>();
            CreateMap<BLModel.Media, VMAiringLongFileModel.Media>();
            CreateMap<BLModel.Caption, VMAiringLongFileModel.Caption>();
            CreateMap<BLModel.ContentSegment, VMAiringLongFileModel.ContentSegment>();
            CreateMap<BLModel.PlayList, VMAiringLongFileModel.PlayList>();
            CreateMap<BLModel.Url, VMAiringLongFileModel.Url>();
        }
    }
}
