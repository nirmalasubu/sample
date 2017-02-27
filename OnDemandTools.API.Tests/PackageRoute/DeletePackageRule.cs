using Newtonsoft.Json.Linq;
using OnDemandTools.API.Tests.Helpers;
using RestSharp;
using System.Threading.Tasks;
using Xunit;

namespace OnDemandTools.API.Tests.PackageRoute
{
    [TestCaseOrderer("OnDemandTools.API.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.API.Tests")]
    [Collection("API Collection")]
    public class DeletePackageRule
    {
        APITestFixture fixture;
        RestClient _client;
        public DeletePackageRule(APITestFixture fixture)
        {
            this.fixture = fixture;
            _client = this.fixture.restClient;
        }

        [Fact]
        public void DeleteNonExistantPackageTest()
        {
            JObject packageJson = JObject.Parse(Resources.Resources.DeletePackage_InvalidTitle);
            JObject response = new JObject();
            var request = new RestRequest("/v1/package", Method.DELETE);
            request.AddParameter("application/json", packageJson, ParameterType.RequestBody);

            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);

            }).Wait();

            Assert.True((response.GetValue("StatusCode").ToString() != "OK"));
        }

        [Fact]
        public void DeletPackageWithNoType_parameterTest()
        {

            JObject packageJson = JObject.Parse(Resources.Resources.DeletePackage_TypeNotPresent);
            JObject response = new JObject();
            var request = new RestRequest("/v1/package", Method.DELETE);
            request.AddParameter("application/json", packageJson, ParameterType.RequestBody);

            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);

            }).Wait();

            Assert.True((response.GetValue("StatusCode").ToString() != "OK"));
        }

        #region Delete Package With AiringId
        [Fact]
        public void Delete_WithInValidPackage_InvalidAiringIdPresentTest()
        {
            JObject packageJson = JObject.Parse(Resources.Resources.InvalidPackage_InvalidAiringId);
            JObject response = new JObject();
            var request = new RestRequest("/v1/package", Method.DELETE);
            request.AddParameter("application/json", packageJson, ParameterType.RequestBody);

            Task.Run(async () =>
            {
                response = await _client.RetrieveRecordwithContent(request);

            }).Wait();

            string value = response.Value<string>(@"ErrorMessage");
            if (value != null)
            {
                Assert.True(value.Contains("Provided AiringId does not exist"));
            }
        }

        [Fact]
        public void Delete_WithInValidPackage_NoAiringIdPresentTest()
        {
            JObject packageJson = JObject.Parse(Resources.Resources.InvalidPackage_NoIdsPresent);
            JObject response = new JObject();
            var request = new RestRequest("/v1/package", Method.DELETE);
            request.AddParameter("application/json", packageJson, ParameterType.RequestBody);

            Task.Run(async () =>
            {
                response = await _client.RetrieveRecordwithContent(request);

            }).Wait();

            string value = response.Value<string>(@"ErrorMessage");
            if (value != null)
            {
                Assert.True(value.Contains("At least one AiringId or  TitleId or ContentId is required"));
            }
        }

        [Fact]
        public void Delete_WithInValidPackage_withAiringIdAndContentidAndTitleIdPresentTest()
        {
            JObject packageJson = JObject.Parse(Resources.Resources.InvalidPackage_AllIdsPresent);
            JObject response = new JObject();
            var request = new RestRequest("/v1/package", Method.DELETE);
            request.AddParameter("application/json", packageJson, ParameterType.RequestBody);

            Task.Run(async () =>
            {
                response = await _client.RetrieveRecordwithContent(request);

            }).Wait();

            string value = response.Value<string>(@"ErrorMessage");
            if (value != null)
            {
                Assert.True(value.Contains("Cannot delete package. Must only provide either AiringId or TitleId or ContentId"));
            }
        }

        #endregion
    }
}
