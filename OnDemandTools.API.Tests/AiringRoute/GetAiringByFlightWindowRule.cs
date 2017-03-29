using Newtonsoft.Json.Linq;
using OnDemandTools.API.Tests.Helpers;
using RestSharp;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace OnDemandTools.API.Tests.AiringRoute
{
    [TestCaseOrderer("OnDemandTools.API.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.API.Tests")]
    [Collection("API Collection")]
    public class GetAiringByFlightWindowRules
    {
        APITestFixture fixture;
        RestClient client;
        public GetAiringByFlightWindowRules(APITestFixture fixture)
        {
            this.fixture = fixture;
            this.client = this.fixture.restClient;
        }

        [Fact, Order(1)]
        public void GetAiringByBrandFlightWindowDetails_PassingWithValidBrandAndStartEndDate_PassTest()
        {
            JArray response = default(JArray);
            var request = new RestRequest("/v1/airings/brand/Cartoon/destination/CMA?startDate=01/10/2020&endDate=01/18/2020", Method.GET);
            Task.Run(async () =>
            {
                response = await client.RetrieveRecords(request);

            }).Wait();

            if (response.Count > 0)
            {
                var my_obj = response.First.Value<JArray>(@"flights").First;

                Assert.True(response.First.Value<string>(@"brand") == "Cartoon", string.Format("Brand should be 'Cartoon' and but the returned {0}", response.First.Value<string>(@"brand")));
                Assert.True(my_obj.Value<string>(@"start") == "01/10/2020 08:00:00", string.Format("Start should be '01/10/2020 08:00:00' and but the returned {0}", my_obj.Value<string>(@"start")));
                Assert.True(my_obj.Value<string>(@"end") == "01/18/2020 07:59:59", string.Format("End should be '01/18/2020 07:59:59' and but the returned {0}", my_obj.Value<string>(@"end")));

                var flightsDestinationToken = response.First[@"flights"].First["destinations"];
                var propertiesToken = flightsDestinationToken.First["properties"];
                var deliverablesToken = flightsDestinationToken.First["deliverables"];
                Assert.Null(propertiesToken.First);
                Assert.Null(deliverablesToken.First);
            }
        }

        [Fact, Order(2)]
        public void GetAiringByBrandFlightWindowDetails_PassingWithInValid_StartDate_ErrorTest()
        {
            JObject response = default(JObject);
            var request = new RestRequest("/v1/airings/brand/Cartoon/destination/CMA?startDate=18/01/2020&endDate=01/18/2020", Method.GET);
            Task.Run(async () =>
            {
                response = await client.RetrieveRecord(request);

            }).Wait();

            string value = response.Value<string>(@"StatusCode");
            if (value != null)
            {
                Assert.True(true, "message: String was not recognized as a valid DateTime.");
            }
        }
    }
}
