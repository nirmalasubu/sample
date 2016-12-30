using Newtonsoft.Json;
using OnDemandTools.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.API.v1.Models.File
{

    public class FileViewModel
    {
        public FileViewModel()
        {

        }

        public string MediaId { get; set; }
        public string AiringId { get; set; }
        public string ContentId { get; set; }
        public int? TitleId { get; set; }
        public string Type { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Category { get; set; }



        // Workround for JSON ignore during serialization
        // and not deserialization
        [JsonIgnore]
        public bool Unique { get; set; }

        [JsonProperty("Unique")]
        public bool UniquePseudo
        {
            set { Unique = value; }
        }

        public bool Secure { get; set; }
        public bool ShouldSerializeSecure()
        {
            // Serialize secure property only if contents is empty
            return (Contents.IsNullOrEmpty());
        }

        public bool Video { get; set; }

        public string AspectRatio { get; set; }

        public string Match { get; set; }

        public List<FileContentViewModel> Contents { get; set; }
        public bool ShouldSerializeContents()
        {
            // Serialize contents only if not empty
            return (!Contents.IsNullOrEmpty());
        }
    }

    public class ItemViewModel
    {
        public ItemViewModel(string id, string type, int position)
        {
            Id = id;
            Type = type;
            Position = position;
        }

        public int Position { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public int Length { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>

    public class FileContentViewModel
    {
        public FileContentViewModel()
        {
            Media = new List<FileMediaViewModel>();
        }

        public String Name { get; set; }
        public List<FileMediaViewModel> Media { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class FileMediaViewModel
    {
        public FileMediaViewModel()
        {
            ContentSegments = new List<FileContentSegmentViewModel>();
            Playlists = new List<FilePlayListViewModel>();
            Captions = new List<FileCaptionViewModel>();
        }
        public String Type { get; set; }
        public Double? TotalDuration { get; set; }
        public List<FileContentSegmentViewModel> ContentSegments { get; set; }
        public List<FilePlayListViewModel> Playlists { get; set; }

        public String AdType { get; set; }
        public List<FileCaptionViewModel> Captions { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FileContentSegmentViewModel
    {
        public Int32? SegmentIdx { get; set; }
        public Double? Start { get; set; }
        public Double? Duration { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FilePlayListViewModel
    {
        public FilePlayListViewModel()
        {
            Properties = new Dictionary<string, object>();
           
        }

        public String Name { get; set; }
        public String Type { get; set; }
        [JsonProperty("urls")]
        public List<Dictionary<string, FileUrlViewModel>> Urls { get; set; }
        [JsonProperty("properties")]
        public Dictionary<string, object> Properties { get; set; }
        
    }


    public class FileUrlViewModel
    {
        public String Host { get; set; }
        public String Path { get; set; }
        public String FileName { get; set; }
    }

    public class FileCaptionViewModel
    {
        public String Type { get; set; }
        public String Location { get; set; }
        public String Language { get; set; }
    }
}
