using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;
using VMTitleModel = OnDemandTools.API.v1.Models.Airing.Title;
using VMChangeModel = OnDemandTools.API.v1.Models.Airing.Change;
using VMPackageModel = OnDemandTools.API.v1.Models.Airing.Package;
using VMDestinationModel = OnDemandTools.API.v1.Models.Airing.Destination;

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
        }
      
        public List<File> Files { get; set; }

        public List<VMTitleModel.Title> Titles { get; set; }

        public List<VMTitleModel.Title> Series { get; set; }

        public List<VMChangeModel.Change> Changes { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<VMPackageModel.Package> Packages { get; set; }

        public List<VMDestinationModel.Destination> Destinations { get; set; }


        public Status Status { get; set; }
        public bool ShouldSerializeStatus()
        {
            // Serialize if status not null
            return (Status != null);
        }

        public bool ShouldSerializeFiles()
        {
            // Serialize if files not empty
            return (Files.Count > 0);
        }

        public bool ShouldSerializeTitles()
        {
            // Serialize if Titles not empty
            return (Titles.Count > 0);
        }

        public bool ShouldSerializeSeries()
        {
            // Serialize if Series not empty
            return (Series.Count > 0);
        }

        public bool ShouldSerializeChanges()
        {
            // Serialize if Changes not empty
            return (Changes.Count > 0);
        }

        public bool ShouldSerializeDestinations()
        {
            // Serialize if Destinations not empty
            return (Destinations.Count > 0);
        }

    }
}