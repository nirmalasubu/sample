using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.API.v1.Models.Airing.Queue
{
    public class QueuePushViewModel
    {
        public List<string> AiringIds { get; set; }
        public string QueueName { get; set; }
    }
}
