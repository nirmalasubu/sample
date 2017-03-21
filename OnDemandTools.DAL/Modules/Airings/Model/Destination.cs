using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace OnDemandTools.DAL.Modules.Airings.Model
{
    [BsonIgnoreExtraElements]
    public class Destination
    {
        public int ExternalId { get; set; }

        public string Name { get; set; }

        public Package Package { get; set; }

        public bool AuthenticationRequired { get; set; }

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