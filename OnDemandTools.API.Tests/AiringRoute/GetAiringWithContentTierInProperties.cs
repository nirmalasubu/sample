using Newtonsoft.Json.Linq;
using OnDemandTools.API.Tests.AiringRoute.PostAiring;
using OnDemandTools.API.Tests.Helpers;
using RestSharp;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OnDemandTools.API.Tests.AiringRoute
{

    [TestCaseOrderer("OnDemandTools.API.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.API.Tests")]
    [Collection("API Collection")]
    public class GetAiringWithContentTierInProperties : BaseAiringRule
    {

        APITestFixture _fixture;
        RestClient _client;
        private readonly AiringObjectHelper _airingObjectHelper;
        private static string _airingId = "";
        public GetAiringWithContentTierInProperties(APITestFixture fixture) : base("TBSE", "TBSFullAccessApiKey")
        {
            _fixture = fixture;
            _client = _fixture.restClient;
            _airingObjectHelper = new AiringObjectHelper();
        }

        [Fact, Order(1)]
        public void PostAiringWithValidProduct()
        {
            _airingId = PostAiringTest(_airingObjectHelper.UpdateDates(Resources.Resources.TBSAiringWithValidProduct, 0), "Content Tier post test");

            Assert.True(_airingId.StartsWith("TBSE"), "Airing post failed for Content Tier test. Airing Id: " + _airingId);

        }


        /// <summary>
        /// Unit test to verify properties in the destination UTEST and Content Tier UNITTESTCategory
        /// </summary>
        [Fact, Order(2)]
        public void GetAiringHavingDestinationUTESTWithContentTierUNITTESTContentTier()
        {
            JObject response = new JObject();
            var request = new RestRequest("/v1/airing/" + _airingId, Method.GET);
            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);

            }).Wait();

            string value = response.Value<string>(@"StatusCode");
            if (value != null)
            {
                Assert.True(false, "Error in getting airing :" + _airingId);
            }

            JArray flights = response.Value<JArray>(@"flights");
            JArray destinations = flights.First.Value<JArray>(@"destinations");
            JArray properties = destinations.First.Value<JArray>(@"properties");
            bool isContentTierExists = false;
            foreach (var item in properties.Children())
            {
                var itemProperties = item.Children<JProperty>();
                var nameProperty = itemProperties.FirstOrDefault(x => x.Name == "name");
                var valueProperty = itemProperties.FirstOrDefault(x => x.Name == "value");
                if (nameProperty.Value.ToString().Equals("ContentTier") && valueProperty.Value.ToString().Equals("UnitTest"))
                {
                    isContentTierExists = true;
                }

            }

            Assert.True(isContentTierExists, string.Format("Content Tier name 'UnitTest' does not exists for airing Id: {0}", _airingId));
        }
    }
}

