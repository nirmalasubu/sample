using System;

namespace OnDemandTools.API.v1.Models.Airing.Long
{
    public class AiringLink
    {
        public DateTime? Date { get; set; }

        public int AiringId { get; set; }

        public bool Linked { get; set; }

        public string Authority { get; set; }
    }
}