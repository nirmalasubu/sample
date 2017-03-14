using System;
using System.Linq;
using OnDemandTools.Business.Tests.Helpers;
using Xunit;
using Newtonsoft.Json.Linq;
using OnDemandTools.Business.Modules.Airing;


namespace OnDemandTools.Business.Tests.Airing
{
    [TestCaseOrderer("OnDemandTools.Business.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.Business.Tests")]
    [Collection("Business Collection")]
    public class ChangeFinderRules
    {
        BusinessTestFixture fixture;

        public ChangeFinderRules(BusinessTestFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact, Order(1)]
        public void GetAiringwithOptionChange_InitialReleaseTest()
        {
            //Prepare
            IAiringService airingService = fixture.container.GetInstance<IAiringService>();
            var airingPost = JObject.Parse(Resources.AiringBusinessResource.ResourceManager.GetString("TBSEHistoryInitial")).ToObject<Modules.Airing.Model.Airing>();
            var savedAiringObj = airingService.Save(airingPost, false, true);

            JObject jsonObject = JObject.Parse(Resources.AiringBusinessResource.ResourceManager.GetString("TBSEAiring"));
            jsonObject.Add("AiringId", savedAiringObj.AssetId);
            var businessAiring = jsonObject.ToObject<Modules.Airing.Model.Alternate.Long.Airing>();

            //Act
            airingService.AppendChanges(ref businessAiring);

            //Assert
            Assert.True(businessAiring.Options.Changes.Count > 0, string.Format("The value returned for change should be 'New Release' but it is returned as {0}", businessAiring.Options.Changes[0].TheChange));
        }

        [Fact, Order(2)]
        public void GetAiringwithOptionChange_FlightEndDateModifiedTest()
        {
            //Prepare
            IAiringService airingService = fixture.container.GetInstance<IAiringService>();
            var airingPost = JObject.Parse(Resources.AiringBusinessResource.ResourceManager.GetString("TBSEHistoryWithFightDateModified")).ToObject<Modules.Airing.Model.Airing>();
            var savedAiringObj = airingService.Save(airingPost, false, true);

            JObject jsonObject = JObject.Parse(Resources.AiringBusinessResource.ResourceManager.GetString("TBSEAiring"));
            jsonObject.Add("AiringId", savedAiringObj.AssetId);
            var businessAiring = jsonObject.ToObject<Modules.Airing.Model.Alternate.Long.Airing>();

            //Act
            airingService.AppendChanges(ref businessAiring);

            //Assert           
            Assert.True(businessAiring.Options.Changes.Any(e => e.TheChange == "Flights's End"), "Flight End change not found.");
        }


    }
}