﻿using OnDemandTools.Common;
using System;
using System.Collections.Generic;

namespace OnDemandTools.API.v1.Models.Airing.Long
{
    public class Airing
    {
        public string AiringId { get; set; }
       
        public string MediaId { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Brand { get; set; }

        public string Platform { get; set; }

        public List<AiringLink> Airings { get; set; }

        public Duration Duration { get; set; }

        public Title Title { get; set; }

        public List<Flight> Flights { get; set; }

        public Flags Flags { get; set; }

        public List<Version> Versions { get; set; }

        public List<PlayItem> PlayList { get; set; }

        public List<string> DeviceExclusions { get; set; }

        public List<string> WebFlags { get; set; }

        public DateTime ReleasedOn { get; set; }

        public string ReleasedBy { get; set; }

        public Options Options { get; set; }

        public SerializableDictionary<string, object> Properties { get; set; }

        public Airing()
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
          
            Options = new Options();
            Properties = new SerializableDictionary<string, object>();
        }

        public bool ShouldSerializeOptions()
        {
            // Serialize if Options not empty
            return ((Options.Files.Count > 0 || Options.Titles.Count > 0 || Options.Series.Count > 0 ||
                     Options.Changes.Count > 0 || Options.Destinations.Count > 0 || Options.Destinations.Count > 0 || Options.Packages != null));
        }
    }
}