using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Web.Models.DeliveryQueue
{
    public class DeliveryQueueHubModel
    {
        [JsonProperty("name")]
        public string FriendlyName { get; set; }

        [JsonProperty("order")]
        public string Name { get; set; }

        [JsonProperty("messageCount")]
        public int MessageCount { get; set; }

        [JsonProperty("pendingDeliveryCount")]
        public int PendingDeliveryCount { get; set; }

        [JsonProperty("processedDateTime")]
        public DateTime ProcessedDateTime { get; set; }
    }


}
