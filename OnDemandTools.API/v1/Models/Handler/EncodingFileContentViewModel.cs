using Newtonsoft.Json;
using OnDemandTools.Common;
using OnDemandTools.Common.Model;
using System;
using System.Collections.Generic;

namespace OnDemandTools.API.v1.Models.Handler
{

    public class EncodingFileContentViewModel:IModel
    {
        /// <summary>
        /// This structure serves as the general
        /// data contract for encoding related routes
        /// </summary>
        public EncodingFileContentViewModel()
        {
            MediaCollection = new List<MediaViewModel>();
        }

        [JsonProperty(PropertyName = "odt-airing-id")]
        public String AiringId { get; set; }

        [JsonProperty(PropertyName = "root-id")]
        public String RootId { get; set; }

        [JsonProperty(PropertyName = "odt-media-id")]
        public String MediaId { get; set; }

        [JsonProperty(PropertyName = "output")]
        public List<MediaViewModel> MediaCollection { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedDateTime { get; set; }
    }

    /// <summary>
    /// Datacontract elements designated for Encoding
    /// </summary>
    public partial class MediaViewModel
    {
        public MediaViewModel()
        {
            ContentSegments = new List<ContentSegmentViewModel>();
            Playlists = new List<PlayListViewModel>();
        }


        [JsonProperty(PropertyName = "adType")]
        public String AdType { get; set; }

        [JsonProperty(PropertyName = "closed-captions-type")]
        public String ClosedCaptionsType { get; set; }

        [JsonProperty(PropertyName = "output")]
        public String Output { get; set; }

        [JsonProperty(PropertyName = "total-duration")]
        public Double TotalDuration { get; set; }

        [JsonProperty(PropertyName = "content-segments")]
        public List<ContentSegmentViewModel> ContentSegments { get; set; }

        [JsonProperty(PropertyName = "master-playlists")]
        public List<PlayListViewModel> Playlists { get; set; }
    }

    /// <summary>
    /// Datacontract elements designated for Encoding
    /// </summary>
    public class ContentSegmentViewModel
    {
        [JsonProperty(PropertyName = "segment-idx")]
        public Int32 SegmentIdx { get; set; }

        [JsonProperty(PropertyName = "start")]
        public Double Start { get; set; }

        [JsonProperty(PropertyName = "duration")]
        public Double Duration { get; set; }
    }

    /// <summary>
    /// Datacontract elements designated for Encoding
    /// </summary>
    public class PlayListViewModel
    {
        public PlayListViewModel()
        {
            TranslatedUrls = new List<TranslatedUrlViewModel>();
            Urls = new Dictionary<string, string>();
        }

        [JsonProperty(PropertyName = "name")]
        public String Name { get; set; }

        [JsonProperty(PropertyName = "type")]
        public String Type { get; set; }

        [JsonProperty(PropertyName = "avmux")]
        public Boolean IsAVMUX { get; set; }

        [JsonProperty(PropertyName = "media-container")]
        public String MediaContainer { get; set; }

        [JsonProperty(PropertyName = "encryption")]
        public String Encryption { get; set; }

        [JsonProperty(PropertyName = "drm-access")]
        public Boolean DRMAccess { get; set; }

        [JsonProperty(PropertyName = "drm-widevine")]
        public Boolean DRMWideVine { get; set; }

        [JsonProperty(PropertyName = "drm-fairplay")]
        public Boolean DRMFairPlay { get; set; }

        [JsonProperty(PropertyName = "drm-clearkey")]
        public Boolean DRMClearKey { get; set; }

        [JsonProperty(PropertyName = "bucket-url")]
        public String BucketURL { get; set; }

        [JsonProperty(PropertyName = "urls")]
        public Dictionary<string, string> Urls { get; set; }

        [JsonIgnore]
        public String ProtectionType { get; set; }

        /// <summary>
        /// This property used to store all given urls and translated urls.
        /// </summary>
        [JsonIgnore]        
        public List<TranslatedUrlViewModel> TranslatedUrls { get; set; }

        [JsonProperty(PropertyName = "asset-id")]
        public String AssetId { get; set; }

    }

    public class TranslatedUrlViewModel
    {
        public string UrlType { get; set; }

        public string Url { get; set; }
    }
}
