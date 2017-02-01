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
            // TODO - note, I have made the following recommendation, it is not complete yet. You will have to verify and made necessary changes.
            var airing = JObject.Parse(Resources.AiringBusinessResource.ResourceManager.GetString("TBSEAiring")).ToObject<Modules.Airing.Model.Alternate.Long.Airing>();
            IAiringService airingService = fixture.container.GetInstance<IAiringService>();
        
            //airingService.AppendChanges(ref airing);

            ////Assert
            //Assert.True(airing.Options.Changes[0].TheChange == "New Release", string.Format("The value returned for change should be 'New Release' but it is returned as {0}", airing.Options.Changes[0].TheChange));
        }

        [Fact, Order(2)]
        public void GetAiringwithOptionChange_ChangeTest()
        {

            // Arrange
            // TODO - note, I have made the following recommendation, it is not complete yet. You will have to verify and made necessary changes.
            var airing = JObject.Parse(Resources.AiringBusinessResource.ResourceManager.GetString("TBSEAiring")).ToObject<Modules.Airing.Model.Alternate.Long.Airing>();
            IAiringService airingService = fixture.container.GetInstance<IAiringService>();

            //airingService.AppendChanges(ref airing);


            //Assert
            //Assert.True(airing.Options.Changes[0].TheChange == "Flights's Start", string.Format("The value returned for change should be 'Flights's Start' but it is returned as {0}", airing.Options.Changes[0].TheChange));
            //Assert.True(airing.Options.Changes[1].TheChange == "Flights's End", string.Format("The value returned for change should be 'Flights's End' but it is returned as {0}", airing.Options.Changes[1].TheChange));
        }

       
    }
}
