using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace OnDemandTools.DAL.Modules.Pathing.Model
{
    public class PathInfo
    {
        public virtual String BaseUrl { get; set; }
        [BsonIgnoreIfNull]
        public virtual String Brand { get; set; }
        [BsonIgnoreIfNull]
        public virtual String ProtectionType { get; set; }

        [BsonIgnoreIfNull]
        public virtual String UrlType { get; set; }

    }

}
