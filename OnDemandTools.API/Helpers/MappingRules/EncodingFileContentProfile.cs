using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using OnDemandTools.Common.Extensions;
using RQModel = OnDemandTools.API.v1.Models.Handler;
using BLModel = OnDemandTools.Business.Modules.File.Model;
using System.Text.RegularExpressions;

namespace OnDemandTools.API.Helpers.MappingRules
{

    public class EncodingFileContentProfile : Profile
    {
        public EncodingFileContentProfile()
        {
            // Map all encoding file (data contracts) content to file (business model)
            CreateMap<RQModel.EncodingFileContentViewModel, BLModel.File>()
               .ApplyEncodingFileContentToFile();
            CreateMap<RQModel.MediaViewModel, BLModel.Media>()
              .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Output))
              .ForMember(dest => dest.Captions, opt => opt.ResolveUsing<EncodingCaptionsResolver>());
            CreateMap<RQModel.ContentSegmentViewModel, BLModel.ContentSegment>();
            CreateMap<RQModel.PlayListViewModel, BLModel.PlayList>()
                .ApplyEncodingPlaylistToFilePlaylist();
        }
    }

    public static class MappingExpressionExtensions
    {
        /// <summary>
        /// Ignores all members.
        /// </summary>
        /// <typeparam name="TSrc">The type of the source.</typeparam>
        /// <typeparam name="TDest">The type of the dest.</typeparam>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        public static IMappingExpression<TSrc, TDest> IgnoreAllMembers<TSrc, TDest>(this IMappingExpression<TSrc, TDest> mapping)
            where TSrc : class
            where TDest : class
        {

            mapping
                .ForAllMembers(opt => opt.Ignore());
            return mapping;
        }

        /// <summary>
        /// Applies logic to translate encoding file content to file.
        /// Assumption: according to the current business rule, all encoding payload
        /// is video related. Hence set 'Video' flag to true during
        /// translation
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDest">The type of the dest.</typeparam>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        public static IMappingExpression<TSource, TDest> ApplyEncodingFileContentToFile<TSource, TDest>(this IMappingExpression<TSource, TDest> mapping)
            where TSource : RQModel.EncodingFileContentViewModel
            where TDest : BLModel.File
        {
            mapping.ForMember(dest => dest.AiringId, opt => opt.Ignore());
            mapping.ForMember(dest => dest.Video, opt => opt.UseValue(true));
            mapping.ForMember(dest => dest.MediaId, opt => opt.MapFrom(src => src.MediaId));
            mapping.ForMember(dest => dest.Contents, opt => opt.ResolveUsing<EncodingContentsResolver>());
            return mapping;
        }


        /// <summary>
        /// Applies the encoding playlist to file playlist.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDest">The type of the dest.</typeparam>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        public static IMappingExpression<TSource, TDest> ApplyEncodingPlaylistToFilePlaylist<TSource, TDest>(this IMappingExpression<TSource, TDest> mapping)
            where TSource : RQModel.PlayListViewModel
            where TDest : BLModel.PlayList
        {
            mapping.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            mapping.ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type));
            mapping.ForMember(dest => dest.Urls, opt => opt.ResolveUsing<EncodingURLResolver>());
            mapping.ForMember(dest => dest.Properties, opt => opt.ResolveUsing<EncodingPropertiesResolver>());
            return mapping;
        }

    }

    public class EncodingContentsResolver : IValueResolver<RQModel.EncodingFileContentViewModel, BLModel.File, List<BLModel.Content>>
    {
        public List<BLModel.Content> Resolve(RQModel.EncodingFileContentViewModel source, BLModel.File des, List<BLModel.Content> d, ResolutionContext context)
        {
            List<BLModel.Content> contents = new List<BLModel.Content>();

            BLModel.Content c = new BLModel.Content();
            contents.Add(c);
            c.MediaCollection = Mapper.Map<List<RQModel.MediaViewModel>, List<BLModel.Media>>(source.MediaCollection);
            c.Name = c.MediaCollection.FirstOrDefault().Type;
            return contents;

        }

    }

    /// <summary>
    /// Automapper resolver to resolve
    /// PlayList
    /// to
    /// System.Collections.Generic.Dictionary{System.String,System.Object}
    /// </summary>      
    public class EncodingPropertiesResolver : IValueResolver<RQModel.PlayListViewModel, BLModel.PlayList, Dictionary<String, Object>>
    {
        public Dictionary<String, Object> Resolve(RQModel.PlayListViewModel source, BLModel.PlayList des, Dictionary<String, Object> d, ResolutionContext context)
        {
            Dictionary<String, Object> props = new Dictionary<String, Object>();
            props.Add("IsAVMUX", source.IsAVMUX);
            props.Add("MediaContainer", source.MediaContainer);
            props.Add("Encryption", source.Encryption);
            props.Add("DRMAccess", source.DRMAccess);
            props.Add("DRMWideVine", source.DRMWideVine);
            props.Add("DRMFairPlay", source.DRMFairPlay);
            props.Add("DRMClearKey", source.DRMClearKey);
            props.Add("AssetId", source.AssetId);
            if (!source.ProtectionType.IsNullOrEmpty())
                props.Add("ProtectionType", source.ProtectionType);
            return props;
        }
    }

    /// <summary>
    /// Automapper resolve individual caption string (608, 708 etc) to 
    /// appropriate caption type in ODT
    /// </summary>    
    ///     
    public class EncodingCaptionsResolver : IValueResolver<RQModel.MediaViewModel, BLModel.Media, List<BLModel.Caption>>
    {
        public List<BLModel.Caption> Resolve(RQModel.MediaViewModel source, BLModel.Media des, List<BLModel.Caption> d, ResolutionContext context)
        {
            // Create empty captions list
            List<BLModel.Caption> captions = new List<BLModel.Caption>();

            // Split captionSource and create individual caption for each entry
            String[] captionsEntries = source.ClosedCaptionsType.Split('/');

            foreach (String cap in captionsEntries)
            {
                switch (cap)
                {
                    case "608":
                        {
                            BLModel.Caption c = new BLModel.Caption();
                            c.Language = "eng";
                            c.Location = "cc1";
                            c.Type = "608";
                            captions.Add(c);
                            break;
                        }
                    case "708":
                        {
                            BLModel.Caption c = new BLModel.Caption();
                            c.Language = "eng";
                            c.Location = "svc1";
                            c.Type = "708";
                            captions.Add(c);
                            break;
                        }
                    case "webvtt-sidecar":
                        {
                            BLModel.Caption c = new BLModel.Caption();
                            c.Language = "pt";
                            c.Location = "./sidecar.vtt";
                            c.Type = "webvtt-sidecar";
                            captions.Add(c);
                            break;
                        }
                    case "webvtt-manifest":
                        {
                            BLModel.Caption c = new BLModel.Caption();
                            c.Language = "pt";
                            c.Location = "m3u8";
                            c.Type = "webvtt-manifest";
                            captions.Add(c);
                            break;
                        }
                    default:
                        break;
                }
            }

            return captions;
        }
    }

    /// <summary>
    /// Automapper resolver to resolve
    /// PlayList
    /// to
    /// System.Collections.Generic.List{System.Collections.Generic.Dictionary{System.String, OnDemandTools.DAL.Modules.File.Model.Url}
    /// </summary>
    /// <seealso cref="AutoMapper.ValueResolver{,}" />
    ///     
    public class EncodingURLResolver : IValueResolver<RQModel.PlayListViewModel, BLModel.PlayList, List<Dictionary<String, BLModel.Url>>>
    {
        public List<Dictionary<String, BLModel.Url>> Resolve(RQModel.PlayListViewModel source, BLModel.PlayList des, List<Dictionary<String, BLModel.Url>> d, ResolutionContext context)
        {
            List<Dictionary<String, BLModel.Url>> urls = new List<Dictionary<String, BLModel.Url>>();
            Regex regex = new Regex(@"^((http[s]?|ftp):\/)?\/?([^:\/\s]+)((\/\w+)*\/)([\w\-\.]+[^#?\s]+)(.*)?(#[\w\-]+)?$");

            foreach (var translatedUrl in source.TranslatedUrls)
            {
                if (!string.IsNullOrEmpty(translatedUrl.Url))
                {
                    var matches = regex.Match(translatedUrl.Url);
                    urls.Add(BuildUrlFor(matches, translatedUrl.UrlType));
                }
            }
            return urls;
        }

        private Dictionary<String, BLModel.Url> BuildUrlFor(Match matches, string key)
        {
            var result = new Dictionary<String, BLModel.Url>();
            var host = matches.Groups[1].ToString() + @"/" + matches.Groups[3].ToString();
            var path = matches.Groups[4].ToString();
            var fileName = matches.Groups[6].ToString();
            result.Add(key, new BLModel.Url() { Host = host, Path = path, FileName = fileName });
            return result;
        }
    }
}
