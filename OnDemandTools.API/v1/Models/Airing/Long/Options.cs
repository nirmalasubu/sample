using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;
using VMTitleModel = OnDemandTools.API.v1.Models.Airing.Title;
using VMChangeModel = OnDemandTools.API.v1.Models.Airing.Change;
using VMPackageModel = OnDemandTools.API.v1.Models.Airing.Package;
using VMDestinationModel = OnDemandTools.API.v1.Models.Airing.Destination;
using OnDemandTools.Common;
using Newtonsoft.Json.Linq;

namespace OnDemandTools.API.v1.Models.Airing.Long
{
    [XmlRoot("Airing", Namespace = "http://api.odt.turner.com/schemas/airing/options")]
    public class Options
    {
        public Options()
        {
            Files = new List<File>();
            Titles = new List<VMTitleModel.Title>();
            Series = new List<VMTitleModel.Title>();
            Destinations = new List<VMDestinationModel.Destination>();
            Changes = new List<VMChangeModel.Change>();
            Status = new SerializableDictionary<string, bool>();
            Premieres = new JArray();
        }

        public JArray Premieres { get; set; }
        public List<File> Files { get; set; }

        public List<VMTitleModel.Title> Titles { get; set; }

        public List<VMTitleModel.Title> Series { get; set; }

        public List<VMChangeModel.Change> Changes { get; set; }

        public List<VMPackageModel.Package> Packages { get; set; }

        public List<VMDestinationModel.Destination> Destinations { get; set; }
     
        [JsonConverter(typeof(AiringStatusConverter))]
        public SerializableDictionary<string, bool> Status { get; set; }

    }
}