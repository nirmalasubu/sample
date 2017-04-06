using System;
using System.Collections.Generic;

namespace OnDemandTools.Business.Modules.Airing.Model
{
    public class ChangeNotification
    {
        public ChangeNotification()
        {
            ChangedProperties = new List<string>();
        }

        public string QueueName { get; set; }

        public string ChangeNotificationType { get; set; }

        public List<string> ChangedProperties { get; set; }

        public DateTime ChangedDateTime { get; set; }
    }
}
