using System.Collections.Generic;

namespace OnDemandTools.Web.Models.Category
{
    public class CategoryImportViewModel
    {
        public CategoryImportViewModel()
        {
            Categories = new List<Destination.Category>();
        }

        public int MappingId { get; set; }

        public IEnumerable<Destination.Category> Categories { get; set; }
    }
}
