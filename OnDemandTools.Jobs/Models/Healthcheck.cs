using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Jobs.Models
{
    public class Healthcheck
    {
        public bool IsAppHealthy { get; internal set; }
        public string DeporterAgentsHealth { get; internal set; }       
        public string PublisherAgentsHealth { get; internal set; }
        public string TitleSyncAgentsHealth { get; internal set; }
    }
}
