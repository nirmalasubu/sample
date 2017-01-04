using System.Collections.Generic;

namespace OnDemandTools.API.v1.Models.Airing.Update
{
    public class Genre
    {
        public Genre()
        {
            SubGenres = new List<Genre>();
        }

        public string name { get; set; }

        public IList<Genre> SubGenres { get; set; }
    }
}