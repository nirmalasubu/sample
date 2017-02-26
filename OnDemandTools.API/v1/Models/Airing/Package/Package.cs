using System.Collections.Generic;
using Newtonsoft.Json;
using OnDemandTools.API.v1.Models.Package;

namespace OnDemandTools.API.v1.Models.Airing.Package
{
    public class Package
    {
        public Package()
        {
            TitleIds = new List<int>();
            ContentIds = new List<string>();
        }
        public string AiringId { get; set; }

        public List<int> TitleIds { get; set; }

        public List<string> ContentIds { get; set; }

        public string DestinationCode { get; set; }

        public string Type { get; set; }

        [JsonConverter(typeof(PackageDataConverter))]
        public object PackageData { get; set; }

        public string Data { get; set; }

        public bool ShouldSerializeTitleIds()
        {
            return (TitleIds.Count > 0);
        }

        public bool ShouldSerializeContentIds()
        {
            return (ContentIds.Count > 0);
        }
    }
}