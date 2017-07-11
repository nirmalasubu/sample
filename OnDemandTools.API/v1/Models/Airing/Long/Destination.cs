using System.Collections.Generic;
using System.Xml.Serialization;

namespace OnDemandTools.API.v1.Models.Airing.Long
{
    public class Destination
    {
        public int ExternalId { get; set; }

        public string Name { get; set; }

        public bool AuthenticationRequired { get; set; }

        public Package Package { get; set; }

        public List<Property> Properties { get; set; }

        public List<Deliverable> Deliverables { get; set; }

        public Destination()
        {
            Package = new Package();
            Properties = new List<Property>();
            Deliverables = new List<Deliverable>();
        }
    }    
}