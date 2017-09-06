using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;


namespace OnDemandTools.DAL.Modules.Product.Model
{
    [BsonIgnoreExtraElements]
    public class ContentTier
    {
        public ContentTier()
        {
            Brands = new List<string>();
            TitleIds = new List<int>();
        }

        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public List<string> Brands { get; set; }

        public List<int> TitleIds { get; set; }
    }
}
