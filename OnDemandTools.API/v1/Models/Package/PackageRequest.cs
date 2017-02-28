using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace OnDemandTools.API.v1.Models.Package
{
    public class PackageRequest
    {
        public PackageRequest()
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

        #region Serialisation
        public bool ShouldSerializeDestinationCode()
        {
            return !string.IsNullOrEmpty(DestinationCode);
        }

        public bool ShouldSerializeTitleIds()
        {
            return TitleIds.Any();
        }

        public bool ShouldSerializeContentIds()
        {
            return ContentIds.Any();
        }
        #endregion
    }
}
