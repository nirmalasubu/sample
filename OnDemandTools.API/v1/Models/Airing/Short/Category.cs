using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.API.v1.Models.Airing.Short
{
   
    public class Category
    {
        public Category()
        {
            Brands = new List<string>();
            TitleIds = new List<int>();
            SeriesIds = new List<int>();
        }

        public string Name { get; set; }

        public string GroupName { get; set; }

        public List<string> Brands { get; set; }

        public List<int> TitleIds { get; set; }

        public List<int> SeriesIds { get; set; }
    }
}
