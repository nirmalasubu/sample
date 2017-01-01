using System;
using System.Collections.Generic;

namespace OnDemandTools.API.v1.Models.Airing.Short
{
    public class Airing
    {
        public string AiringId { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Brand { get; set; }

        public string Platform { get; set; }

        public string MediaId { get; set; }

        public Duration Duration { get; set; }

        public Title Title { get; set; }

        public List<Flight> Flights { get; set; }

        public Flags Flags { get; set; }

        public DateTime ReleasedOn { get; set; }

        public string ReleasedBy { get; set; }

        public Airing()
        {
            Flights = new List<Flight>();
            Duration = new Duration();
            Title = new Title();
            Flags = new Flags();
        }
    }
}