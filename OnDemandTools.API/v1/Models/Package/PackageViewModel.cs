using Newtonsoft.Json;
using OnDemandTools.Common.Model;
using System.Collections.Generic;
using System;

namespace OnDemandTools.API.v1.Models.Package
{
    public class PackageViewModel:IModel
    {
        public PackageViewModel()
        {
            TitleIds = new List<int>();
            ContentIds = new List<string>();
        }
        public List<int> TitleIds { get; set; }
        public List<string> ContentIds { get; set; }
        public string DestinationCode { get; set; }

        public string Type { get; set; }

        [JsonConverter(typeof(PackageDataConverter))]
        public object PackageData { get; set; }

        public string Data { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedDateTime { get; set; }
    }
}
