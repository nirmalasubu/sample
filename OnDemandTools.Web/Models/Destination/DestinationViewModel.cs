using OnDemandTools.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Web.Models.Destination
{
    public class DestinationViewModel : IModel
    {
        public DestinationViewModel()
        {
            Deliverables = new List<Deliverable>();
            Properties = new List<Property>();
            Categories = new List<Category>();
            Content = new Content();

            this.UpdateCreatedBy();
        }

        public string Id { get; set; }

        public int ExternalId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<Property> Properties { get; set; }

        public IEnumerable<Deliverable> Deliverables { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public Content Content { get; set; }

        public bool AuditDelivery { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }
    }
}
