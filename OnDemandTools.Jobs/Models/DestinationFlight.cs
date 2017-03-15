using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Jobs.Models
{
    public class DestinationFlight
    {
        public int Id { get; set; }
        public string Destination { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public int AiringDestinationId { get; set; }

        public virtual AiringDestination AiringDestination { get; set; }
    }
}
