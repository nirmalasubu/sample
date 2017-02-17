using Newtonsoft.Json.Linq;
using OnDemandTools.Jobs.Tests.Helpers;
using RestSharp;

using System.Threading.Tasks;
using Xunit;

namespace OnDemandTools.Jobs.Tests.AiringRoute.PostAiring
{
    public class BaseAiringRule
    {
        private readonly string _abbreviation;
        JobTestFixture _fixture;
        RestClient _client;

        protected BaseAiringRule(string abbreviation,string  brandApiKey)
        {
            _abbreviation = abbreviation;
            _fixture = new JobTestFixture(brandApiKey);
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
