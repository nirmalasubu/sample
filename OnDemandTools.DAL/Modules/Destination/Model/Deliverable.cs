using MongoDB.Bson.Serialization.Attributes;

namespace OnDemandTools.DAL.Modules.Destination.Model
{
    [BsonIgnoreExtraElements]
    public class Deliverable
    {
        public string Value { get; set; }
    }
}
