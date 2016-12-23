using MongoDB.Bson.Serialization.Attributes;

namespace OnDemandTools.DAL.Modules.Destination.Model
{
    [BsonIgnoreExtraElements]
    public class Content
    {
        public bool HighDefinition { get; set; }
        public bool StandardDefinition { get; set; }
        public bool Cx { get; set; }
        public bool NonCx { get; set; }
    }
}
