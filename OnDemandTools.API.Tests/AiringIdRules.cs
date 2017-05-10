using Newtonsoft.Json.Linq;
using OnDemandTools.API.Tests.Helpers;
using RestSharp;
using System.Threading.Tasks;
using Xunit;
using System;
using System.Collections.Generic;
using System.Text;

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

        [Fact]
        public void JoesFakeTest()
        {  
            Assert.True(false);          
        }

        [Theory, Order(1)]
        [InlineData("ODTD")]
        public void DeleteAiringId(string prefix)
        {
            JObject response = new JObject();

            var request = new RestRequest(string.Format("/v1/airingid/{0}", prefix), Method.DELETE);

            Task.Run(async () =>
            {
                response = await client.RetrieveRecord(request);
            }).Wait();

            string message = response.Value<String>(@"message");

            Assert.True(message == "deleted successfully");
        }

        [Theory, Order(2)]
        [InlineData("ODTD", 1)]
        public void CreateAiringId(string prefix, int sequenceNumber)
        {
            JObject response = new JObject();

            var request = new RestRequest(string.Format("/v1/airingid/{0}", prefix), Method.POST);

            Task.Run(async () =>
            {
                response = await client.RetrieveRecord(request);
            }).Wait();

            string airingId = response.Value<String>(@"airingId");

            VerifyAiringIdFormat(airingId, prefix, sequenceNumber);
        }

        [Theory, Order(3)]
        [InlineData("ODTD", 2)]
        [InlineData("ODTD", 3)]
        [InlineData("ODTD", 4)]
        public void GenerateAiringId_EnsureAiringIdInProperFormat(string prefix, int sequenceNumber)
        {
            JObject response = new JObject();

            var request = new RestRequest(string.Format("/v1/airingid/generate/{0}", prefix), Method.GET);

            Task.Run(async () =>
            {
                response = await client.RetrieveRecord(request);

            }).Wait();

            string airingId = response.Value<String>(@"airingId");

            int curentbillingNumber = response.Value<int>(@"billingNumberCurrent");

            Assert.True(curentbillingNumber == sequenceNumber,
                string.Format("Current billing number not matched. Expected {0} Returned {1}", sequenceNumber, curentbillingNumber));

            VerifyAiringIdFormat(airingId, prefix, sequenceNumber);
        }

        private void VerifyAiringIdFormat(string airingId, string prefix, int sequenceNumber)
        {
            Assert.True(airingId.StartsWith(prefix),
                string.Format("Returned Airing id: {0}, Airing should starts with {1}", airingId, prefix));

            var dateFormat = DateTime.Now.Date.ToString("MMddyy");

            var builder = new StringBuilder(prefix);

            builder
                .Append("10")
                .Append(dateFormat)
                .Append("000")
                .Append(sequenceNumber.ToString("00000"));

            Assert.True(airingId.Equals(builder.ToString()),
                string.Format("Airing Id not matched. Expected airing Id: {0}, Returned Airing id: {1}", builder.ToString(), airingId));
        }

        [Theory, Order(4)]
        [InlineData("ODTD")]
        public void GenerateAiringId_EnsureDuplicateAiringIdNotGenerated(string prefix)
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
