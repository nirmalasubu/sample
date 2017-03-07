using OnDemandTools.Common;

namespace OnDemandTools.API.v1.Models.Airing.Update
{
    public class AiringStatusRequest
    {
        public string ReleasedBy { get; set; }

        public SerializableDictionary<string, bool> Status { get; set; }
    }
}