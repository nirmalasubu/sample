using System;
using OnDemandTools.DAL.Modules.Airings.Model.Queues;

namespace OnDemandTools.Jobs.JobRegistry.Models
{
    public class Envelope
    {
        public string AiringId { get; set; }
        public byte? MessagePriority { get; set; }
        public QueueAiring Message { get; set; }
        public string MediaId { get; set; }
        public DateTime PostMarkedDateTime { get; set; }
    }
}