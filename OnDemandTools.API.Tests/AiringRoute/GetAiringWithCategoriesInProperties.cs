using Newtonsoft.Json.Linq;
using OnDemandTools.API.Tests.Helpers;
using RestSharp;
using System;
using System.Threading.Tasks;
using Xunit;

namespace OnDemandTools.API.Tests.AiringRoute
{
  
    [TestCaseOrderer("OnDemandTools.API.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.API.Tests")]
    [Collection("API Collection")]
    public class GetAiringWithCategoriesInProperties
    {

        APITestFixture _fixture;
        RestClient _client;

        public GetAiringWithCategoriesInProperties(APITestFixture fixture)
        {
            _fixture = fixture;
            _client = _fixture.restClient;
        }


        /// <summary>
        /// Unit test to verify properties in the destination UTEST and category UNITTESTCategory
        /// </summary>
        [Fact, Order(1)]
        public void GetAiringHavingDestinationUTESTWithCategoryUNITTESTCategory()
        {
            string airingId = PostAiring("TBSAiring_UTEST_UNITTESTCategory");
            JObject response = new JObject();
            var request = new RestRequest("/v1/airing/" + airingId, Method.GET);
            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);

            }).Wait();

            string value = response.Value<string>(@"StatusCode");
            if (value != null)
            {
                Assert.True(false, "Error in getting airing :" + airingId);
            }

            JArray flights = response.Value<JArray>(@"flights");
            JArray destinations = flights.First.Value <JArray>(@"destinations");
            JArray properties = destinations.First.Value<JArray>(@"properties");

            Assert.True(properties != null, string.Format("Either destination doesn't exists  or Properties are null for the destination. Add a property or category for the destination"));
        }
        


        #region Private Mathods 

        private string PostAiring(string resourceString)
        {
            JObject airingJson = JObject.Parse(Resources.Resources.ResourceManager.GetString(resourceString));
            JArray jArray = (JArray)airingJson.SelectToken("Flights");

            foreach (JObject obj in jArray)
            {
                obj["Start"] = DateTime.UtcNow.AddDays(2);
                obj["End"] = DateTime.Now.AddDays(3);
            }
            JObject response = new JObject();
            var request = new RestRequest("/v1/airing/TBSE", Method.POST);
            request.AddParameter("application/json", airingJson, ParameterType.RequestBody);

            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);

            }).Wait();

            return response.SelectToken("airingId").Value<string>();
        }
        #endregion
    }
}

