using OnDemandTools.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Web.Models.Destination
{
    public class CategoryViewModel
    {
        public CategoryViewModel()
        {
            Destinations = new List<DestinationViewModel>();
        }
        public string Name { get; set; }

        public IEnumerable<DestinationViewModel> Destinations { get; set; }
    }
}
