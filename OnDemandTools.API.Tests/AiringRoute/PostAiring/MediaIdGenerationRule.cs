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
    /// <summary>
    /// Hint : order 1 runs first and  then order 2 runs
    /// </summary>
    [TestCaseOrderer("OnDemandTools.API.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.API.Tests")]
    [Collection("API Collection")]
    public class MediaIdGenerationRule : BaseAiringRule
    {
        private readonly AiringObjectHelper _airingObjectHelper;
        APITestFixture _fixtureLocal ;
        RestClient _clientLocal;
        public MediaIdGenerationRule(APITestFixture fixture)
            : base("CARE", "CartoonFullAccessApiKey")
        {
            _airingObjectHelper = new AiringObjectHelper();
            _fixtureLocal = fixture;
            _clientLocal = this._fixtureLocal.restClient;            
        }

        [Fact, Order(1)]
        public void PostAiring_WithVersion_MediaIdGenerationTest()
        {
            //JSON string with version with out AiringId, MediaID
            string airingId = PostAiringTest(_airingObjectHelper.UpdateDates(Resources.Resources.CartoonAiringWith3Flights, 0), "Media Id Generation test");
            
            JObject response = new JObject();
            var request = new RestRequest("/v1/airing/" + airingId, Method.GET);
            Task.Run(async () =>
            {
                response = await _clientLocal.RetrieveRecord(request);

            }).Wait();

            // Assert
            Assert.True(!string.IsNullOrEmpty(response.Value<string>(@"mediaId")), string.Format("Media Id {0} is generated", response.Value<string>(@"mediaId")));
        }

        [Fact, Order(1)]
        public void PostAiring_WithOutVersion_MediaIdNonGenerationTest()
        {
            //JSON string with out version
            string airingId = PostAiringTest(Resources.Resources.CartoonAiringWithNoVersion, "Media Id Non Generation test");

            JObject Response = new JObject();
            var Request = new RestRequest("/v1/airing/" + airingId, Method.GET);
            Task.Run(async () =>
            {
                Response = await _clientLocal.RetrieveRecord(Request);

            }).Wait();

            // Assert
            Assert.True(string.IsNullOrEmpty(Response.Value<string>(@"mediaId")), string.Format("Media Id is not generated"));
        }

    }

}
