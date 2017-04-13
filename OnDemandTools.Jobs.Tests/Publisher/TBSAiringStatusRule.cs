using System.Linq;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Jobs.Tests.Helpers;
using Xunit;
using OnDemandTools.Business.Modules.Queue;

namespace OnDemandTools.Jobs.Tests.Publisher
{
    /// <summary>
    ///     Hint : order 1 runs first and  then order 2 runs
    /// </summary>
    [TestCaseOrderer("OnDemandTools.Jobs.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.Jobs.Tests")]
    [Collection("Jobs")]
    [Order(1)]
    public class TbsAiringStatusRule : BaseAiring
    {
        private const string ValidAiringStatusJson = "{ \"Status\": { \"MEDIUM\" : \"true\" } }";

        public TbsAiringStatusRule(JobTestFixture fixture)
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
        public void AiringStatus_Asset_ShouldBeCreated()
        {
            _airingId = PostAiringTest(_airingObjectHelper.UpdateDates(_jsonString, 1), "Active  Airing test");
            _queueTester.AddAiringToDataStore(_airingId, true, "Playlist Active Airing test", _tbsQueueKey);

            Assert.True(_airingId.StartsWith("TBSE"), "Airing id not begins with prefix TBSE");
        }

        [Fact]
        [Order(3)]
        public void AiringStatus_Asset_ShouldDeliverToQueue()
        {
            _queueTester.VerifyClientQueueDelivery();

            var allAiringMessages = _queueTester.ProcessedAirings.Select(e => string.Join(", ", e.Messages));

            Assert.True(!_queueTester.ProcessedAirings.Any(e => e.HasQueueDeliveryError),
                string.Join(", ", allAiringMessages));
        }


        [Fact]
        [Order(2)]
        public void AiringStatus_WithInvalidPayload_ShouldFail()
        {
            var airingStatusPayload = ValidAiringStatusJson;

            //Replacing airing status root element to invalid text
            airingStatusPayload = airingStatusPayload.Replace("Status", "Statuses");

            var statusCode = PostAiringStatus(airingStatusPayload, _airingId);

            Assert.True(statusCode == "BadRequest", "API Not Returned Bad Request for invalid airing status payload.");
        }

        [Fact]
        [Order(2)]
        public void AiringStatus_WithInvalidStatusKey_ShouldFail()
        {
            var airingStatusPayload = ValidAiringStatusJson;

            //Replacing airing status key to invalid status
            airingStatusPayload = airingStatusPayload.Replace("MEDIUM", "MEDIUMINVALID");

            var statusCode = PostAiringStatus(airingStatusPayload, _airingId);

            Assert.True(statusCode == "BadRequest", "API Not Returned Bad Request for invalid airing status key.");
        }

        [Fact]
        [Order(2)]
        public void AiringStatus_WithInvalidStatusValue_ShouldFail()
        {
            var airingStatusPayload = ValidAiringStatusJson;

            //Replacing airing status value with to invalid value
            airingStatusPayload = airingStatusPayload.Replace("true", "Completed");

            var statusCode = PostAiringStatus(airingStatusPayload, _airingId);

            Assert.True(statusCode == "BadRequest", "API Not Returned Bad Request for invalid airing status value.");
        }

        [Fact]
        [Order(4)]
        public void AiringStatus_WithValidPost_ShouldPassAndQueueVariableShouldBeCleared()
        {

            var airingStatusPayload = ValidAiringStatusJson;

            PostAiringStatus(airingStatusPayload, _airingId);

            var airingService = _fixture.Container.GetInstance<IAiringService>();

            var airing = airingService.GetBy(_airingId);

            Assert.True(!airing.DeliveredTo.Contains(_tbsQueueKey), "Delivered Queue not cleared for successfull Airing Status post.");

            Assert.True(airing.ChangeNotifications.Any(), "Change notifications not added for the status change.");

            Assert.True(airing.ChangeNotifications.Any(e => e.QueueName == _tbsQueueKey && e.ChangeNotificationType == ChangeNotificationType.Status.ToString()),
                "Change notification not added for the queue: " + _tbsQueueKey);
        }

        [Fact]
        [Order(5)]
        public void AiringStatus_StatusChange_ShouldDeliverToQueue()
        {
            _queueTester.VerifyClientQueueDelivery();

            var allAiringMessages = _queueTester.ProcessedAirings.Select(e => string.Join(", ", e.Messages));

            Assert.True(!_queueTester.ProcessedAirings.Any(e => e.HasQueueDeliveryError),
                string.Join(", ", allAiringMessages));

            var queueService = _fixture.Container.GetInstance<IQueueService>();

            var messages = queueService.GetAllMessagesDeliveredForAiringId(_airingId, _tbsQueueKey);

            var changeNotificationJson = "{\"ChangeNotificationType\":\"Status\",\"ChangedProperties\":[\"MEDIUM\"]}";

            var lastNotification = messages.OrderBy(e => e.DateTime).Last();

            Assert.True(lastNotification.Message.Contains(changeNotificationJson),
                string.Format("Airing: {0} Status notification not sent to Cloud AMQP", _airingId));

        }
    }
}