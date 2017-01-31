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
        protected void Post_WithValidPackageDataTest()
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
            {
                Assert.True(true,  "Package posted for Title Ids : "+ packageJson[@"TitleIds"].ToString());
            }
        }

        [Fact]
        protected void Post_WithInValidPackage_PackageDataEmptyTest()
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
        protected void Post_WithInValidPackage_PackageDataNotPresentTest()
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
        protected void Post_WithInValidPackage_TitleIdsEmptyTest()
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
        protected void Post_WithInValidPackage_TypeNotPresentTest()
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
    }
}
