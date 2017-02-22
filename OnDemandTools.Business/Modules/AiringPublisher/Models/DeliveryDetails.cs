

namespace OnDemandTools.Business.Modules.AiringPublisher.Models
{
    public class DeliveryDetails
    {
        public int Limit { get; set; }
        public RabbitMQ.Client.IModel RabbitMqChannel { get; set; }
        public string ExchangeName { get; set; }
    }
}