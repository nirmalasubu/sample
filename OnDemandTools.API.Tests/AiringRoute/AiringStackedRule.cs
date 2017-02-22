using Newtonsoft.Json.Linq;
using OnDemandTools.API.Tests.Helpers;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OnDemandTools.API.Tests.AiringRoute
{
    [TestCaseOrderer("OnDemandTools.API.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.API.Tests")]
    [Collection("API Collection")]
    public class AiringStackedRule
    {
        APITestFixture _fixture;
        RestClient _client;
      

        public AiringStackedRule(APITestFixture fixture)
        {
            _fixture = fixture;
            _client = _fixture.restClient;
        }

        [Fact, Order(1)]
        public void AiringWithStackedFalseTest()
        {
            JObject airingJson = JObject.Parse(Resources.Resources.ResourceManager.GetString("TBSAiringStackedFalse"));
            JObject response = new JObject();
            var request = new RestRequest("/v1/airing/TBSE", Method.POST);
            request.AddParameter("application/json", airingJson, ParameterType.RequestBody);

            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);

            }).Wait();
           
                Assert.False(response.SelectToken("flags.stacked").Value<bool>());
           
        }

        [Fact, Order(1)]
        public void AiringWithStackedTrueTest()
        {
            JObject airingJson = JObject.Parse(Resources.Resources.ResourceManager.GetString("TBSAiringStackedTrue"));
            JObject response = new JObject();
            var request = new RestRequest("/v1/airing/TBSE", Method.POST);
            request.AddParameter("application/json", airingJson, ParameterType.RequestBody);

            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);

            }).Wait();

            Assert.True(response.SelectToken("flags.stacked").Value<bool>());

        }
    }
}
