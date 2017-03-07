using System.Collections.Generic;

namespace OnDemandTools.API.v1.Models.Airing.Update
{
    public class AiringStatusRequest
    {
        public Dictionary<string, bool> Status { get; set; }

        public AiringStatusRequest()
        {
            Status = new Dictionary<string, bool>();
        }
    }
}