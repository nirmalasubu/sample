using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OnDemandTools.DAL.Modules.Reporting.Model
{
    [BsonIgnoreExtraElements]
    public partial class DF_Destination
    {
        public DF_Destination()
        {
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("ExternalId")]
        public int DestinationID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool AuditDelivery { get; set; }

        public Nullable<bool> AcceptsHDContent { get; set; }
        public Nullable<bool> AcceptsSDContent { get; set; }
        public Nullable<bool> AcceptsCXContent { get; set; }
        public Nullable<bool> AcceptsNCXContent { get; set; }
    }
}
