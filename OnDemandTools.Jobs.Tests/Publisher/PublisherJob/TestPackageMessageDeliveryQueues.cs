using Newtonsoft.Json.Linq;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Jobs.Tests.Helpers;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OnDemandTools.Jobs.Tests.Publisher.PublisherJob
{
    [TestCaseOrderer("OnDemandTools.Jobs.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.Jobs.Tests")]
    [Collection("Jobs")]
    [Order(9)]
    public class TestPackageMessageDeliveryQueues
    {
        private const string CartoonBrand = "CAR";        
        private readonly string cartoonQueueName;
        private string cartoonProhibitResendMediaIdToQueueApiKey = Resources.Resources.CartoonProhibitResendMediaId;
        private string validPkg = Resources.Resources.ValidPackage;
        private readonly string _jsonString;
        private readonly AiringObjectHelper _airingObjectHelper;
        private readonly IAiringService _airingService;
        private string cartoonAiringId;
        private List<AiringDataStore> _alreadyDeliveredAirings;

        JobTestFixture fixture;
        RestClient client;

        public TestPackageMessageDeliveryQueues(JobTestFixture _fixture)
        {
            _jsonString = Resources.Resources.CartoonAiringWith3Flights;
            fixture = _fixture;
            client = fixture.restClient;
            _airingObjectHelper = new AiringObjectHelper();
            _airingService = fixture.container.GetInstance<IAiringService>();
            cartoonQueueName = fixture.Configuration["CartoonQueueApiKey"];
        }

        [Fact, Order(60)]
        public void CartoonInvalidTitlePostPackageTest()
        {
            _alreadyDeliveredAirings = AiringDataStore.ProcessedAirings.Where(e => e.AiringId.StartsWith(CartoonBrand)).ToList();
            cartoonAiringId = _alreadyDeliveredAirings.Select(s => s.AiringId).FirstOrDefault();
            //Thread.Sleep(500);
            JObject _cartoonAiring = new JObject();
            var requestAiring = new RestRequest("/v1/airing/" + cartoonAiringId, Method.GET);
            Task.Run(async () =>
            {
                _cartoonAiring = await client.RetrieveRecord(requestAiring);

            }).Wait();

            List<string> titleIds = new List<string>();
            foreach (var element in (_cartoonAiring.SelectToken("title.titleIds")).Children())
            {
                if (element.Value<string>(@"authority") == "Turner")
                    titleIds.Add(element.Value<string>(@"value"));
            }
            titleIds[0] = (Convert.ToInt32(titleIds[0]) + 100).ToString();
            var jTitleIds = JArray.FromObject(titleIds);
            var destination = _cartoonAiring.SelectToken("flights[0].destinations[0]").Value<string>(@"name");


            JObject packageJson = JObject.Parse(validPkg);
            packageJson["TitleIds"] = jTitleIds;
            packageJson["DestinationCode"] = destination;
            var requestPackage = new RestRequest("/v1/package", Method.POST);
            requestPackage.AddParameter("application/json", packageJson, ParameterType.RequestBody);

            Task.Run(async () =>
            {
                await client.RetrieveRecord(requestPackage);

            }).Wait();

            CheckQueueDelivery(_cartoonAiring, CartoonBrand, false, cartoonQueueName);
        }

        private void CheckQueueDelivery(JObject jAiring, string brand, bool isSuccessTest, string queueName)
        {
            var airingId = jAiring.Value<string>(@"airingId");

            if (isSuccessTest)
            {
                if (_airingService.IsAiringDistributed(airingId, queueName))
                {
                    if (queueName != cartoonProhibitResendMediaIdToQueueApiKey)
                    {
                        var message = string.Format("Package Queue reset test: Queue Name {0} not removed for airing {1}.", queueName,
                                                airingId);

                        Assert.True(false, message);
                    }
                }
                else
                {
                    var message = string.Format("Package Queue reset test:  Queue Name {0} successfully removed in DeliveredTo", queueName);

                    Assert.True(true, message);
                }
            }
            else
            {
                if (!_airingService.IsAiringDistributed(airingId, queueName))
                {
                    if (queueName != cartoonProhibitResendMediaIdToQueueApiKey)
                    {
                        var message = string.Format("Package Queue reset test: DeliveredTo {0} has been removed for airing {1}.", queueName,
                                                    airingId);

                        Assert.True(false, message);
                    }
                }
            }
        }
    }
}
