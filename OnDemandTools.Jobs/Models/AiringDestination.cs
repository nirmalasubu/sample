using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Jobs.Models
{
    public class AiringDestination
    {
        public AiringDestination()
        {
            this.DestinationFlights = new HashSet<DestinationFlight>();
        }

        public int Id { get; set; }
        public string AiringId { get; set; }
        public string Destination { get; set; }

        public virtual Airing Airing { get; set; }
        
        public virtual ICollection<DestinationFlight> DestinationFlights { get; set; }
    }
}
