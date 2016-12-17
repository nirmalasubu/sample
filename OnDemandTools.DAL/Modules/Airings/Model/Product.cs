using MongoDB.Bson.Serialization.Attributes;
using System;

namespace OnDemandTools.DAL.Modules.Airings.Model
{
    public class Product
    {
        public Guid ExternalId { get; set; }

        [BsonDefaultValue(false, SerializeDefaultValue = true)]
        public bool IsAuth { get; set; }
    }
}