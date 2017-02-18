using Newtonsoft.Json.Linq;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Jobs.Tests.Helpers;
using RestSharp;
using System;
using BLModel = OnDemandTools.Business.Modules.Airing.Model;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace OnDemandTools.Jobs.Tests.Deporter
{
    [TestCaseOrderer("OnDemandTools.Jobs.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.Jobs.Tests")]
    [Collection("Jobs")]
    public class DeportAiringJobRule
    {
        JobTestFixture fixture;
        RestClient _client;
       
        public DeportAiringJobRule(JobTestFixture fixture)
        {
            this.fixture = fixture;
            _client = this.fixture.restClient;
        }

        [Fact, Order(1)]
        public void VerifyDeporterJobTest()
        {
            //Prepare
            IAiringService airingService = fixture.container.GetInstance<IAiringService>();
            IAiringUnitTestService airingUnitTestService= fixture.container.GetInstance<IAiringUnitTestService>();
            JObject airingJson = JObject.Parse(Resources.Resources.ResourceManager.GetString("AiringWithSingleFlight"));
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
                Assert.True(false, "Deporter Airing test Failed : Airirng is  not returned from Expired Collection : " + expiredairingModel.AssetId);
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
