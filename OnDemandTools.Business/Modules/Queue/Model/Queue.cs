using OnDemandTools.Common.Model;
using System;

namespace OnDemandTools.Business.Modules.Queue.Model
{
    public class Queue : IModel
    {
        public Queue()
        {
           
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string RoutingKey { get; set; }
        public string Query { get; set; }
        public string FriendlyName { get; set; }
        public string ContactEmailAddress { get; set; }
        public int HoursOut { get; set; }
        public uint MessageCount { get; set; }
        public long PendingDeliveryCount { get; set; }
        public bool Active { get; set; }
        public bool Report { get; set; }

        public bool BimRequired { get; set; }
        public bool DetectTitleChanges { get; set; }
        public bool DetectImageChanges { get; set; }
        public bool DetectVideoChanges { get; set; }
        public bool DetectPackageChanges { get; set; }
        public bool IsPriorityQueue { get; set; }
        public bool IsProhibitResendMediaId { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }
    }
}
