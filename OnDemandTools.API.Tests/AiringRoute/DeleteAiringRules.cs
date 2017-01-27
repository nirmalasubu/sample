using Newtonsoft.Json.Linq;
using OnDemandTools.API.Tests.Helpers;
using RestSharp;
using System.Threading.Tasks;
using Xunit;

/// <summary>
/// Hint : Covered all the  negative secenerios of the delete airing.
/// Postive scenerios of delete airing is covered under each post Airing rule "Delete{{Brand}}AiringTest" Methods
/// </summary>
namespace OnDemandTools.API.Tests
{
    
    [TestCaseOrderer("OnDemandTools.API.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.API.Tests")]   
    [Collection("API Collection")]
    public class DeleteAiringRules
    {
        APITestFixture fixture;
        RestClient _client;
        public DeleteAiringRules(APITestFixture fixture)
        {
            this.fixture = fixture;
           _client = this.fixture.restClient;
        }

        [Fact]
        public void DeleteNonExistantAiringTest()
        {
           
            JObject response = new JObject();
            var request = new RestRequest("/v1/airing", Method.DELETE);

            request.AddJsonBody(new
            {
                AiringId = "ABCD1234",
                ReleasedBy = "ntuser"
            });
            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);

            }).Wait();

            Assert.True((response.GetValue("StatusCode").ToString() != "OK"));
        }

        [Fact]
        public void DeletAiringwithNoReleasedBy_parameterTest()
        {

            JObject response = new JObject();
            var request = new RestRequest("/v1/airing", Method.DELETE);

            request.AddJsonBody(new
            {
                AiringId = "CARE1007291600012449"
            });
            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);

            }).Wait();

            Assert.True((response.GetValue("StatusCode").ToString() != "OK"));
        }
    }
}
