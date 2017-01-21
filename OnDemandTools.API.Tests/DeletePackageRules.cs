using OnDemandTools.API.Tests.Helpers;
using RestSharp;
using System;
using Xunit;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace OnDemandTools.API.Tests
{
    [TestCaseOrderer("OnDemandTools.API.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.API.Tests")]
    [Collection("API Collection")]
    public class DeletePackageRules
    {
        APITestFixture fixture;
        RestClient client;
        public DeletePackageRules(APITestFixture fixture)
        {
            this.fixture = fixture;
            this.client = this.fixture.restClient;
        }


        // Some example code for you to get started, not necessarily actual code
        [Fact, Order(1)]
        public void PassingTest()
        {

            Console.WriteLine(client.BaseUrl);
            JObject response = new JObject();
            var request = new RestRequest("/healthcheck", Method.GET);
            Task.Run(async () =>
                {
                    response = await client.RetrieveRecord(request);

                }).Wait();

            Console.WriteLine(response.ToString());
            Assert.True(response.Count > 1);
        }

    }
}
