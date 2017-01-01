using System;

namespace OnDemandTools.API.v1.Models.Airing.Change
{
    public class ChangeValue
    {
        public DateTime On { get; set; }
        public string By { get; set; }

        public string Value { get; set; }
    }
}