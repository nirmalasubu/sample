using System.Collections.Generic;

namespace OnDemandTools.Web.Models.ContentTier
{
    public class ContentTierImportViewModel
    {
        public ContentTierImportViewModel()
        {
            ContentTiers = new List<Product.ContentTier>();
        }

        public int MappingId { get; set; }

        public IEnumerable<Product.ContentTier> ContentTiers { get; set; }
    }
}
