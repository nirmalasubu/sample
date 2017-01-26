using Newtonsoft.Json.Linq;
using OnDemandTools.API.Tests.Helpers;
using RestSharp;
using System.Threading.Tasks;
using Xunit;

namespace OnDemandTools.API.Tests
{
    [TestCaseOrderer("OnDemandTools.API.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.API.Tests")]
    [Collection("API Collection")]
    public class ApiRoutingRules
    {
        APITestFixture fixture;
        RestClient client;
        public ApiRoutingRules(APITestFixture fixture)
        {
            this.fixture = fixture;
            this.client = this.fixture.restClient;
        }

        [Theory]
        [InlineData("/v1/airingId/generate/TEST", Method.GET)]
        [InlineData("/v1/airingId/TEST", Method.POST)]
        [InlineData("/v1/airing/TEST", Method.POST)]
        [InlineData("/v1/airing", Method.DELETE)]
        [InlineData("/v1/airing/AIRINGID", Method.GET)]
        [InlineData("/v1/products", Method.GET)]
        [InlineData("/v1/product/1/destinations", Method.GET)]
        [InlineData("/v1/destinations", Method.GET)]
        [InlineData("/v1/destination/1", Method.GET)]
        [InlineData("/v1/files/title/1", Method.GET)]
        [InlineData("/v1/files/airing/AIRINGID", Method.GET)]
        [InlineData("/v1/files", Method.POST)]
        [InlineData("/v1/package", Method.POST)]
        [InlineData("/v1/package", Method.DELETE)]
        public void VerifyUnAuthenticatedRoutes(string route, Method method)
        {
            JObject response = new JObject();

            var request = new RestRequest(route, method);
            request.AddHeader("Authorization", "SOMEINVALIDID");


            Task.Run(async () =>
            {
                response = await client.RetrieveRecord(request);

            }).Wait();

            Assert.True((response.GetValue("StatusCode").ToString() == "Unauthorized"));
        }
    }
}
