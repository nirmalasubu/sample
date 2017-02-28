using Newtonsoft.Json.Linq;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Jobs.Tests.Helpers;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OnDemandTools.Jobs.Tests.Publisher
{
    /// <summary>
    /// Hint : order 1 runs first and  then order 2 runs
    /// </summary>
    [TestCaseOrderer("OnDemandTools.Jobs.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.Jobs.Tests")]
    [Collection("Jobs")]
    [Order(1)]
    public class PostAndDeletePackageByAiringIdTest : BaseAiring
    {

        private readonly string _tbsQueueKey;
        private readonly AiringObjectHelper _airingObjectHelper;
        private static QueueTester _queueTester;
        private readonly string _jsonString;       
        private static string _airingId;
        JobTestFixture _fixture;
        RestClient _client;

        public PostAndDeletePackageByAiringIdTest(JobTestFixture fixture)
            : base("TBSE", "TBSFullAccessApiKey", fixture)
        {
            _airingObjectHelper = new AiringObjectHelper();
            _jsonString = _airingObjectHelper.UpdateDates(Resources.Resources.TBSAiringWithSingleFlight, 1);

            _tbsQueueKey = fixture.Configuration["TbsQueueApiKey"];

            _fixture = fixture;
            _client = _fixture.restClient;

            if (_queueTester == null)
                _queueTester = new QueueTester(fixture);
        }

        /// <summary>
        /// Step 1: New Airing Posted
        /// </summary>
        [Fact, Order(1)]
        public void Post_ValidAiring()
        {
            _airingId = PostAiringTest(_airingObjectHelper.UpdateDates(_jsonString, 1), "Active  Airing test");
            _queueTester.AddAiringToDataStore(_airingId, true, "Post Active Airing test: For package", _tbsQueueKey);

            Assert.True(_airingId.StartsWith("TBSE"), "Airing id not begins with prefix TBSE");
        }

        /// <summary>
        /// Step 2 : Verify Airing delivered to Queue
        /// </summary>
        [Fact, Order(2)]
        public void PostedAiringQueueDeliveryTest()
        {
            _queueTester.VerifyClientQueueDelivery();
        }

        /// <summary>
        /// Step 3: Post package for the airing 
        /// </summary>
        [Fact, Order(3)]
        public void PostPackagetest()
        {
            JObject packageJson = JObject.Parse(Resources.Resources.PackageWithNoIds);
            packageJson.Add("AiringId", _airingId);           
            var requestPackage = new RestRequest("/v1/package", Method.POST);
            requestPackage.AddParameter("application/json", packageJson, ParameterType.RequestBody);
            JObject response = new JObject();
            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(requestPackage);
            }).Wait();

            string airingId = response.Value<string>(@"airingId");

            Assert.True(airingId.Equals(_airingId));
        }

        /// <summary>
        /// step 4: Check the queue Name is removed from DeliveredTo
        /// </summary>
        /// <param name="airingId"></param>
        /// <param name="queueName"></param>
        /// 
        [Fact, Order(4)]
        public void PostedPackageQueueNotificationTest()
        {
            PackageChangeQueueNotification();
        }
        
        /// <summary>
        /// Verify Posted package notifications sent back to Queue
        /// </summary>
        [Fact, Order(5)]
        public void PostedpackageQueueDeliveryTest()
        {
            _queueTester.VerifyClientQueueDelivery();
        }
        
        /// <summary>
        /// Step 6: Post package for the airing 
        /// </summary>
        [Fact, Order(6)]
        public void DeletePackagetest()
        {
            JObject packageJson = JObject.Parse(Resources.Resources.PackageWithNoIds);
            packageJson.Add("AiringId", _airingId);
            var requestPackage = new RestRequest("/v1/package", Method.DELETE);
            requestPackage.AddParameter("application/json", packageJson, ParameterType.RequestBody);
            JObject response = new JObject();
            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(requestPackage);
            }).Wait();

            string Message = response.Value<string>(@"message");

            Assert.True(Message.Contains("Package deleted successfully"));
        }

        /// <summary>
        /// step 7: Check the queue Name is removed from DeliveredTo
        /// </summary>
        /// <param name="airingId"></param>
        /// <param name="queueName"></param>       
        [Fact, Order(7)]
        public void DeletedPackageQueueNotificationTest()
        {
            PackageChangeQueueNotification();
        }

        [Fact, Order(8)]
        public void DeletedpackageQueueDeliveryTest()
        {
            _queueTester.VerifyClientQueueDelivery();
        }

        /// <summary>
        /// step 8: Check the package deletion notification delivered to Queue
        /// </summary>
        /// <param name="airingId"></param>
        /// <param name="queueName"></param>  
        private void PackageChangeQueueNotification()
        {
            var _airingService = _fixture.container.GetInstance<IAiringService>();

            if (!_airingService.IsAiringDistributed(_airingId, _tbsQueueKey))
            {
                var message = string.Format("Package Queue : DeliveredTo {0} has been removed for airing {1}.", _tbsQueueKey,
                                            _airingId);
                Assert.True(true, message);
            }
        }
    }


}
