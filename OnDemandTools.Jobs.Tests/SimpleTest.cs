using Newtonsoft.Json.Linq;
using OnDemandTools.Jobs.Tests.Helpers;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OnDemandTools.Jobs.Tests
{
    [TestCaseOrderer("OnDemandTools.Jobs.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.Jobs.Tests")]
    [Collection("Job Collection")]
    public class SimpleTest
    {
        JobTestFixture fixture;
        RestClient client;
        public SimpleTest(JobTestFixture fixture)
        {
            this.fixture = fixture;
            this.client = this.fixture.restClient;
        }

        [Theory]
        [InlineData("/v1/airingId/generate/TEST", Method.GET)]
        public void SimpleTestCase(string route, Method method)
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
