using OnDemandTools.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.Product.Model
{

    public class Product : IModel
    {
        public Product()
        {
            Destinations = new List<string>();            
        }

        public String Id { get; set; }

        public string ExternalId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int MappingId { get; set; }

        public List<string> Tags { get; set; }

        public List<string> Destinations { get; set; }

        public bool DynamicAdTrigger { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }
    }
}

