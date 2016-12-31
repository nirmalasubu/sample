using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;


namespace OnDemandTools.DAL.Modules.Pathing.Model
{
  
    public class PathTranslation 
    {
        public virtual ObjectId Id { get; set; }
        public virtual PathInfo Source { get; set; }
        public virtual PathInfo Target { get; set; }

        public String ModifiedBy { get; set; }

        public DateTime ModifiedDateTime { get; set; }
        [BsonIgnore]
        public string CreatedBy { get; set; }
        [BsonIgnore]
        public DateTime CreatedDateTime { get; set; }

    }
}
