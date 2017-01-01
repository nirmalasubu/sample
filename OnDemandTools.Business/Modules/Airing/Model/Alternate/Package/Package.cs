using System.Collections.Generic;

namespace OnDemandTools.Business.Modules.Airing.Model.Alternate.Package
{
    public class Package
    {
        public Package()
        {
            TitleIds = new List<int>();
        }
        public List<int> TitleIds { get; set; }

        public string DestinationCode { get; set; }

        public string Type { get; set; }
        public object PackageData { get; set; }

        public string Data { get; set; }
    }
}