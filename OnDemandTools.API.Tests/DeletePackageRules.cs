using OnDemandTools.API.Tests.Helpers;
using RestSharp;
using System;
using Xunit;
using System.Threading.Tasks;

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

        [Fact, Order(1)]
        public void PassingTest()
        {

            Console.WriteLine(client.BaseUrl);
            String response = String.Empty;
            var request = new RestRequest("/healthcheck", Method.GET);
            Task.Run(async () =>
                {
                    response = await client.SubmitRequest(request) as String;

                }).Wait(); 
            
            Console.WriteLine(response);
            Assert.True(!String.IsNullOrEmpty(response));
        }

    }
}
