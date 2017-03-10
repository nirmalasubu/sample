using Newtonsoft.Json.Linq;
using OnDemandTools.API.Tests.Helpers;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace OnDemandTools.API.Tests.AiringRoute
{
    [TestCaseOrderer("OnDemandTools.API.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.API.Tests")]
    [Collection("API Collection")]
    public class GetAiringWithOptionStatusRule
    {
        APITestFixture _fixture;
        RestClient _client;
        static string AIRINGID;

        public GetAiringWithOptionStatusRule(APITestFixture fixture)
        {
            _fixture = fixture;
            _client = _fixture.restClient;
        }

        [Fact, Order(1)]
        public void GetAiringWithOptionStatus_PostAiringStatus_Test()
        {
             AIRINGID = PostAiring();
            JObject req = new JObject();
            Dictionary<string, bool> dict = new Dictionary<string, bool>();
            dict["MEDIUM"] = true;
            req["status"] = JObject.FromObject(dict);

            string response=null;
            var request = new RestRequest("/v1/airingstatus/"+ AIRINGID, Method.POST);
            request.AddParameter("application/json", req, ParameterType.RequestBody);

            Task.Run(async () =>
            {
                response = await _client.RetrieveString(request);

            }).Wait();
                Assert.True(response.Contains("Successfully updated the airing status."));
        }

        [Fact, Order(2)]
        public void GetAiringWithOptionStatus_GetAiringWithOptionStatus_Returns_StatusTest()
        {
            JObject response = new JObject();
            var request = new RestRequest("/v1/airing/"+AIRINGID+"?options=status", Method.GET);
            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);

            }).Wait();

            string value = response.Value<string>(@"StatusCode");   // if get operation fails
            if (value != null)
            {
                Assert.True(false, "AiringId : "+AIRINGID+" not exists");
            }
            var statusValue = response[@"options"]["status"];

            Assert.True(statusValue.Value<bool>(@"MEDIUM"));
        }

        [Fact, Order(3)]
        public void GetAiringWithOptionStatus_GetAiringWithoutOptionsStatus_Returns_NoStatusTest()
        {
            JObject response = new JObject();
            var request = new RestRequest("/v1/airing/"+AIRINGID+"?options=file", Method.GET);
            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);

            }).Wait();

            string value = response.Value<string>(@"StatusCode"); // if get operation fails
            if (value != null)
            {
                Assert.True(false, "AiringId : " + AIRINGID + " not exists");
            }
            var statusToken = response[@"options"]["status"];

            Assert.Null(statusToken.First);

          
        }

        [Fact, Order(4)]
        public void GetAiringWithOptionStatus_DeallocateStaticVariable()
        {
            Dispose();  // Deallocate the static variable at the end of the test method.
        }

        #region Private Mathods 

        private string PostAiring()
        {
            JObject airingJson = JObject.Parse(Resources.Resources.ResourceManager.GetString("TBSAiringWithSingleFlight"));
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
                response = await _client.RetrieveRecord(request);

            }).Wait();

            return response.SelectToken("airingId").Value<string>();
        }
        #endregion

        private void Dispose()
        {
            AIRINGID = null;
        }
    }
}
