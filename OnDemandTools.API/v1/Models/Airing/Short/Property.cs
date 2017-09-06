using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.API.v1.Models.Airing.Short
{
    public class Property
    {
        public Property()
        {
            Brands = new List<string>();
            TitleIds = new List<int>();
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public List<string> Brands { get; set; }

        public List<int> TitleIds { get; set; }
        
    }
}
