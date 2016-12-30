using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace OnDemandTools.DAL.Modules.File.Model
{
    /// <summary>
    /// Model elements designated for Encoding
    /// </summary>
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
        [BsonIgnoreIfNull]
        public String AdType { get; set; }
        public List<Caption> Captions { get; set; }
    }

    /// <summary>
    /// Model elements designated for Encoding
    /// </summary>
    public class ContentSegment
    {
        public Int32 SegmentIdx { get; set; }
        public Double Start { get; set; }
        public Double Duration { get; set; }
    }

    /// <summary>
    /// Model elements designated for Encoding
    /// </summary>
    public class PlayList
    {
        public PlayList()
        {
            Properties = new Dictionary<string, Object>();
        }

        public String Name { get; set; }
        public String Type { get; set; }

        public List<Dictionary<string, Url>> Urls { get; set; }
        public Dictionary<string, Object> Properties { get; set; }

    }

   

    public class Caption
    {
        public String Type { get; set; }
        public String Location { get; set; }
        public String Language { get; set; }
    }
}
