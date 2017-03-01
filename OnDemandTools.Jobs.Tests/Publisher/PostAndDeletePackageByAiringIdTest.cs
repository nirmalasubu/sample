using Newtonsoft.Json.Linq;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Business.Modules.AiringPublisher;
using OnDemandTools.Business.Modules.Queue;
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
        private readonly string _jsonString;
        private readonly IPublisher _publisher;
        private  static string _airingId;
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
            _publisher = _fixture.container.GetInstance<IPublisher>();
        }

        /// <summary>
        /// Step 1: New Airing Posted
        /// </summary>
        [Fact, Order(1)]
        public void PostAndDeletePackage_PostValidAiring_ReturnsAiringIdTest()
        {
            _airingId = PostAiringTest(_airingObjectHelper.UpdateDates(_jsonString, 1), "Active  Airing test");         

            Assert.True(_airingId.StartsWith("TBSE"), "Post Active Airing  posted with prefix TBSE");
        }

        /// <summary>
        /// Step 2 : Verify Airing delivered to Queue
        /// </summary>
        [Fact, Order(2)]
        public void PostAndDeletePackage_AiringDeliveryToQueue_ReturnsDeliveredToQueueTest()
        {
            var message = string.Format("Post airing :  Airing {0} has been delivered to the queue {1}.", _tbsQueueKey,
                                           _airingId);
            Assert.True(IsAiringIdDelvieredToQueue(), message);
        }

        /// <summary>
        /// Step 3: Post package for the airing 
        /// </summary>
        [Fact, Order(3)]
        public void PostAndDeletePackage_PostPackage_ReturnsPostedPackageTest()
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
        public void PostAndDeletePackage_PostPackageQueueNotification_ReturnsTrueTest()
        {
            var message = string.Format("Post Package : DeliveredTo QueueName {0} has been removed for airing {1}.", _tbsQueueKey,
                                            _airingId);

            Assert.True(PackageChangeQueueNotification(), message);
        }
        
        /// <summary>
        /// Verify Posted package notifications sent back to Queue
        /// </summary>
        [Fact, Order(5)]
        public void PostAndDeletePackage_PostedpackageQueueDelivery_ReturnsDeliveredToQueueTest()
        {
            var message = string.Format("Delete Package : Related Package airing {0} has been delivered to the queue {1}.", _tbsQueueKey,
                                           _airingId);
            Assert.True(IsAiringIdDelvieredToQueue(), message);
        }
        
        /// <summary>
        /// Step 6: delete package for the airing 
        /// </summary>
        [Fact, Order(6)]
        public void PostAndDeletePackage_DeletePackage_ReturnsDeletedMessageTest()
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
        public void PostAndDeletePackage_DeletePackageQueueNotification_ReturnsTrueTest()
        {
            var message = string.Format("Delete Package : DeliveredTo QueueName {0} has been removed for airing {1}.", _tbsQueueKey,
                                            _airingId);

            Assert.True(PackageChangeQueueNotification(), message);
        }

        /// <summary>
        /// step 8: Check the package deletion notification delivered to Queue
        /// </summary>
        [Fact, Order(8)]
        public void PostAndDeletePackage_DeletedpackageQueueDelivery_ReturnsDeliveredToQueueTest()
        {
            var message = string.Format("Delete Package : Related Package airing {0} has been delivered to the queue {1}.", _tbsQueueKey,
                                          _airingId);
           Assert.True(IsAiringIdDelvieredToQueue(), message);          
        }


        #region Private Methods 

        private bool PackageChangeQueueNotification()
        {
            var _airingService = _fixture.container.GetInstance<IAiringService>();

            return (!_airingService.IsAiringDistributed(_airingId, _tbsQueueKey));
        }

        private bool IsAiringIdDelvieredToQueue()
        {

            IQueueService queueService = _fixture.container.GetInstance<IQueueService>();
            var deliveryQueue = queueService.GetByApiKey(_tbsQueueKey);
            if (deliveryQueue == null)
            {
                Assert.True(false, string.Format("Unit test delivery queue not found: API Key {0}", _tbsQueueKey));
            }
            queueService.Unlock(deliveryQueue.Name);
            _publisher.Execute(deliveryQueue.Name);

            IAiringService airingService = _fixture.container.GetInstance<IAiringService>();

            var airing = airingService.GetBy(_airingId);

            return airing.DeliveredTo.Contains(_tbsQueueKey);
            
        }

        #endregion

    }


}
