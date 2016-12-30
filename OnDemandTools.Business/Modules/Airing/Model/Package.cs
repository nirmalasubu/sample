using System.Collections.Generic;

namespace OnDemandTools.Business.Modules.Airing.Model
{
    public class Package
    {
        public string PackageName { get; set; }
        public string FileName { get; set; }
        public string TitleDigital { get; set; }
        public string TitleBrief { get; set; }
        public IList<string> Genres { get; set; }
        public IList<string> SubGenres { get; set; }
        public IList<string> ContentTiers { get; set; }
        public IList<string> ProductCodes { get; set; }
        public IList<string> GuideCategories { get; set; }
        public IList<string> ProgramTypes { get; set; }
        public IList<string> Categories { get; set; }

        public Package()
        {
            Genres = new List<string>();
            SubGenres = new List<string>();
            ContentTiers = new List<string>();
            ProductCodes = new List<string>();
            GuideCategories = new List<string>();
            ProgramTypes = new List<string>();
            Categories = new List<string>();
        }
    }
}