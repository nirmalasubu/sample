using System.Collections.Generic;

namespace OnDemandTools.Business.Modules.Airing.Model.Alternate.Title
{
    public class Title
    {
        public Title()
        {
            Ratings = new List<Rating>();
            Participants = new List<Participant>();
            Storylines = new List<Storyline>();
            Genres = new List<Genre>();
        }

        public int TitleId { get; set; }
        public int? SeriesTitleId { get; set; }
        public int SeasonNumber { get; set; }
        public int ReleaseYear { get; set; }
        
        public string TitleName { get; set; }
        public string TitleNameSortable { get; set; }
        public string TitleTypeCode { get; set; }
        public TitleType TitleType { get; set; }
        public string SeriesTitleName { get; set; }
        public string SeriesTitleNameSortable { get; set; }
        public string EpisodeNumber { get; set; }
        public string SeasonEpisodeNumber { get; set; }
        public string SeriesItemNumber { get; set; }

        public string ExternalSeriesItemNumber { get; set; }
        public string SeasonElementNumber { get; set; }

        public int? AiringSequenceNumber {get; set;}

        public List<Genre> Genres {get; set;}
        public List<Rating> Ratings { get; set; }
        public List<Participant> Participants { get; set; }
        public List<Storyline> Storylines { get; set; }
        public List<OtherName> OtherNames { get; set; }
        public List<ExternalSource> ExternalSources { get; set; }
    }
}
