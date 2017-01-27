using Newtonsoft.Json.Linq;
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
    public class GetAiringByDestinationRules
    {
        APITestFixture fixture;
        RestClient client;
        public GetAiringByDestinationRules(APITestFixture fixture)
        {
            this.fixture = fixture;
            this.client = this.fixture.restClient;
        }

        [Fact, Order(1)]
        public void GetAiringByDestination_PassingWithInValidDestinationTest()
        {
            JArray response = default(JArray);
            var request = new RestRequest("/v1/airings/destination/CMA", Method.GET);
            Task.Run(async () =>
            {
                response = await client.RetrieveRecords(request);

            }).Wait();

            if (response.Count > 0)
            {
                var my_obj = response.First.Value<JArray>(@"flights").First.Value<JArray>(@"destinations").ToList();
                var message = my_obj.Any(i => i.Value<String>(@"name") == "CMA") ? "Test Passed" : "User do not have access to destination";
                Assert.True(!my_obj.Any(i => i.Value<String>(@"name") == "CMA"), message);
            }
        }

        [Fact, Order(2)]
        public void GetAiring_WithValidDestinationTest()
        {
            JArray response = default(JArray);
            var request = new RestRequest("/v1/airings/destination/CNWB", Method.GET);
            Task.Run(async () =>
            {
                response = await client.RetrieveRecords(request);

            }).Wait();

            if (response.Count > 0)
            {
                var my_obj = response.First.Value<JArray>(@"flights").First.Value<JArray>(@"destinations").ToList();
                var message = my_obj.Any(i => i.Value<String>(@"name") == "CMA") ? "Test Passed" : "User do not have access to destination";
                Assert.True(my_obj.Any(i => i.Value<String>(@"name") == "CMA"), message);
            }
        }
    }
}
