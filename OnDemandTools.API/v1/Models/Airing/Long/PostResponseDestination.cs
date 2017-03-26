using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace OnDemandTools.API.v1.Models.Airing.Long
{
    public class PostResponseDestination
    {
        public int ExternalId { get; set; }

        public string Name { get; set; }

        public bool AuthenticationRequired { get; set; }

        public Package Package { get; set; }

        public List<Property> Properties { get; set; }

        public List<Deliverable> Deliverables { get; set; }

        public PostResponseDestination()
        {
            Package = new Package();
            Properties = new List<Property>();
            Deliverables = new List<Deliverable>();
        }

        #region Serialization
        public bool ShouldSerializeProperties()
        {
            return (Properties.Any());
        }

        public bool ShouldSerializeDeliverables()
        {
            return (Deliverables.Any());
        }
        #endregion
    }
}