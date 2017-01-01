using System;
using System.Collections.Generic;

namespace OnDemandTools.API.v1.Models.Airing.Short
{
    public class Flight
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public List<Destination> Destinations { get; set; }

        public List<String> Tags { get; set; }

        public Flight()
        {
            Destinations = new List<Destination>();
            Tags = new List<string>();
        }
    }
}