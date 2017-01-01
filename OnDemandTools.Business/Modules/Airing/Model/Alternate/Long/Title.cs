using System;
using System.Collections.Generic;

namespace OnDemandTools.Business.Modules.Airing.Model.Alternate.Long
{
    public class Title
    {
        public Rating Rating { get; set; }

        public Story StoryLine { get; set; }

        public int ReleaseYear { get; set; }

        public string Keywords { get; set; }

        public DateTime OriginalPremiereDate { get; set; }

        public Episode Episode { get; set; }

        public Series Series { get; set; }

        public Season Season { get; set; }

        public List<Participant> Participants { get; set; }

        public List<TitleId> RelatedTitleIds { get; set; }

        public List<TitleId> TitleIds { get; set; }

        public Title()
        {
            Rating = new Rating();
            StoryLine = new Story();

            Series = new Series();
            Season = new Season();
            Episode = new Episode();

            Participants = new List<Participant>();
            TitleIds = new List<TitleId>();
            RelatedTitleIds = new List<TitleId>();
        }
    }
}