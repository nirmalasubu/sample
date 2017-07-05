using System.Collections.Generic;

namespace OnDemandTools.API.v1.Models.Airing.Destination
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

        public List<string> Brands { get; set; }

        public List<int> TitleIds { get; set; }

        public List<int> SeriesIds { get; set; }
    }
}