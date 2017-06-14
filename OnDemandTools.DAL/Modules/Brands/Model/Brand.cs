using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OnDemandTools.DAL.Modules.Brands.Model
{
    [BsonIgnoreExtraElements]
    public class Brand
    {
        public Brand(string name)
        {
            Name = name;
        }

        [BsonId]
        [BsonIgnoreIfDefault]
        public virtual ObjectId Id { get; set; }

        public virtual string Name { get; set; }
    }
}
