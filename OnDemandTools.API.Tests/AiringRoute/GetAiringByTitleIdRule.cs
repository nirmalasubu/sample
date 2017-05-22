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
            bool postairing = PostAiring();
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

        #region Private Mathods 

        private bool PostAiring()
        {
            JObject airingJson = JObject.Parse(Resources.Resources.ResourceManager.GetString("TBSAiringwithSeriesIdandTitleId"));
            JArray jArray = (JArray)airingJson.SelectToken("Flights");

            foreach (JObject obj in jArray)
            {
                obj["Start"] = DateTime.UtcNow.AddDays(2);
                obj["End"] = DateTime.Now.AddDays(3);
            }
            JObject response = new JObject();
            var request = new RestRequest("/v1/airing/TBSE", Method.POST);
            request.AddParameter("application/json", airingJson, ParameterType.RequestBody);

            Task.Run(async () =>
            {
                response = await client.RetrieveRecord(request);

            }).Wait();

            return response.SelectToken("airingId").Value<string>() != "" ? true : false;
        }
        #endregion
    }

}
