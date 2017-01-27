using Newtonsoft.Json.Linq;
using OnDemandTools.API.Tests.Helpers;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OnDemandTools.API.Tests.AiringRoute.PostAiring
{
    public class BaseAiringRule
    {
        private readonly string _abbreviation;
        APITestFixture _fixture;
        RestClient _client;

        protected BaseAiringRule(string abbreviation,string  brandApiKey)
        {
            _abbreviation = abbreviation;
            _fixture = new APITestFixture(brandApiKey);
            _client = _fixture.restClient;
        }

        protected string PostAiringTest(string airingJson,string TestCaseText)
        {

            JObject response = new JObject();
            var request = new RestRequest("/v1/airing/"+ _abbreviation, Method.POST);
            request.AddParameter("text/xml", airingJson, ParameterType.RequestBody);
           
            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);

            }).Wait();

            string value = response.Value<string>(@"StatusCode");
            if (value != null)
            {
                Assert.True(false,  "Test method Failed for Brand : " +_abbreviation + ", Method Name :"+ TestCaseText);
            }
           
            return response[@"airingId"].ToString();
        }

        protected string DeleteAiringRequest(string airingID, string TestCaseText)
        {
            JObject response = new JObject();
            var request = new RestRequest("/v1/airing", Method.DELETE);
          
             request.AddJsonBody(new
             {
                 AiringId =airingID,
                 ReleasedBy= "UnitTestApp"
             });
            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);

            }).Wait();

            string value = response.Value<string>(@"StatusCode");
            if (value != null)
            {
                Assert.True(false, "failure in Delete airing");
            }
            return response[@"airingId"].ToString();
        }


    }
}
