﻿using System.Linq;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Jobs.Tests.Helpers;
using Xunit;

namespace OnDemandTools.Jobs.Tests.Publisher
{
    /// <summary>
    ///     Hint : order 1 runs first and  then order 2 runs
    /// </summary>
    [TestCaseOrderer("OnDemandTools.Jobs.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.Jobs.Tests")]
    [Collection("Jobs")]
    [Order(1)]
    public class TbsPlaylistRule : BaseAiring
    {
        public TbsPlaylistRule(JobTestFixture fixture)
            : base("TBSE", "TBSFullAccessApiKey", fixture)
        {
            _airingObjectHelper = new AiringObjectHelper();
            _jsonString = _airingObjectHelper.UpdateDates(Resources.Resources.TBSAiringWithSingleFlight, 1);

            _tbsQueueKey = fixture.Configuration["TbsQueueApiKey"];

            _fixture = fixture;

            if (_queueTester == null)
                _queueTester = new QueueTester(fixture);
        }

        private readonly string _tbsQueueKey;
        private readonly AiringObjectHelper _airingObjectHelper;
        private static QueueTester _queueTester;
        private readonly string _jsonString;
        private static string _airingId;
        private readonly JobTestFixture _fixture;

        [Fact]
        [Order(1)]
        public void Playlist_Asset_ShouldBeCreated()
        {
            _airingId = PostAiringTest(_airingObjectHelper.UpdateDates(_jsonString, 1), "Active  Airing test");
            _queueTester.AddAiringToDataStore(_airingId, true, "Playlist Active Airing test", _tbsQueueKey);

            Assert.True(_airingId.StartsWith("TBSE"), "Airing id not begins with prefix TBSE");
        }

        [Fact]
        [Order(3)]
        public void Playlist_Asset_ShouldDeliverToQueue()
        {
            _queueTester.VerifyClientQueueDelivery();
        }

        [Fact]
        [Order(2)]
        public void Playlist_WithInvalidAiringId_ShouldFail()
        {
            var statusCode = PostPlaylist(Resources.Resources.TbsValidPlaylist, "TBS");

            Assert.True(statusCode != "Ok", "API Returned Ok Status for invalid playlist.");
        }

        [Fact]
        [Order(2)]
        public void Playlist_WithInvalidReleaseBy_ShouldFail()
        {
            var playListPayload = Resources.Resources.TbsValidPlaylist;

            playListPayload = playListPayload.Replace("UnitTestApp", string.Empty);

            var statusCode = PostPlaylist(playListPayload, _airingId);

            Assert.True(statusCode != "Ok", "API Returned Ok Status for invalid playlist.");
        }

        [Fact]
        [Order(2)]
        public void Playlist_WithInvalidVersion_ShouldFail()
        {
            var playListPayload = Resources.Resources.TbsValidPlaylist;

            playListPayload = playListPayload.Replace("1E66T", "INVALID");

            var statusCode = PostPlaylist(playListPayload, _airingId);

            Assert.True(statusCode != "Ok", "API Returned Ok Status for invalid playlist.");
        }

        [Fact]
        [Order(4)]
        public void Playlist_WithValidPost_ShouldPassAndQueueVariableShouldBeEmpty()
        {
            var playListPayload = Resources.Resources.TbsValidPlaylist;

            PostPlaylist(playListPayload, _airingId);

            var airingService = _fixture.Container.GetInstance<IAiringService>();

            var airing = airingService.GetBy(_airingId);

            Assert.True(!airing.DeliveredTo.Any(), "Delivered Queue not cleared for successfull playlist post.");

            Assert.True(!airing.IgnoredQueues.Any(), "Ignored Queue not cleared for successfull playlist post.");
        }
    }
}