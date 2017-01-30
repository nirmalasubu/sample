using Newtonsoft.Json.Linq;
using OnDemandTools.API.Tests.Helpers;
using RestSharp;
using System.Threading.Tasks;
using Xunit;
using System;
using System.Collections.Generic;

namespace OnDemandTools.API.Tests
{
    [TestCaseOrderer("OnDemandTools.API.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.API.Tests")]
    [Collection("API Collection")]
    public class AiringIdRules
    {
        APITestFixture fixture;
        RestClient client;
        public AiringIdRules(APITestFixture fixture)
        {
            this.fixture = fixture;
            this.client = this.fixture.restClient;
        }

        [Theory]
        [InlineData("TEST")]
        [InlineData("CARE")]
        public void GetAiringId_EnsureAiringIdInProperFormat(string prefix)
        {
            JObject response = new JObject();

            var request = new RestRequest(string.Format("/v1/airingid/generate/{0}", prefix), Method.GET);

            Task.Run(async () =>
            {
                response = await client.RetrieveRecord(request);

            }).Wait();

            string airingId = response.Value<String>(@"airingId");

            Assert.True(airingId.StartsWith(prefix), string.Format("Returned Airing id: {0}, Airing should starts with {1}", airingId, prefix));

            var dateFormat = DateTime.Now.Date.ToString("MMddyy");

            Assert.True(airingId.StartsWith(string.Format("{0}10{1}", prefix, dateFormat)),
                string.Format("Returned Airing id: {0}, Airing should contain date in format {1}", airingId, dateFormat));

        }

        [Theory]
        [InlineData("TEST")]
        [InlineData("CARE")]
        public void GetAiringId_EnsureDuplicateAiringIdNotGenerated(string prefix)
        {
            var airingIds = new List<string>();
            for (int i = 0; i < 100; i++)
            {
                JObject response = new JObject();

                var request = new RestRequest(string.Format("/v1/airingid/generate/{0}", prefix), Method.GET);

                Task.Run(async () =>
                {
                    response = await client.RetrieveRecord(request);

                }).Wait();

                string airingId = response.Value<String>(@"airingId");

                if (airingIds.Contains(airingId))
                {
                    Assert.True(false, string.Format("Duplicate Airing id: {0} generated", airingId));
                }
                else
                {
                    airingIds.Add(airingId);
                }
            }

            Assert.True(airingIds.Count == 100, "100 Airing Ids not generated");
        }
    }
}
