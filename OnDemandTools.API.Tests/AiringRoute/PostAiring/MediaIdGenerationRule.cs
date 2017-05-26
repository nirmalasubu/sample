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
        private static string AIRINGID;
        private  static string MEDIAID;
        
        public MediaIdGenerationRule(APITestFixture fixture)
            : base("CARE", "CartoonFullAccessApiKey")
        {
            _airingObjectHelper = new AiringObjectHelper();
            _fixtureLocal = new APITestFixture("CartoonFullAccessApiKey");
            _clientLocal = this._fixtureLocal.restClient;            
        }

        #region "MediaId Generation test in Post Airing Route"

        [Fact(Skip="Temporarily"), Order(1)]
        public void MediaIdGeneration_PostAiringWithOutVersion_MediaIdNonGenerationTest()
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

        [Fact, Order(1)]
        public void MediaIdGeneration_PostAiringWithVersion_MediaIdGenerationTest()
        {
            //JSON string with version with out AiringId, MediaID
             AIRINGID = PostAiringTest(_airingObjectHelper.UpdateDates(Resources.Resources.CartoonAiringWith3Flights, 0), "Media Id Generation test");
            
            JObject response = new JObject();
            var request = new RestRequest("/v1/airing/" + AIRINGID, Method.GET);
            Task.Run(async () =>
            {
                response = await _clientLocal.RetrieveRecord(request);

            }).Wait();

             MEDIAID = response.Value<string>(@"mediaId");
            // Assert
            Assert.True(!string.IsNullOrEmpty(response.Value<string>(@"mediaId")), string.Format("Media Id {0} is generated", response.Value<string>(@"mediaId")));
        }

        [Fact(Skip="Temporarily"), Order(1)]
        public void MediaIdGeneration_PostAiringWithContentIdsInDifferntOrder_returns_SameMediaIdTest()
        {
            //JSON string with version with out AiringId, MediaID
            string airingId = PostAiringTest(_airingObjectHelper.UpdateDates(Resources.Resources.CartoonAiringWithVersionCIDOrderChanged, 0), "Media Id Generation test");

            JObject response = new JObject();
            var request = new RestRequest("/v1/airing/" + airingId, Method.GET);
            Task.Run(async () =>
            {
                response = await _clientLocal.RetrieveRecord(request);

            }).Wait();

            // Assert
            Assert.Equal(MEDIAID, response.Value<string>(@"mediaId"));
        }

        [Fact(Skip="Temporarily"), Order(2)]
        public void MediaIdGeneration_PostAiringWithdifferentPlaylist_returns_NewMediaIdTest()
        {
            //JSON string with version with  AiringId
            string updatedairing = _airingObjectHelper.UpdateAiringId(AIRINGID, Resources.Resources.CartoonAiringWith3FlightsWithDifferentPlaylist);
            string airingId = PostAiringTest(_airingObjectHelper.UpdateDates(updatedairing, 0), "Media Id Generation test");

            JObject response = new JObject();
            var request = new RestRequest("/v1/airing/" + airingId, Method.GET);
            Task.Run(async () =>
            {
                response = await _clientLocal.RetrieveRecord(request);

            }).Wait();

          
            // Assert
            Assert.NotEqual(MEDIAID, response.Value<string>(@"mediaId"));

            MEDIAID = response.Value<string>(@"mediaId");  // allocate new MediaId value to Old one
        }

        [Fact, Order(3)]
        public void MediaIdGeneration_PostAiringWithSamePlaylist_returns_SameMediaIdTest()
        {
            //JSON string with version with out AiringId, MediaID
            string updatedairing = _airingObjectHelper.UpdateAiringId(AIRINGID, Resources.Resources.CartoonAiringWith3FlightsWithDifferentPlaylist);
            string airingId = PostAiringTest(_airingObjectHelper.UpdateDates(updatedairing, 0), "Media Id Generation test");

            JObject response = new JObject();
            var request = new RestRequest("/v1/airing/" + airingId, Method.GET);
            Task.Run(async () =>
            {
                response = await _clientLocal.RetrieveRecord(request);

            }).Wait();

            // Assert
            Assert.Equal(MEDIAID, response.Value<string>(@"mediaId"));
        }

        
        #endregion

        #region "MediaId Generation test in Post playlist Route"

        [Fact(Skip="Temporarily"), Order(4)]
        public void MediaIdGeneration_PostPlaylistWithNewPlaylist_returns_NewMediaIdTest()
        {
            //post a playlist 
            JObject playlistJson = JObject.Parse(Resources.Resources.CartoonAirngWithValidPlaylist);
            var request = new RestRequest(string.Format("/v1/playlist/{0}",AIRINGID), Method.POST);
            request.AddParameter("text/json", playlistJson, ParameterType.RequestBody);
            string playlistResponse = null;
            Task.Run(async () =>
            {
                playlistResponse = await _clientLocal.RetrieveString(request);

            }).Wait();
            Assert.True(playlistResponse.Contains("Successfully updated the playlist."));

            // Retrive and verify the  generated media Id
            JObject response = new JObject();
            var airingRequest = new RestRequest("/v1/airing/" + AIRINGID, Method.GET);
            Task.Run(async () =>
            {
                response = await _clientLocal.RetrieveRecord(airingRequest);

            }).Wait();

           
            // Assert
            Assert.NotEqual(MEDIAID, response.Value<string>(@"mediaId"));

            MEDIAID = response.Value<string>(@"mediaId");  // allocate new MediaId value to Old one. 
        }

        [Fact, Order(5)]
        public void MediaIdGeneration_PostPlaylistWithSamePlaylist_returns_SameMediaIdTest()
        {
           // post a playlist
            JObject playlistJson = JObject.Parse(Resources.Resources.CartoonAirngWithValidPlaylist);
            var request = new RestRequest(string.Format("/v1/playlist/{0}", AIRINGID), Method.POST);
            request.AddParameter("text/json", playlistJson, ParameterType.RequestBody);
            string playlistResponse=null;
            Task.Run(async () =>
            {
                playlistResponse = await _clientLocal.RetrieveString(request);

            }).Wait();
            Assert.True(playlistResponse.Contains("Successfully updated the playlist."));

            // Retrive and verify the  generated media Id
            JObject response = new JObject();
            var airingRequest = new RestRequest("/v1/airing/" + AIRINGID, Method.GET);
            Task.Run(async () =>
            {
                response = await _clientLocal.RetrieveRecord(airingRequest);

            }).Wait();

            // Assert
            Assert.Equal(MEDIAID, response.Value<string>(@"mediaId"));
        }

        [Fact(Skip="Temporarily"), Order(6)]
        public void MediaIdGeneration_PostPlaylistWithDifferentPlaylist_returns_NewMediaIdTest()
        {
            // post a playlist
            JObject playlistJson = JObject.Parse(Resources.Resources.CartoonAirngWithValidPlaylistEdited);
            var request = new RestRequest(string.Format("/v1/playlist/{0}", AIRINGID), Method.POST);
            request.AddParameter("text/json", playlistJson, ParameterType.RequestBody);
             string playlistResponse = null;
            Task.Run(async () =>
            {
                playlistResponse = await _clientLocal.RetrieveString(request);

            }).Wait();

            Assert.True(playlistResponse.Contains("Successfully updated the playlist."));

            // Retrive and verify the  generated media Id
            JObject response = new JObject();
            var airingRequest = new RestRequest("/v1/airing/" + AIRINGID, Method.GET);
            Task.Run(async () =>
            {
                response = await _clientLocal.RetrieveRecord(airingRequest);

            }).Wait();

            // Assert
            Assert.NotEqual(MEDIAID, response.Value<string>(@"mediaId"));
            dispose();
        }

        #endregion

        void dispose()  // deallocate memory of the static variables
        {
            MEDIAID = null;
            AIRINGID = null;
        }
    }

}
