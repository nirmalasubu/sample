using OnDemandTools.Business.Modules.Airing.Model.Alternate.Title;

namespace OnDemandTools.Web.Models.TitleSearch
{
    public class TitleShort
    {
        public int TitleId { get; set; }
        public int ReleaseYear { get; set; }

        public string TitleName { get; set; }
        public string TitleNameSortable { get; set; }
        public TitleType TitleType { get; set; }
        public string SeriesTitleName { get; set; }
        public string SeriesTitleNameSortable { get; set; }
    }
}
