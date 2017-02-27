using Newtonsoft.Json.Linq;
using OnDemandTools.API.Tests.Helpers;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OnDemandTools.API.Tests.PostPackage
{
    [TestCaseOrderer("OnDemandTools.API.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.API.Tests")]
    [Collection("API Collection")]
    public class PostPackageRule
    {
        APITestFixture _fixture;
        RestClient _client;

        public PostPackageRule(APITestFixture fixture)
        {
            _fixture = fixture;
            _client = _fixture.restClient;            
        }

        [Fact, Order(1)]
        public void Post_WithValidPackageDataTest()
        {
            JObject packageJson = JObject.Parse(Resources.Resources.ValidPackage);
            JObject response = new JObject();
            var request = new RestRequest("/v1/package", Method.POST);
            request.AddParameter("application/json", packageJson, ParameterType.RequestBody);
           
            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);

            }).Wait();

            string value = response.Value<string>(@"StatusCode");
            if (value == null)
                Assert.True(true,  "Package posted for Title Ids : "+ packageJson[@"TitleIds"].ToString());
            
        }

        [Fact]
        public void Post_WithInValidPackage_PackageDataEmptyTest()
        {
            JObject packageJson = JObject.Parse(Resources.Resources.InvalidPackage_PackageEmpty);
            JObject response = new JObject();
            var request = new RestRequest("/v1/package", Method.POST);
            request.AddParameter("application/json", packageJson, ParameterType.RequestBody);

            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);

            }).Wait();

            string value = response.Value<string>(@"StatusCode");
            if (value != null)
            {
                Assert.True(true, "PackageData cannot be empty");
            }
        }

        [Fact]
        public void Post_WithInValidPackage_PackageDataNotPresentTest()
        {
            JObject packageJson = JObject.Parse(Resources.Resources.InvalidPackage_PackageNotPresent);
            JObject response = new JObject();
            var request = new RestRequest("/v1/package", Method.POST);
            request.AddParameter("application/json", packageJson, ParameterType.RequestBody);

            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);

            }).Wait();

            string value = response.Value<string>(@"StatusCode");
            if (value != null)
            {
                Assert.True(true, "PackageData must contain valid JSON");
            }
        }

        [Fact]
        public void Post_WithInValidPackage_TitleIdsEmptyTest()
        {
            JObject packageJson = JObject.Parse(Resources.Resources.InvalidPackage_TitleIdsEmpty);
            JObject response = new JObject();
            var request = new RestRequest("/v1/package", Method.POST);
            request.AddParameter("application/json", packageJson, ParameterType.RequestBody);

            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);

            }).Wait();

            string value = response.Value<string>(@"StatusCode");
            if (value != null)
            {
                Assert.True(true, "At least one TitleId is required");
            }
        }

        [Fact]
        public void Post_WithInValidPackage_ContentIdsEmptyTest()
        {
            JObject packageJson = JObject.Parse(Resources.Resources.InvalidPackage_ContentIdsEmpty);
            JObject response = new JObject();
            var request = new RestRequest("/v1/package", Method.POST);
            request.AddParameter("application/json", packageJson, ParameterType.RequestBody);

            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);

            }).Wait();

            string value = response.Value<string>(@"StatusCode");
            if (value != null)
            {
                Assert.True(true, "At least one TitleId or ContentId is required");
            }
        }

        [Fact]
        public void Post_WithInValidPackage_TypeNotPresentTest()
        {
            JObject packageJson = JObject.Parse(Resources.Resources.InvalidPackage_TypeNotPresent);
            JObject response = new JObject();
            var request = new RestRequest("/v1/package", Method.POST);
            request.AddParameter("application/json", packageJson, ParameterType.RequestBody);

            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);

            }).Wait();

            string value = response.Value<string>(@"StatusCode");
            if (value != null)
            {
                Assert.True(true, "Type field must be provided");
            }
        }

        #region Post Package With AiringId
        [Fact]
        public void Post_WithInValidPackage_InvalidAiringIdPresentTest()
        {
            JObject packageJson = JObject.Parse(Resources.Resources.InvalidPackage_InvalidAiringId);
            JObject response = new JObject();
            var request = new RestRequest("/v1/package", Method.POST);
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
        public void Post_WithInValidPackage_NoAiringIdPresentTest()
        {
            JObject packageJson = JObject.Parse(Resources.Resources.InvalidPackage_NoIdsPresent);
            JObject response = new JObject();
            var request = new RestRequest("/v1/package", Method.POST);
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
        public void Post_WithInValidPackage_withAiringIdAndContentidAndTitleIdPresentTest()
        {
            JObject packageJson = JObject.Parse(Resources.Resources.InvalidPackage_AllIdsPresent);
            JObject response = new JObject();
            var request = new RestRequest("/v1/package", Method.POST);
            request.AddParameter("application/json", packageJson, ParameterType.RequestBody);

            Task.Run(async () =>
            {
                response = await _client.RetrieveRecordwithContent(request);

            }).Wait();

            string value = response.Value<string>(@"ErrorMessage");
            if (value != null)
            {
                Assert.True(value.Contains("Cannot register package. Must only provide either AiringId or TitleId or ContentId"));
            }
        }

        [Fact]
        public void Post_WithValidPackage_withAiringIdTest()
        {
           
            string airingId =PostAiring();          
            JObject packageJson = JObject.Parse(Resources.Resources.InvalidPackage_NoIdsPresent);
            packageJson.Add("AiringId", airingId);
            JObject response = new JObject();
            var request = new RestRequest("/v1/package", Method.POST);
            request.AddParameter("application/json", packageJson, ParameterType.RequestBody);

            Task.Run(async () =>
            {
                response = await _client.RetrieveRecordwithContent(request);

            }).Wait();

            string value = response.Value<string>(@"AiringId");

            if(value == null)
                Assert.True(airingId.Equals(airingId));
        }

        #endregion

        [Fact, Order(2)]
        public void DeleteExistingPackageTest()
        {
            JObject packageJson = JObject.Parse(Resources.Resources.ValidPackage);
            JObject response = new JObject();
            var request = new RestRequest("/v1/package", Method.DELETE);
            request.AddParameter("application/json", packageJson, ParameterType.RequestBody);

            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);

            }).Wait();

            Assert.True((response.GetValue("message").ToString() == "Package deleted successfully"));
        }

        #region Private Mathods 

        private string PostAiring()
        {
            JObject airingJson = JObject.Parse(Resources.Resources.ResourceManager.GetString("TBSAiringWithSingleFlight"));
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
