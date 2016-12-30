﻿using System;
using System.Collections.Generic;


namespace OnDemandTools.Business.Modules.Airing.Model
{
    public class Title
    {
        public TVRating TVRating { get; set; }

        public Story StoryLine { get; set; }

        public int ReleaseYear { get; set; }

        public string Keywords { get; set; }

        public string Genre { get; set; }

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
            TVRating = new TVRating();
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