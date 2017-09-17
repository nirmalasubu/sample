using Newtonsoft.Json.Linq;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Jobs.Tests.Helpers;
using RestSharp;
using System;
using BLModel = OnDemandTools.Business.Modules.Airing.Model;
using System.Threading.Tasks;
using Xunit;
using OnDemandTools.Business.Modules.Reporting;

namespace OnDemandTools.Jobs.Tests.Deporter
{
    [TestCaseOrderer("OnDemandTools.Jobs.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.Jobs.Tests")]
    [Collection("Jobs")]
    public class DeportAiringJobRule
    {
        readonly JobTestFixture fixture;
        readonly RestClient _client;

        public DeportAiringJobRule(JobTestFixture fixture)
        {
            this.fixture = fixture;
            _client = this.fixture.RestClient;

        }

        [Fact (Skip="MediaID"), Order(1)]
        public void VerifyDeporterJobTest()
        {
            //Prepare
            IAiringService airingService = fixture.Container.GetInstance<IAiringService>();
            IAiringUnitTestService airingUnitTestService = fixture.Container.GetInstance<IAiringUnitTestService>();
            var dfStatusService = fixture.Container.GetInstance<IDfStatusService>();
            JObject airingJson = JObject.Parse(Resources.Resources.ResourceManager.GetString("TBSAiringWithSingleFlight"));
            JObject response = new JObject();
            var request = new RestRequest("/v1/airing/TBSE", Method.POST);
            request.AddParameter("application/json", UpdateAiringDates(airingJson), ParameterType.RequestBody);

            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);

            }).Wait();
            string airingId = response.Value<string>(@"airingId");
            airingUnitTestService.UpdateAiringRelasedDateAndFlightEndDate(airingId, DateTime.UtcNow.AddDays(-3));

            //Act 
            airingService.Deport(int.Parse(fixture.Configuration["AiringDeportGraceDays"]));

            //Assert
            BLModel.Airing expiredairingModel = airingService.GetBy(airingId, AiringCollection.ExpiredCollection);

            if (expiredairingModel == null)
            {
                Assert.True(false, "Deporter Airing test Failed : Airing is  not returned from Expired Collection : " + expiredairingModel.AssetId);
            }

            if (dfStatusService.HasMessages(airingId, true))
            {
                Assert.True(false, string.Format("Deporter Airing test Failed : Airing {0} DF messages not moved to Expired Collection.", expiredairingModel.AssetId));
            }
        }

        private JObject UpdateAiringDates(JObject jObject)
        {

            JArray jArray = (JArray)jObject.SelectToken("Flights");

            foreach (JObject obj in jArray)
            {
                obj["Start"] = DateTime.UtcNow.AddDays(-2);
                obj["End"] = DateTime.Now.AddDays(2);
            }

            jObject["ReleasedOn"] = DateTime.UtcNow.AddDays(-int.Parse(fixture.Configuration["airingDeportGraceDays"]));
            return jObject;

        }
    }
}
