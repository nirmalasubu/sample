using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Web.Models.DeliveryQueue
{
    public class QueuesHubModel
    {
        [JsonProperty("queues")]
        public List<DeliveryQueueHubModel> Queues { get; set; }

        [JsonProperty("jobLastRun")]
        public DateTime JobLastRun { get; set; }

        [JsonProperty("jobCount")]
        public int JobCount { get; set; }
      
    }
}
