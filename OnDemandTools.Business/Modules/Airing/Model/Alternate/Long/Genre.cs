using System.Collections.Generic;

namespace OnDemandTools.Business.Modules.Airing.Model.Alternate.Long
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