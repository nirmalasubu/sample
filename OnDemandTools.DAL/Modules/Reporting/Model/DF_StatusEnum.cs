using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace OnDemandTools.DAL.Modules.Reporting.Model
{
    [BsonIgnoreExtraElements]
    public partial class DF_StatusEnum
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int StatusID { get; set; }
        public int Enum { get; set; }
        public string Value { get; set; }
        public Nullable<int> TypeEnum { get; set; }
    }
}
