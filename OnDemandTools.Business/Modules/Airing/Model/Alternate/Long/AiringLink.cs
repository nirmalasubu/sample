using System;

namespace OnDemandTools.Business.Modules.Airing.Model.Alternate.Long
{
    public class AiringLink
    {
        public DateTime? Date { get; set; }

        public int AiringId { get; set; }

        public bool Linked { get; set; }

        public string Authority { get; set; }
    }
}