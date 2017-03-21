using System.Collections.Generic;

namespace OnDemandTools.API.v1.Models.Airing.Short
{
    public class Destination
    {
        public int ExternalId { get; set; }

        public string Name { get; set; }

        public List<Property> Properties { get; set; }

        public List<Deliverable> Deliverables { get; set; }

        public Destination()
        {
            Properties = new List<Property>();
            Deliverables = new List<Deliverable>();
        }
    }




}