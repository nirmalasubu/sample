using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Jobs.Tests.Helpers;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OnDemandTools.Jobs.Tests.Publisher
{
    /// <summary>
    /// Hint : order 1 runs first and  then order 2 runs
    /// </summary>
    [TestCaseOrderer("OnDemandTools.Jobs.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.Jobs.Tests")]
    [Collection("Jobs")]
    [Order(1)]
    public class TBSPlaylistRule : BaseAiring
    {
        private readonly string _tbsQueueKey;
        private readonly AiringObjectHelper _airingObjectHelper;
        private static QueueTester _queueTester;

        RestClient client;
        private readonly string _jsonString;
        private static List<string> airingIds = new List<string>();

        private static string _airingId;
        JobTestFixture _fixture;

        public TBSPlaylistRule(JobTestFixture fixture)
            : base("TBSE", "TBSFullAccessApiKey", fixture)
        {
            _airingObjectHelper = new AiringObjectHelper();
            _jsonString = _airingObjectHelper.UpdateDates(Resources.Resources.TBSAiringWithSingleFlight, 1);

            _tbsQueueKey = fixture.Configuration["TbsQueueApiKey"];

            _fixture = fixture;

            if (_queueTester == null)
                _queueTester = new QueueTester(fixture);
        }

        [Fact, Order(1)]
        public void Playlist_Post_ValidAiring()
        {
            _airingId = PostAiringTest(_airingObjectHelper.UpdateDates(_jsonString, 1), "Active  Airing test");
            _queueTester.AddAiringToDataStore(_airingId, true, "Playlist Active Airing test", _tbsQueueKey);
        }

        [Fact, Order(2)]
        public void Playlist_WithInvalidAiringId_ExceptionThrown()
        {
            string statusCode = PostPlaylist(Resources.Resources.TbsValidPlaylist, "TBS");

            Assert.True(statusCode != "Ok", "API Returned Ok Status for invalid playlist.");
        }

        [Fact, Order(2)]
        public void Playlist_WithInvalidReleaseBy_ExceptionThrown()
        {
            var playListPayload = Resources.Resources.TbsValidPlaylist;

            playListPayload = playListPayload.Replace("UnitTestApp", string.Empty);

            string statusCode = PostPlaylist(playListPayload, _airingId);

            Assert.True(statusCode != "Ok", "API Returned Ok Status for invalid playlist.");
        }

        [Fact, Order(2)]
        public void Playlist_WithInvalidVersion_ExceptionThrown()
        {
            var playListPayload = Resources.Resources.TbsValidPlaylist;

            playListPayload = playListPayload.Replace("1E66T", "INVALID");

            string statusCode = PostPlaylist(playListPayload, _airingId);

            Assert.True(statusCode != "Ok", "API Returned Ok Status for invalid playlist.");
        }

        [Fact, Order(3)]
        public void VerifyClientDeliveryQueue()
        {
            _queueTester.VerifyClientQueueDelivery();
        }

        [Fact, Order(4)]
        public void Playlist_WithValidPlaylist_PassTest()
        {
            var playListPayload = Resources.Resources.TbsValidPlaylist;

            string statusCode = PostPlaylist(playListPayload, _airingId);

            var airingService = _fixture.container.GetInstance<IAiringService>();

            var airing = airingService.GetBy(_airingId);

            Assert.True(!airing.DeliveredTo.Any(), "Delivered Queue not cleared for successfull playlist post.");

            Assert.True(!airing.IgnoredQueues.Any(), "Ignored Queue not cleared for successfull playlist post.");

        }
    }
}
