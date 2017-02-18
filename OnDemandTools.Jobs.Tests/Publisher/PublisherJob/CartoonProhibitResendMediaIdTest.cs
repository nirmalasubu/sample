using Newtonsoft.Json.Linq;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Jobs.Tests.Helpers;
using RestSharp;
using System;

using System.Threading.Tasks;
using Xunit;

namespace OnDemandTools.Jobs.Tests.Publisher.PublisherJob
{
    [TestCaseOrderer("OnDemandTools.Jobs.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.Jobs.Tests")]
    [Collection("Jobs")]
    [Order(2)]
    public class CartoonProhibitResendMediaIdTest
    {
        JobTestFixture fixture;
        RestClient _client;

        public CartoonProhibitResendMediaIdTest(JobTestFixture fixture)
        {
            this.fixture = fixture;
            _client = this.fixture.restClient;
        }

        [Fact, Order(1)]
        public void AiringSendtoQueueWithNewMediaID()
        {
            JObject airingJson = JObject.Parse(Resources.Resources.ResourceManager.GetString("CartoonProhibitResendMediaId"));
            JObject response = new JObject();
            var request = new RestRequest("/v1/airing/TBSE", Method.POST);
            request.AddParameter("application/json", UpdateAiringDates(airingJson), ParameterType.RequestBody);

            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);

            }).Wait();
            string airingId = response.Value<string>(@"airingId"); ;
            AiringDataStore.AddAiring(airingId,
                "ProhibitResendMediaIdToQueue:  prohibit Resend Media ID  to  Queue Initial Test",
                fixture.Configuration["CartoonProhibitResendMediaIdToQueueKey"]);


        }

        [Fact, Order(2)]
        public void AiringResendToQueuewithExsistingMediaID()
        {
            IAiringUnitTestService airingUnitTestService = fixture.container.GetInstance<IAiringUnitTestService>();
            JObject airingJson = JObject.Parse(Resources.Resources.ResourceManager.GetString("CartoonProhibitResendMediaId"));
            JObject response = new JObject();
            var request = new RestRequest("/v1/airing/TBSE", Method.POST);
            request.AddParameter("application/json", UpdateAiringDates(airingJson), ParameterType.RequestBody);

            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);

            }).Wait();
            string airingId = response.Value<string>(@"airingId"); ;
            AiringDataStore.AddAiring(airingId, "ProhibitResendMediaIdToQueue:  prohibit Resend Media ID  to  Queue Repeated Test", "", fixture.Configuration["CartoonProhibitResendMediaIdToQueueKey"]);
        }


        private JObject UpdateAiringDates(JObject jObject)
        {

            JArray jArray = (JArray)jObject.SelectToken("Flights");

            foreach (JObject obj in jArray)
            {
                obj["Start"] = DateTime.UtcNow.AddDays(-2);
                obj["End"] = DateTime.Now.AddDays(2);
            }

            return jObject;

        }
    }
}
