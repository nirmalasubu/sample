using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Web.Models.DeliveryQueue
{
    public class GetNotificationHistoryModel
    {
        [JsonProperty("QueueName")]
        public string QueueName { get; set; }

        [JsonProperty("AiringIds")]
        public List<string> AiringIds { get; set; }

        public GetNotificationHistoryModel()
        {
            QueueName = string.Empty;
            AiringIds = new List<string>();
        }
    }
}
