using System.Collections.Generic;

namespace OnDemandTools.API.v1.Models.Airing.Short
{
    public class Title
    {
        public Rating Rating { get; set; }

        public Story StoryLine { get; set; }

        public int ReleaseYear { get; set; }

        public string Keywords { get; set; }

        public Episode Episode { get; set; }

        public Series Series { get; set; }

        public Season Season { get; set; }

        public List<TitleId> TitleIds { get; set; }

        public List<TitleId> RelatedTitleIds { get; set; }

        public Title()
        {
            Rating = new Rating();
            StoryLine = new Story();

            Series = new Series();
            Season = new Season();

            TitleIds = new List<TitleId>();
            RelatedTitleIds = new List<TitleId>();
        }
    }
}