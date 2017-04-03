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

        public ChangeNotificationType ChangeNotificationType { get; set; }

        public List<string> ChangedProperties { get; set; }
    }
}
