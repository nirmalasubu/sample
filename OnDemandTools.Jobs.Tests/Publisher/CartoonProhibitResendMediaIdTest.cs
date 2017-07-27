using Newtonsoft.Json.Linq;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Business.Modules.Queue;
using OnDemandTools.Jobs.Tests.Helpers;
using RestSharp;
using System;

using System.Threading.Tasks;
using Xunit;

namespace OnDemandTools.Jobs.Tests.Publisher
{
    [TestCaseOrderer("OnDemandTools.Jobs.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.Jobs.Tests")]
    [Collection("Jobs")]
    [Order(2)]
    public class CartoonProhibitResendMediaIdTest : BaseAiring
    {
        JobTestFixture fixture;
        RestClient _client;
        private static QueueTester _queueTester;
        private const string MediaId = "de24e1255c4ef791151d2da61bbcf9e352a4c2d1";

        public CartoonProhibitResendMediaIdTest(JobTestFixture fixture)
            : base("TBSE", "", fixture)
        {
            this.fixture = fixture;
            _client = this.fixture.RestClient;

            if (_queueTester == null)
                _queueTester = new QueueTester(fixture);
        }

        [Fact, Order(1)]
        public void DeleteHistoricalMessage()
        {
            var queueService = fixture.Container.GetInstance<IQueueService>();

            queueService.DeleteHistoricalMessage(MediaId);
        }

        [Fact, Order(2)]
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
            _queueTester.AddAiringToDataStore(airingId,
                "ProhibitResendMediaIdToQueue:  prohibit Resend Media ID  to  Queue Initial Test",
                fixture.Configuration["CartoonProhibitResendMediaIdToQueueKey"]);


        }

        [Fact (Skip="MediaID"), Order(3)]
        public void VerifyClientDeliveryQueueForDeliverTest()
        {
            _queueTester.VerifyClientQueueDelivery();
        }

        [Fact, Order(4)]
        public void AiringResendToQueuewithExsistingMediaID()
        {
            IAiringUnitTestService airingUnitTestService = fixture.Container.GetInstance<IAiringUnitTestService>();
            JObject airingJson = JObject.Parse(Resources.Resources.ResourceManager.GetString("CartoonProhibitResendMediaId"));
            JObject response = new JObject();
            var request = new RestRequest("/v1/airing/TBSE", Method.POST);
            request.AddParameter("application/json", UpdateAiringDates(airingJson), ParameterType.RequestBody);

            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);

            }).Wait();
            string airingId = response.Value<string>(@"airingId"); ;
            _queueTester.AddAiringToDataStore(airingId, "ProhibitResendMediaIdToQueue:  prohibit Resend Media ID  to  Queue Repeated Test", "", fixture.Configuration["CartoonProhibitResendMediaIdToQueueKey"]);
        }

        [Fact (Skip="MediaID"), Order(99)]
        public void VerifyClientDeliveryQueueForNotDeliverTest()
        {
            _queueTester.VerifyClientQueueDelivery();
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
