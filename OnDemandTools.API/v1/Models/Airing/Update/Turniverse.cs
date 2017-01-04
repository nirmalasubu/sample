using System;
using System.Collections.Generic;

namespace OnDemandTools.API.v1.Models.Airing.Update
{
    public class Turniverse
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public IList<Feed> Feeds { get; set; }
    }
}