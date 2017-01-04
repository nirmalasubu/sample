using System;
using System.Collections.Generic;

namespace OnDemandTools.API.v1.Models.Airing.Update
{
    public class Title
    {
        public Rating Rating { get; set; }

        public Story StoryLine { get; set; }

        public int ReleaseYear { get; set; }

        public string Keywords { get; set; }

        public DateTime OriginalPremiereDate { get; set; }

        public Episode Episode { get; set; }
       
        public Element Element { get; set; }

        public Series Series { get; set; }

        public Season Season { get; set; }

        public IList<Participant> Participants { get; set; }

        public IList<TitleId> TitleIds { get; set; }

        public IList<TitleId> RelatedTitleIds { get; set; }

        public Title()
        {
            Rating = new Rating();
            StoryLine = new Story();

            Series = new Series();
            Season = new Season();

            Participants = new List<Participant>();
            RelatedTitleIds = new List<TitleId>();
            TitleIds = new List<TitleId>();
        }
    }
}