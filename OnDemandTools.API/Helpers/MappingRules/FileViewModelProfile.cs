using AutoMapper;
using OnDemandTools.API.v1.Models.File;
using OnDemandTools.Business.Modules.File.Model;
using OnDemandTools.Common.Extensions;

namespace OnDemandTools.API.Helpers.MappingRules
{
    public class FileViewModelProfile: Profile
    {
        public FileViewModelProfile()
        {
            CreateMap<FileViewModel, File>()
               .ForMember(dest => dest.Contents, opt => opt.Condition(src => !src.Contents.IsNullOrEmpty()));
            CreateMap<FileContentViewModel, Content>()
                .ForMember(dest => dest.MediaCollection, opt => opt.MapFrom(src => src.Media));
            CreateMap<FileMediaViewModel, Media>();
            CreateMap<FileCaptionViewModel, Caption>();
            CreateMap<FileContentSegmentViewModel, ContentSegment>();
            CreateMap<FilePlayListViewModel, PlayList>();
            CreateMap<FileUrlViewModel, Url>();


            CreateMap<File, FileViewModel>();
            CreateMap<Content, FileContentViewModel>()
                .ForMember(dest => dest.Media, opt => opt.MapFrom(src => src.MediaCollection));
            CreateMap<Media, FileMediaViewModel>();
            CreateMap<Caption, FileCaptionViewModel>();
            CreateMap<ContentSegment, FileContentSegmentViewModel>();
            CreateMap<PlayList, FilePlayListViewModel>();
            CreateMap<Url, FileUrlViewModel>();

        }
    }
}
