using Newtonsoft.Json.Linq;
using OnDemandTools.API.Tests.Helpers;
using RestSharp;
using System;
using System.Threading.Tasks;
using Xunit;


namespace OnDemandTools.API.Tests.AiringRoute
{
    [TestCaseOrderer("OnDemandTools.API.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.API.Tests")]
    [Collection("API Collection")]
    public class GetAiringBySeriesIdRule
    {
        APITestFixture fixture;
        RestClient client;
        public GetAiringBySeriesIdRule(APITestFixture fixture)
        {
            this.fixture = fixture;
            this.client = this.fixture.restClient;
        }

        [Fact]
        public void GetAiringBySeriesId_PassingWithValidId()
        {

            JArray response = new JArray();
            var request = new RestRequest("/v1/airings/seriesId/326558", Method.GET);
            Task.Run(async () =>
            {
                response = await client.RetrieveRecords(request);

            }).Wait();

            string value = response.First.Value<String>(@"StatusCode");
            if (value != null)
            {
                Assert.True(false, "SeriesId : 326558 is has no airings");
            }

            JObject jSeries = response.First.SelectToken("title").Value<JObject>(@"series");
            
            Assert.True(jSeries.Value<string>(@"id") == "326558", string.Format("Series Id should be '326558' and but the returned {0}", jSeries.Value<string>(@"id")));
            Assert.True(!string.IsNullOrEmpty(response.First.Value<string>(@"airingId")), string.Format("airing Id should not be null or empty and but the returned Null "));
        }

        [Fact]
        public void GetAiringBySeriesId_PassingWithInValidId()
        {

            JArray response = new JArray();
            var request = new RestRequest("/v1/airings/seriesId/1111", Method.GET);
            Task.Run(async () =>
            {
                response = await client.RetrieveRecords(request);

            }).Wait();

            Assert.True((response.First == null), "Series Id - 1111 is has no airings");

        }
    }
}
