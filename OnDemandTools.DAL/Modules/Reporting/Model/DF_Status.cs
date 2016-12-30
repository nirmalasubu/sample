using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace OnDemandTools.DAL.Modules.Reporting.Model
{
    [BsonIgnoreExtraElements]
    public  class DF_Status
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int StatusID { get; set; }
        public string AssetID { get; set; }
        public string Message { get; set; }
        public Nullable<int> StatusEnum { get; set; }
        public Nullable<int> ReporterEnum { get; set; }
        public Nullable<int> DestinationID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.Guid> UniqueId { get; set; }
    }
}
