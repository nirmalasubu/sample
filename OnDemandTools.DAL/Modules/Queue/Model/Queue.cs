using System;
using System.Security.Principal;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace OnDemandTools.DAL.Modules.Queue.Model
{
    [BsonIgnoreExtraElements]
    public class Queue
    {
        public Queue()
        {
          
            CreatedDateTime = DateTime.UtcNow;
        }

        public virtual ObjectId Id { get; set; }
        public string Name { get; set; }
        public string RoutingKey { get; set; }
        public string Query { get; set; }
        public string FriendlyName { get; set; }
        public string ContactEmailAddress { get; set; }
        public int HoursOut { get; set; }
        public bool Active { get; set; }
        public bool Report { get; set; }

        public bool AllowAiringsWithNoVersion { get; set; }
        public bool BimRequired { get; set; }
        public bool DetectTitleChanges { get; set; }
        public bool DetectImageChanges { get; set; }
        public bool DetectVideoChanges { get; set; }
        public bool DetectPackageChanges { get; set; }
        public bool DetectStatusChanges { get; set; }
        public bool IsPriorityQueue { get; set; }
        public bool IsProhibitResendMediaId { get; set; }
        public List<string> StatusNames { get; set; }

        public DateTime ProcessedDateTime { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }
    }
}