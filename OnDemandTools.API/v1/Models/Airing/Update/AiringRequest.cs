using System;
using System.Collections.Generic;

namespace OnDemandTools.API.v1.Models.Airing.Update
{
    public class AiringRequest
    {
        public string AiringId { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Brand { get; set; }

        public string Platform { get; set; }

        public IList<AiringLink> Airings { get; set; }

        public Duration Duration { get; set; }

        public Title Title { get; set; }

        public IList<Flight> Flights { get; set; }

        public Flags Flags { get; set; }

        public Turniverse Turniverse { get; set; }

        public IList<Version> Versions { get; set; }

        public IList<PlayItem> PlayList { get; set; }

        public IList<string> DeviceExclusions { get; set; }

        public IList<string> WebFlags { get; set; }

        public Guid ReleaseId { get; set; }

        public string ReleasedBy { get; set; }

        public Instructions Instructions { get; set; }

        public Dictionary<string, object> Properties { get; set; }

        public AiringRequest()
        {
            Airings = new List<AiringLink>();
            Flights = new List<Flight>();
            Versions = new List<Version>();
            PlayList = new List<PlayItem>();
            DeviceExclusions = new List<string>();
            WebFlags = new List<string>();
            Duration = new Duration();
            Title = new Title();
            Flags = new Flags();
            Instructions = new Instructions();
            Properties = new Dictionary<string, object>();
        }
    }
}