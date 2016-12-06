using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTPOCHarbor.Models
{
    [BsonIgnoreExtraElements]
    public class Product
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        public virtual ObjectId Id { get; set; }

        public Product()
        {
            Destinations = new List<string>();
            Tags = new List<string>();

            ExternalId = Guid.NewGuid();

        }

        public Guid ExternalId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int MappingId { get; set; }

        public List<string> Tags { get; set; }

        public List<string> Destinations { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }
    }
}
