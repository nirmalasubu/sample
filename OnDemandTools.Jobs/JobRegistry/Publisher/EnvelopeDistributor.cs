using EasyNetQ;
using Newtonsoft.Json;
using OnDemandTools.Business.Modules.Queue.Model;
using OnDemandTools.DAL.Modules.QueueMessages.Model;
using OnDemandTools.Jobs.JobRegistry.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnDemandTools.Jobs.JobRegistry.Publisher
{
    public class EnvelopeDistributor : IEnvelopeDistributor
    {
        private readonly IQueueReporter _reporter;
        private readonly IEnvelopeDistributorQueueHistoryRecorder _historyRecorder;

        //TODO 384 Pass the string builder and add the log
        //private static Logger pbLogger = LogManager.GetLogger("Publisher");
        public EnvelopeDistributor(IQueueReporter queueReporter, IEnvelopeDistributorQueueHistoryRecorder queueHistoryRecorder)
        {
            _reporter = queueReporter;
            _historyRecorder = queueHistoryRecorder;
        }

        public void Distribute(IList<Envelope> envelopes, Queue deliveryQueue, DeliveryDetails details)
        {
            foreach (var envelope in envelopes.OrderBy(e => e.PostMarkedDateTime))
            {
                var message = JsonConvert.SerializeObject(envelope.Message);

                try
                {
                    Deliver(envelope, deliveryQueue.RoutingKey, details);

                    var historicalMessage = new HistoricalMessage(envelope.AiringId, envelope.MediaId, message, deliveryQueue.Name, envelope.MessagePriority);

                    _historyRecorder.Record(historicalMessage);

                    _reporter.Report(deliveryQueue, envelope.AiringId,
                        string.Format("Sent successfully to {0} Message: {1}", deliveryQueue.FriendlyName, message),
                        12);
                }
                catch (Exception exception)
                {
                    //pbLogger.Trace(String.Format("Agent:{0}-Job:{1}-Thread:{2}-Queue:{3}: Failed to deliver message:{4}", details.Agent.AgentId, details.Job.JobName, Thread.CurrentThread.ManagedThreadId, deliveryQueue.FriendlyName, message));
                    //pbLogger.Error(String.Format("Agent:{0}-Job:{1}-Thread:{2}-Queue:{3}: Failed to deliver message:{4}", details.Agent.AgentId, details.Job.JobName, Thread.CurrentThread.ManagedThreadId, deliveryQueue.FriendlyName, message));

                    _reporter.Report(deliveryQueue, envelope.AiringId,
                        string.Format("Failed to send to {0} Message: {1}", deliveryQueue.FriendlyName, message), 4,
                        true);

                    throw;
                }
            }
        }

        private void Deliver(Envelope envelope, string routingKey, DeliveryDetails details)
        {
            var json = JsonConvert.SerializeObject(envelope.Message);
            var message = new Message<String>(json);
            if (envelope.MessagePriority != null)
            {
                message.Properties.Priority = envelope.MessagePriority.Value;
            }
            details.Bus.Publish(details.Exchange, routingKey, false, message);
        }
    }

    public interface IEnvelopeDistributorQueueHistoryRecorder
    {
        void Record(HistoricalMessage record);
    }

    public interface IQueueReporter
    {
        void Report(Queue queue, string airingId, string message, int statusEnum, bool unique = false);

        void BimReport(Queue queue, string airingId, string message, int statusEnum);

    }

}