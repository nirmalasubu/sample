using System.Collections.Generic;

namespace OnDemandTools.Web.Models.TitleSearch
{
    public class TitleSearchResults
    {
        public List<TitleShort> Titles { get; set; }

        public List<TitleFilterParameter> TitleTypeFilterParameters { get; set; }
        
        public List<TitleFilterParameter> SeriesFilterParameters { get; set; }
    }
}
