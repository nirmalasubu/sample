using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;


namespace OnDemandTools.DAL.Modules.Handler.Model
{ 
    /// <summary>
    /// Handler History business model
    /// </summary>
  
    public class HandlerHistory 
    {
        public ObjectId Id { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        [BsonIgnore]
        public string ModifiedBy { get; set; }

        [BsonIgnore]
        public DateTime ModifiedDateTime { get; set; }

        public BsonDocument RawJSONPayload { get; set; }

        public string MediaId { get; set; }
    }
}
