using System;
using System.Collections.Generic;

namespace OnDemandTools.Business.Modules.AiringPublisher.Models
{
    public class QueueAiring
    {
        public string AiringId { get; set; }
        public string Action { get; set; }
        public DateTime DeliveredOn { get; set; }
        public List<AiringChangeNotification> AiringChangeNotifications { get; set; }
    }

    public class AiringChangeNotification
    {
        public string ChangeNotificationType { get; set; }
        public List<string> ChangedProperties { get; set; }
    }
}
