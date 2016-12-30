using System.Collections.Generic;

namespace OnDemandTools.Business.Modules.Airing.Model
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