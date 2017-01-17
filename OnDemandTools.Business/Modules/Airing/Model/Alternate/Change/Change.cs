using System;
using BLAiringLongModel = OnDemandTools.Business.Modules.Airing.Model.Alternate.Long;
namespace OnDemandTools.Business.Modules.Airing.Model.Alternate.Change
{
    public class Change
    {
        public string Series { get; set; }
        public string Name { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public string TheChange { get; set; }

        public bool IsChangeDetailed { get; set; }

        public BLAiringLongModel.Airing Airing { get; set; }
    }
}