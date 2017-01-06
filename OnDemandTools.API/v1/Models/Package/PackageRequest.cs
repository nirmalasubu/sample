﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace OnDemandTools.API.v1.Models.Package
{
    public class PackageRequest
    {
        public PackageRequest()
        {
            TitleIds = new List<int>();
        }
        public List<int> TitleIds { get; set; }

        public string DestinationCode { get; set; }

        public bool ShouldSerializeDestinationCode()
        {
            return !string.IsNullOrEmpty(DestinationCode);
        }

        public string Type { get; set; }

        [JsonConverter(typeof(PackageDataConverter))]
        public object PackageData { get; set; }
    }
}