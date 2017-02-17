using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.Queue.Model
{
    public class HistoricalMessage
    {
    
        public string AiringId { get; set; }
        public string RemoteQueueName { get; set; }
        public byte? MessagePriority { get; set; }
        public string Message { get; set; }
        public string MediaId { get; set; }
        public DateTime DateTime { get; set; }
    }
}
