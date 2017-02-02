using Newtonsoft.Json.Linq;
using OnDemandTools.API.Tests.AiringRoute.PostAiring;
using OnDemandTools.API.Tests.Helpers;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
namespace OnDemandTools.API.Tests.AiringRoute
{
    
      /// <summary>
      /// Hint : order 1 runs first and  then order 2 runs
      /// </summary>
    [TestCaseOrderer("OnDemandTools.API.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.API.Tests")]
    [Collection("API Collection")]
    public class ProductConvertorRule : BaseAiringRule
    {
        private readonly AiringObjectHelper _airingObjectHelper;
        APITestFixture _fixtureLocal;
        RestClient _clientLocal;
        private static List<string> airingIds = new List<string>();
        public ProductConvertorRule(APITestFixture fixture)
            : base("TBSE", "TBSFullAccessApiKey")
        {
            _airingObjectHelper = new AiringObjectHelper();
            _fixtureLocal = fixture;
            _clientLocal = this._fixtureLocal.restClient;
        }

        [Fact, Order(1)]
        public void PostAiring_WithValidProductToDestinationTest()
        {
            string airingId = PostAiringTest(_airingObjectHelper.UpdateDates(Resources.Resources.ResourceManager.GetString("TBSAiringWithValidProduct"), 0), "valid Product to destination test");
            airingIds.Add(airingId);
            JObject response = new JObject();
            var request = new RestRequest("/v1/airing/" + airingId, Method.GET);
            Task.Run(async () =>
            {
                response = await _clientLocal.RetrieveRecord(request);

            }).Wait();

            // Assert
            Assert.True(string.IsNullOrEmpty(response.Value<string>(@"flights[0].destinations[0].name")), string.Format("Destination {0} is generated", response.Value<string>(@"flights[0].destinations[0].name")));
           
        }

        [Fact, Order(1)]
        public void PostAiring_WithValidProductAndNoDestinationGeneratedTest()
        {
 
            JObject ProductJson = JObject.Parse(Resources.Resources.ResourceManager.GetString("TBSAiringWithValidProductAndNoDestination"));
            var request = new RestRequest("/v1/airing/TBSE", Method.POST);
            request.AddParameter("application/json", ProductJson, ParameterType.RequestBody);
            JObject response = new JObject();
            Task.Run(async () =>
            {
                response = await _clientLocal.RetrieveRecord(request);

            }).Wait();

            // Assert
            Assert.True((response.GetValue("StatusCode").ToString() != "OK"));
        }
        
        [Fact, Order(2)]
        public void DeleteAiring_WithValidProductAndDestinationGeneratedTest()
        {
            foreach (string airingid in airingIds)
            {
                DeleteAiringRequest(airingid, "Delete Airing in product to Destination failed for  :" + airingid);
            }
            Dispose();
        }

        private void Dispose()
        {
            airingIds = null;
        }
    }
}
