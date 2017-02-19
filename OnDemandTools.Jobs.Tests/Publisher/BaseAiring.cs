﻿using Newtonsoft.Json.Linq;
using OnDemandTools.Jobs.Tests.Helpers;
using RestSharp;
using System.Threading.Tasks;
using Xunit;

namespace OnDemandTools.Jobs.Tests.Publisher
{
    public abstract class BaseAiring
    {
        private readonly string _abbreviation;
        private readonly JobTestFixture _fixture;
        private readonly RestClient _client;

        protected BaseAiring(string abbreviation, string brandApiKey, JobTestFixture fixture)
        {
            _abbreviation = abbreviation;
            _fixture = fixture;
            _client = _fixture.restClient;
        }

        protected string PostAiringTest(string airingJson, string TestCaseText)
        {

            JObject response = new JObject();
            var request = new RestRequest("/v1/airing/" + _abbreviation, Method.POST);
            request.AddParameter("text/xml", airingJson, ParameterType.RequestBody);

            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);

            }).Wait();

            string value = response.Value<string>(@"StatusCode");
            if (value != null)
            {
                Assert.True(false, "Test method Failed for Brand : " + _abbreviation + ", Method Name :" + TestCaseText);
            }

            return response[@"airingId"].ToString();
        }

        protected string DeleteAiringRequest(string airingID, string TestCaseText)
        {
            JObject response = new JObject();
            var request = new RestRequest("/v1/airing", Method.DELETE);

            request.AddJsonBody(new
            {
                AiringId = airingID,
                ReleasedBy = "UnitTestApp"
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