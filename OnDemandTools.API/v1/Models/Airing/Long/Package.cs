using System.Collections.Generic;

namespace OnDemandTools.API.v1.Models.Airing.Long
{
    public class Package
    {
        public string PackageName { get; set; }
        public string FileName { get; set; }
        public string TitleDigital { get; set; }
        public string TitleBrief { get; set; }
        public List<string> Genres { get; set; }
        public List<string> SubGenres { get; set; }
        public List<string> ContentTiers { get; set; }
        public List<string> ProductCodes { get; set; }
        public List<string> GuideCategories { get; set; }
        public List<string> ProgramTypes { get; set; }
        public List<string> Categories { get; set; }

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