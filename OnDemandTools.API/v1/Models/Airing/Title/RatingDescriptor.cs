using System;
using System.Collections.Generic;

namespace OnDemandTools.API.v1.Models.Airing.Title
{
    public class RatingDescriptor
    {
        public string NetworkCode { get; set; }
        public string Rating { get; set; }
        public List<string> Descriptors { get; set; }

        public RatingDescriptor()
        {
            Descriptors = new List<string>();
            Rating = String.Empty;
            NetworkCode = String.Empty;
        }
    }
}
