using Newtonsoft.Json.Linq;
using OnDemandTools.Common.Configuration;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Adapters.Hangfire
{
    public class HangfireRecurringJobCommand : IHangfireRecurringJobCommand
    {
        private readonly RestClient _client;

        public HangfireRecurringJobCommand(AppSettings settings)
        {
            _client = new RestClient(settings.PortalSettings.HangFireUrl);
        }

        public void CreatePublisherJob(string queueName)
        {
            var request = new RestRequest(string.Format("/api/hangfire/{0}", queueName), Method.POST);

            JObject response = new JObject();

            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);
            }).Wait();

            if (response.Value<string>(@"StatusCode") != "OK")
                throw new Exception(string.Format("Failed to create publisher job for queue name {0}", queueName));
        }

        public void DeletePublisherJob(string queueName)
        {
            var request = new RestRequest(string.Format("/api/hangfire/{0}", queueName), Method.DELETE);

            JObject response = new JObject();

            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);
            }).Wait();

            if (response.Value<string>(@"StatusCode") != "OK")
                throw new Exception(string.Format("Failed to delete publisher job for queue name {0}", queueName));
        }
    }
}