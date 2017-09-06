
using System.Collections.Generic;

namespace OnDemandTools.Business.Modules.Destination.Model
{
    public class Category
    {
        public Category()
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
