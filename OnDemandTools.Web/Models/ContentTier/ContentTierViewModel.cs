using OnDemandTools.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnDemandTools.Web.Models.Product;

namespace OnDemandTools.Web.Models.ContentTier
{
    public class ContentTierViewModel
    {
        public ContentTierViewModel()
        {
            Products = new List<ProductViewModel>();
        }
        
        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
