using System.Collections.Generic;

namespace OnDemandTools.DAL.Modules.Airings.Model
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