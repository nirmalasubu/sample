using System.Collections.Generic;

namespace OnDemandTools.API.v1.Models.Airing.Long
{
    public class Genre
    {
        public Genre()
        {
            SubGenres = new List<Genre>();
        }

        public string Name { get; set; }

        public List<Genre> SubGenres { get; set; }
    }
}