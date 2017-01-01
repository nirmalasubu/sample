using Newtonsoft.Json;
using OnDemandTools.Common;
using OnDemandTools.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace OnDemandTools.API.v1.Models.Airing.Long
{


    public class File
    {
        public File()
        {

        }

        public string MediaId { get; set; }
        public string AiringId { get; set; }
        public string ContentId { get; set; }
        public int? TitleId { get; set; }
        public string Type { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string AspectRatio { get; set; }
        public bool Secure { get; set; }
        public string Match { get; set; }

        public bool ShouldSerializeSecure()
        {
            // Serialize secure property only if contents is empty
            return (Contents.IsNullOrEmpty());
        }

        public bool Video { get; set; }

        [JsonIgnore]
        public bool Unique { get; set; }

        public String ModifiedBy { get; set; }

        public DateTime ModifiedDatetime { get; set; }
          
        public List<Content> Contents { get; set; }
        public bool ShouldSerializeContents()
        {
            // Serialize contents property only it is
            // not null or not empty
            return (!Contents.IsNullOrEmpty());
        }
    }

    public class Item
    {
        public Item()
        {
           
        }

        public Item(string id, string type, int position)
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


   
   [XmlType(Namespace = "http://api.odt.turner.com/schemas/airing/options/File", TypeName = "FileContent")]
    public class Content
    {
        public Content()
        {
            MediaCollection = new List<Media>();
        }

        public String Name { get; set; }
        
        public List<Media> MediaCollection { get; set; }
    }

   
    public class Media
    {
        public Media()
        {
            ContentSegments = new List<ContentSegment>();
            Playlists = new List<PlayList>();
            Captions = new List<Caption>();
        }

        public String Type { get; set; }
        public Double TotalDuration { get; set; }
        
        public List<ContentSegment> ContentSegments { get; set; }
        
        public List<PlayList> Playlists { get; set; }


        public String AdType { get; set; }
        
        public List<Caption> Captions { get; set; }
    }

  
    public class ContentSegment
    {
        public Int32 SegmentIdx { get; set; }
        public Double Start { get; set; }
        public Double Duration { get; set; }
    }

   
    public class PlayList
    {
        public PlayList()
        {
            Properties = new SerializableDictionary<string, object>();
        }

        public String Name { get; set; }
        public String Type { get; set; }       
        public List<SerializableDictionary<string, Url>> Urls { get; set; }
        
        public SerializableDictionary<string, object> Properties { get; set; }

    }

    public class Url
    {
        public String Host { get; set; }
        public String Path { get; set; }
        public String FileName { get; set; }
    }


    public class Caption
    {
        public String Type { get; set; }
        public String Location { get; set; }
        public String Language { get; set; }
    }
}