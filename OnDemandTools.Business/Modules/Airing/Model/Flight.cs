using System;
using System.Collections.Generic;

namespace OnDemandTools.Business.Modules.Airing.Model

{
    public class Flight
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public IList<Destination> Destinations { get; set; }

        public IList<Product> Products { get; set; }

        public List<String> Tags { get; set; }

        public Flight()
        {
            Destinations = new List<Destination>();
            Tags = new List<string>();
        }
    }
}