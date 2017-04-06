using Newtonsoft.Json;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Business.Modules.AiringPublisher.Models;
using OnDemandTools.Business.Modules.Queue;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLQueue = OnDemandTools.Business.Modules.Queue.Model;

namespace OnDemandTools.Business.Modules.AiringPublisher.Workflow
{
    public class EnvelopeDistributor : IEnvelopeDistributor
    {
        private readonly IQueueReporterService _reporter;
        private readonly IQueueService _queueService;
        private readonly IAiringService _airingService;

        public EnvelopeDistributor(
            IQueueReporterService queueReporter,
            IQueueService queueService,
            IAiringService airingService)
        {
            _reporter = queueReporter;
            _queueService = queueService;
            _airingService = airingService;
        }

        public void Distribute(IList<Envelope> envelopes, BLQueue.Queue deliveryQueue, DeliveryDetails details, StringBuilder logger)
        {
            foreach (var envelope in envelopes.OrderBy(e => e.PostMarkedDateTime))
            {
                if (IsAiringDistributed(envelope, deliveryQueue.Name))
                {
                    logger.AppendWithTime(string.Format("Airing {0} already delivered to the queue", envelope.AiringId));
                    continue;
                }

                var message = JsonConvert.SerializeObject(envelope.Message);

                try
                {
                    Deliver(envelope, deliveryQueue.RoutingKey, details);

                    _queueService.AddHistoricalMessage(envelope.AiringId, envelope.MediaId, message, deliveryQueue.Name, envelope.MessagePriority);

                    _reporter.Report(deliveryQueue, envelope.AiringId, envelope.Message.Action != "Delete",
                        string.Format("Sent successfully to {0} Message: {1}", deliveryQueue.FriendlyName, message),
                        12);

                    UpdateDeliveredTo(envelope, deliveryQueue.Name);

                    logger.AppendWithTime(string.Format("Airing {0} successfully delivered to the queue", envelope.AiringId));
                }
                catch
                {
                    _reporter.Report(deliveryQueue, envelope.AiringId, envelope.Message.Action != "Delete",
                        string.Format("Failed to send to {0} Message: {1}", deliveryQueue.FriendlyName, message), 4,
                        true);

                    throw;
                }
            }
        }

        private void UpdateDeliveredTo(Envelope envelope, string queueName)
        {
            switch (envelope.Message.Action)
            {
                case "Create":
                case "Modify":
                    _airingService.PushDeliveredTo(envelope.AiringId, queueName);
                    break;
                case "Delete":
                    _airingService.PushDeliveredTo(envelope.AiringId, queueName, AiringCollection.DeletedCollection);
                    break;
                default:
                    throw new Exception("Unable to determine the airing collection for action: " + envelope.Message.Action);

            }
        }

        private bool IsAiringDistributed(Envelope envelope, string queueName)
        {
            switch (envelope.Message.Action)
            {
                case "Create":
                case "Modify":
                    return _airingService.IsAiringDistributed(envelope.AiringId, queueName);
                case "Delete":
                    return _airingService.IsAiringDistributed(envelope.AiringId, queueName, AiringCollection.DeletedCollection);
                default:
                    throw new Exception("Unable to determine the airing collection for action: " + envelope.Message.Action);

            }
        }

        private void Deliver(Envelope envelope, string routingKey, DeliveryDetails details)
        {
            var json = JsonConvert.SerializeObject(envelope.Message, Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });

            //Need to double serialize due to EasynetQ, and it can be removed on V2.
            json = JsonConvert.SerializeObject(json);

            byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(json);
            IBasicProperties props = details.RabbitMqChannel.CreateBasicProperties();
            if (envelope.MessagePriority != null)
            {
                props.Priority = envelope.MessagePriority.Value;
            }

            details.RabbitMqChannel.BasicPublish(details.ExchangeName, routingKey, false, props, messageBodyBytes);
        }
    }

}