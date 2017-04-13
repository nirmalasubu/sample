﻿using Newtonsoft.Json.Linq;
using OnDemandTools.API.Tests.Helpers;
using RestSharp;
using System;
using System.Threading.Tasks;
using Xunit;


namespace OnDemandTools.API.Tests.AiringRoute
{
    [TestCaseOrderer("OnDemandTools.API.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.API.Tests")]
    [Collection("API Collection")]
    public class GetAiringByTitleIdRule
    {
        APITestFixture fixture;
        RestClient client;
        public GetAiringByTitleIdRule(APITestFixture fixture)
        {
            this.fixture = fixture;
            this.client = this.fixture.restClient;
        }

        [Fact]
        public void GetAiringByTitleId_PassingWithValidId()
        {

            JArray response = new JArray();
            var request = new RestRequest("/v1/airings/titleId/2065580", Method.GET);
            Task.Run(async () =>
            {
                response = await client.RetrieveRecords(request);

            }).Wait();

            string value = response.First.Value<String>(@"StatusCode");
            if (value != null)
            {
                Assert.True(false, "TitleId : 2065580 is has no airings");
            }

            JArray jTitleIds = response.First.SelectToken("title").Value<JArray>(@"titleIds");
            // Assert
            Assert.True(jTitleIds.First.Value<string>(@"value") == "2065580", string.Format("Title Id should be '2065580' and but the returned {0}", jTitleIds.First.Value<string>(@"value")));
            Assert.True(response.First.Value<string>(@"mediaId") == "94ca5f6ac5222ef35a24ed05df0427ee33b32362", string.Format("Media Id should be '94ca5f6ac5222ef35a24ed05df0427ee33b32362' and but the returned {0}", response.First.Value<string>(@"mediaId")));
        }

        [Fact]
        public void GetAiringByTitleId_PassingWithInValidId()
        {
            JArray response = new JArray();
            var request = new RestRequest("/v1/airings/titleId/1111", Method.GET);
            Task.Run(async () =>
            {
                response = await client.RetrieveRecords(request);

            }).Wait();

            Assert.True((response.First == null), "1111 is has no airings");
        }
    }
}
