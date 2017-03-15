using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Jobs.Models
{
    public class Airing
    {
        public Airing()
        {
            this.AiringDestinations = new HashSet<AiringDestination>();
            this.AiringTitles = new HashSet<AiringTitle>();
        }

        public string AiringId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Nullable<System.DateTime> StartDateTime { get; set; }
        public Nullable<System.DateTime> EndDateTime { get; set; }
        public Nullable<System.DateTime> LinearDateTime { get; set; }
        public Nullable<System.DateTime> ESTStartDateTime { get; set; }
        public Nullable<System.DateTime> ESTEndDateTime { get; set; }
        public Nullable<System.DateTime> ESTLinearDateTime { get; set; }
        public Nullable<System.DateTime> BCStartDateTime { get; set; }
        public Nullable<System.DateTime> BCEndDateTime { get; set; }
        public Nullable<System.DateTime> BCLinearDateTime { get; set; }
        public string Brand { get; set; }
        public System.DateTime UpdatedDateTime { get; set; }
        
        public virtual ICollection<AiringDestination> AiringDestinations { get; set; }
        
        public virtual ICollection<AiringTitle> AiringTitles { get; set; }
    }
}
