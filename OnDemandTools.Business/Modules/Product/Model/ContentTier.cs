
using System.Collections.Generic;

namespace OnDemandTools.Business.Modules.Product.Model
{
    public class ContentTier
    {
        public ContentTier()
        {
            Brands = new List<string>();
            TitleIds = new List<int>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public List<string> Brands { get; set; }

        public List<int> TitleIds { get; set; }
    }
}
