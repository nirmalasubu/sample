using EasyNetQ;
using EasyNetQ.Topology;

namespace OnDemandTools.Business.Modules.AiringPublisher.Models
{
    public class DeliveryDetails
    {
        public IExchange Exchange { get; set; }
        public IAdvancedBus Bus { get; set; }
        public int Limit { get; set; }
    }
}