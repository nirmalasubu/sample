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
    [Collection("Job Collection")]
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
            var airingPost = UpdateAiringDates(JObject.Parse(Resources.Resource.ResourceManager.GetString("AiringWithSingleFlight"))).ToObject<BLModel.Airing>();
            var savedAiringObj = airingService.Save(airingPost, false, true);
            bool IsAiringExistInCurrentCollection = airingService.IsAiringExists(savedAiringObj.AssetId);
            if (!IsAiringExistInCurrentCollection)
            {
                Assert.True(false, "Deporter Airing test Failed : Airing is  not returned from Current Collection : " + savedAiringObj.AssetId);
            }

            //Act 
            Thread.Sleep(20000);
            airingService.Deport(int.Parse(fixture.Configuration["airingDeportGraceDays"]));

            //Assert
            BLModel.Airing expiredairingModel= airingService.GetBy(savedAiringObj.AssetId, AiringCollection.ExpiredCollection);
            if (expiredairingModel == null)
            {
                Assert.True(false, "Deporter Airing test Failed : Airing is  not returned from Expired Collection : " + expiredairingModel.AssetId);
            }
        }
        
        private JObject UpdateAiringDates(JObject jObject)
        {
           
            JArray jArray = (JArray)jObject.SelectToken("Flights");

            foreach (JObject obj in jArray)
            {
                obj["Start"] = DateTime.UtcNow.AddDays(-2);
                obj["End"] = DateTime.UtcNow.AddSeconds(30);                
            }

            jObject["ReleasedOn"] = DateTime.UtcNow.AddDays(-int.Parse(fixture.Configuration["airingDeportGraceDays"]));
            return jObject;

        }
    }
}
