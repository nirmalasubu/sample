using System.Collections.Generic;
using VMTitleModel = OnDemandTools.Business.Modules.Airing.Model.Alternate.Title;
using VMChangeModel = OnDemandTools.Business.Modules.Airing.Model.Alternate.Change;
using VMPackageModel = OnDemandTools.Business.Modules.Airing.Model.Alternate.Package;
using VMDestinationModel = OnDemandTools.Business.Modules.Airing.Model.Alternate.Destination;
using System.Xml.Serialization;
using OnDemandTools.Common;
using Newtonsoft.Json.Linq;

namespace OnDemandTools.Business.Modules.Airing.Model.Alternate.Long
{

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
            Versions = new JArray();
        }
      
        public List<File> Files { get; set; }


        public JArray Premieres { get; set; }

        public JArray Versions { get; set; }

        public List<VMTitleModel.Title> Titles { get; set; }

        public List<VMTitleModel.Title> Series { get; set; }

        public List<VMChangeModel.Change> Changes { get; set; }

        public List<VMPackageModel.Package> Packages { get; set; }

        public List<VMDestinationModel.Destination> Destinations { get; set; }

        public SerializableDictionary<string, bool> Status { get; set; }
    }
}