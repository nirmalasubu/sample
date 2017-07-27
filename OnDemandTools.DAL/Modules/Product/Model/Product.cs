using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OnDemandTools.Common.Model;
using System;
using System.Collections.Generic;

namespace OnDemandTools.DAL.Modules.Product.Model
{
    [BsonIgnoreExtraElements]
    public class Product:IModel
    {
        public Product()
        {
            Destinations = new List<string>();
            Tags = new List<string>();

            ExternalId = Guid.NewGuid();
        }

        public ObjectId Id { get; set; }

        public Guid ExternalId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int MappingId { get; set; }

        public List<string> Tags { get; set; }

        public List<string> Destinations { get; set; }

        public bool DynamicAdTrigger { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }
    }
}
