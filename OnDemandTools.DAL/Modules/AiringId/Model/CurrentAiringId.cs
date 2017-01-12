using System;
using System.Security.Principal;
using MongoDB.Bson;
using OnDemandTools.Common.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace OnDemandTools.DAL.Modules.AiringId.Model
{
    [BsonIgnoreExtraElements]
    public class CurrentAiringId : IModel
    {
       
        public CurrentAiringId()
        {
           
        }

        public ObjectId Id { get; set; }
        public string AiringId { get; set; }
        public string Prefix { get; set; }
        public int SequenceNumber { get; set; }
        public BillingNumber BillingNumber { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }

        public Boolean Locked { get; set; }
    }
}