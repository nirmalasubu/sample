using AutoMapper;
using OnDemandTools.Common.Extensions;
using BLModel = OnDemandTools.Business.Modules.File.Model;
using DLModel = OnDemandTools.DAL.Modules.File.Model;
using BLAltFileModel = OnDemandTools.Business.Modules.Airing.Model.Alternate.Long;

namespace OnDemandTools.API.Utilities.EntityMapping.Rules
{
    public class FileProfile:Profile
    {
        public FileProfile()
        {
            CreateMap<DLModel.File, BLModel.File>();
            CreateMap<DLModel.Item, BLModel.Item>();
            CreateMap<DLModel.Content, BLModel.Content>();
            CreateMap<DLModel.Media, BLModel.Media>();
            CreateMap<DLModel.Caption, BLModel.Caption>();
            CreateMap<DLModel.ContentSegment, BLModel.ContentSegment>();
            CreateMap<DLModel.PlayList, BLModel.PlayList>();
            CreateMap<DLModel.Url, BLModel.Url>();

            CreateMap<BLModel.File, DLModel.File>()
                .ForMember(dest => dest.Contents, opt => opt.Condition(src => (!src.Contents.IsNullOrEmpty() && src.Contents.Count > 0)));
            CreateMap<BLModel.Item, DLModel.Item>();
            CreateMap<BLModel.Content, DLModel.Content>();
            CreateMap<BLModel.Media, DLModel.Media>();
            CreateMap<BLModel.Caption, DLModel.Caption>();
            CreateMap<BLModel.ContentSegment, DLModel.ContentSegment>();
            CreateMap<BLModel.PlayList, DLModel.PlayList>();
            CreateMap<BLModel.Url, DLModel.Url>();


            CreateMap<DLModel.File, BLAltFileModel.File>();
            CreateMap<DLModel.Content, BLAltFileModel.Content>();
            CreateMap<DLModel.Media, BLAltFileModel.Media>();                      
            CreateMap<DLModel.Caption, BLAltFileModel.Caption>();
            CreateMap<DLModel.ContentSegment, BLAltFileModel.ContentSegment>();
            CreateMap<DLModel.PlayList, BLAltFileModel.PlayList>();
            CreateMap<DLModel.Url, BLAltFileModel.Url>();

            //Missing type map configuration or unsupported mapping.

        }
    }
}
