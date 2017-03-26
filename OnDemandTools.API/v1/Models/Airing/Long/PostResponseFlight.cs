using System;
using System.Collections.Generic;

namespace OnDemandTools.API.v1.Models.Airing.Long
{
    public class PostResponseFlight
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public List<PostResponseDestination> Destinations { get; set; }
        
        public List<String> Tags { get; set; }

        public PostResponseFlight()
        {
            Destinations = new List<PostResponseDestination>();
            Tags = new List<string>();
        }
    }
}