using OnDemandTools.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Web.Models.DeliveryQueue
{
    public class DeliveryQueueModel : IModel
    {
        public DeliveryQueueModel()
        {
            this.UpdateCreatedBy();
            MessageCount = "TBD";
            PendingDeliveryCount = "TBD";
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string RoutingKey { get; set; }
        public string Query { get; set; }
        public string FriendlyName { get; set; }
        public string ContactEmailAddress { get; set; }
        public int HoursOut { get; set; }
        public string MessageCount { get; set; }
        public string PendingDeliveryCount { get; set; }
        public bool Active { get; set; }
        public bool Report { get; set; }

        public bool AllowAiringsWithNoVersion { get; set; }
        public bool BimRequired { get; set; }
        public bool DetectTitleChanges { get; set; }
        public bool DetectImageChanges { get; set; }
        public bool DetectVideoChanges { get; set; }
        public bool DetectPackageChanges { get; set; }
        public bool IsPriorityQueue { get; set; }
        public bool IsProhibitResendMediaId { get; set; }
        public IEnumerable<string> StatusNames { get; set; }
        public DateTime ProcessedDateTime { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }
    }
}
