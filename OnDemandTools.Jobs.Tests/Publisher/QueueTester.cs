﻿using Newtonsoft.Json.Linq;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Business.Modules.AiringPublisher;
using OnDemandTools.Business.Modules.Queue;
using OnDemandTools.Jobs.Tests.Helpers;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnDemandTools.Business.Modules.Reporting;
using Xunit;

namespace OnDemandTools.Jobs.Tests.Publisher
{

    public class QueueTester
    {
        private readonly JobTestFixture _fixture;
        private readonly RestClient _client;
        private readonly IPublisher _publisher;
        private readonly List<AiringDataStore> _processedAirings;
        public readonly IDfStatusService _dfStatusService;

        public List<AiringDataStore> ProcessedAirings => _processedAirings;

        public QueueTester(JobTestFixture fixture)
        {
            _fixture = fixture;
            _client = _fixture.RestClient;
            _publisher = _fixture.Container.GetInstance<IPublisher>();
            _dfStatusService = _fixture.Container.GetInstance<IDfStatusService>();
            _processedAirings = new List<AiringDataStore>();
        }

        #region DataStore Methods
        public void AddAiringToDataStore(string airing, bool assetShouldExistsInCurrentCollection, string testName, string expectedQueue = "", string unexpectedQueue = "", bool isDeleted = false)
        {
            var model = new AiringDataStore(airing)
            {
                AssetShouldExistsInCurrentCollection = assetShouldExistsInCurrentCollection,
                TestName = testName,
                IsDeleted = isDeleted,
                Priority = null
            };
            if (expectedQueue != "")
                model.ExpectedQueues.Add(expectedQueue);
            if (unexpectedQueue != "")
                model.UnExpectedQueues.Add(unexpectedQueue);

            model.AddMessage(string.Format("Airing successfully posted."));

            _processedAirings.Add(model);
        }

        public void AddAiringToDataStore(string airing, string testName, string expectedQueue, byte priority)
        {
            var model = new AiringDataStore(airing)
            {
                AssetShouldExistsInCurrentCollection = true,
                TestName = testName,
                ExpectedQueues = new List<string> { expectedQueue },
                UnExpectedQueues = new List<string>(),
                IsDeleted = false,
                Priority = priority
            };
            if (expectedQueue != "")
                model.ExpectedQueues.Add(expectedQueue);


            model.AddMessage(string.Format("Airing successfully posted."));

            _processedAirings.Add(model);
        }

        public void AddAiringToDataStore(string airing, string testName, string expectedQueue, string ignoredQueue = "")
        {
            var model = new AiringDataStore(airing)
            {
                AssetShouldExistsInCurrentCollection = true,
                TestName = testName,
                ExpectedQueues = new List<string>(),
                UnExpectedQueues = new List<string>(),
                IsDeleted = false,
                IgnoredQueues = new List<string>()
            };
            if (expectedQueue != "")
                model.ExpectedQueues.Add(expectedQueue);
            if (ignoredQueue != "")
                model.IgnoredQueues.Add(ignoredQueue);

            model.AddMessage(string.Format("Airing successfully posted."));

            _processedAirings.Add(model);
        }
        #endregion

        #region Queue Delivery test
        public void VerifyClientQueueDelivery()
        {
            if (!_processedAirings.Any())
            {
                Assert.True(false, "No airing found to run the pubslisher job");
            }

            IQueueService queueService = _fixture.Container.GetInstance<IQueueService>();

            var queues = _processedAirings.SelectMany(e => e.ExpectedQueues.ToArray()).Distinct().ToList();

            queues.AddRange(_processedAirings.SelectMany(e => e.UnExpectedQueues.ToArray()).Distinct().ToList());

            queues = queues.Distinct().ToList();

            foreach (var queue in queues)
            {
                var deliveryQueue = queueService.GetByApiKey(queue);

                if (deliveryQueue == null)
                {
                    Assert.True(false, string.Format("Unit test delivery queue not found: API Key {0}", queue));
                }

                queueService.Unlock(deliveryQueue.Name);

                _publisher.Execute(deliveryQueue.Name);
            }

            ProhibitResendMediaIdTest();

            ActiveAiringDeliveryTest();

            ExpiredAiringDelieryTest();

            PriorityQueueTest();

        }

        private void ProhibitResendMediaIdTest()
        {
            IAiringService airingService = _fixture.Container.GetInstance<IAiringService>();

            foreach (var activeAiring in _processedAirings)
            {
                if (!activeAiring.IgnoredQueues.Any()) continue;

                var airing = airingService.GetBy(activeAiring.AiringId,
                                         activeAiring.IsDeleted
                                             ? AiringCollection.DeletedCollection
                                             : AiringCollection.CurrentOrExpiredCollection);


                foreach (var ignoredQueue in activeAiring.IgnoredQueues)
                {
                    if (!airing.IgnoredQueues.Contains(ignoredQueue))
                    {

                        var failureMessage = string.Format("{0}. Airing {1} not added to ignored queue {2}", activeAiring.TestName,
                                                       activeAiring.AiringId, ignoredQueue);

                        activeAiring.HasQueueDeliveryError = true;
                        Assert.True(false, failureMessage);

                    }
                    else
                    {
                        activeAiring.AddMessage(string.Format("Airing successfully delivered to Ignored Queue {0}", ignoredQueue));
                    }
                }
            }
        }

        private void ActiveAiringDeliveryTest()
        {
            IAiringService airingService = _fixture.Container.GetInstance<IAiringService>();
            foreach (var activeAiring in _processedAirings)
            {
                if (!activeAiring.ExpectedQueues.Any()) continue;

                var airing = airingService.GetBy(activeAiring.AiringId,
                                          activeAiring.IsDeleted
                                              ? AiringCollection.DeletedCollection
                                              : AiringCollection.CurrentOrExpiredCollection);

                if (!_dfStatusService.HasMessages(activeAiring.AiringId, !activeAiring.IsDeleted))
                {
                    activeAiring.HasQueueDeliveryError = true;

                    Assert.True(false,
                        activeAiring.IsDeleted
                            ? string.Format(
                                "Airing {0}, DF messages added to current collection instead of Expired/Deleted collection",
                                activeAiring.AiringId)
                            : string.Format(
                                "Airing {0}, DF messages added to expired collection instead of active collection",
                                activeAiring.AiringId));
                }

                foreach (var expectedQueue in activeAiring.ExpectedQueues)
                {
                    if (!airing.DeliveredTo.Contains(expectedQueue))
                    {
                        var failureMessage = string.Format("{0}. Airing {1} not delivered to queue {2}", activeAiring.TestName,
                                                       activeAiring.AiringId, expectedQueue);

                        activeAiring.AddMessage(failureMessage, true);

                        activeAiring.HasQueueDeliveryError = true;

                        Assert.True(false, failureMessage);

                    }
                    else
                    {
                        activeAiring.AddMessage(string.Format("Airing successfully delivered to Queue {0}", expectedQueue));
                    }
                }
            }
        }

        private void ExpiredAiringDelieryTest()
        {
            IAiringService airingService = _fixture.Container.GetInstance<IAiringService>();
            foreach (var expiredAiring in _processedAirings)
            {
                if (!expiredAiring.UnExpectedQueues.Any()) continue;
                var airing = airingService.GetBy(expiredAiring.AiringId,
                                           expiredAiring.IsDeleted
                                               ? AiringCollection.DeletedCollection
                                               : AiringCollection.CurrentOrExpiredCollection);

                foreach (var queueName in expiredAiring.UnExpectedQueues)
                {
                    if (airing.DeliveredTo.Contains(queueName))
                    {
                        var failureMessage = string.Format("{0}. Airing {1} should not delivered to queue {2}",
                                                           expiredAiring.TestName,
                                                           expiredAiring.AiringId, queueName);

                        expiredAiring.AddMessage(failureMessage, true);
                        expiredAiring.HasQueueDeliveryError = true;
                        Assert.True(false, failureMessage);
                    }
                    else
                    {
                        expiredAiring.AddMessage(string.Format("Airing not delivered to Queue {0}", queueName));
                    }
                }
            }
        }

        private void PriorityQueueTest()
        {
            IQueueService queueService = _fixture.Container.GetInstance<IQueueService>();
            foreach (var airingWithPriority in _processedAirings)
            {
                if (airingWithPriority.Priority == null) continue;

                var messageDeliveryHistory =
                    queueService.GetMessageDeliveredForAiringId(airingWithPriority.AiringId, airingWithPriority.ExpectedQueues.First());


                if (messageDeliveryHistory == null) continue;

                if (airingWithPriority.Priority != messageDeliveryHistory.MessagePriority)
                {
                    airingWithPriority.AddMessage(string.Format(
                            "Airing delivered with incorrect priority {0}, expected priority {1}"
                            , messageDeliveryHistory.MessagePriority,
                            airingWithPriority.Priority
                          ));
                    airingWithPriority.HasQueueDeliveryError = true;
                    Assert.True(false, airingWithPriority.TestName + ". " + airingWithPriority.Messages.Last());
                }
                else
                {
                    airingWithPriority.AddMessage(
                        string.Format("Airing successfully delivered to with priority {0} for the start date {1}", messageDeliveryHistory.MessagePriority, ""));
                }
            }
        }

        public void PackageDeliveryTest()
        {
            var cartoonAiring = _processedAirings.Where(e => e.ExpectedQueues.Any()).First();

            var cartoonQueueName = cartoonAiring.ExpectedQueues.First();
            var cartoonAiringId = cartoonAiring.AiringId;

            JObject _cartoonAiring = new JObject();
            var requestAiring = new RestRequest("/v1/airing/" + cartoonAiringId, Method.GET);
            Task.Run(async () =>
            {
                _cartoonAiring = await _client.RetrieveRecord(requestAiring);

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

            PostPackage(jTitleIds, destination);

            CheckPackageQueueDelivery(_cartoonAiring, false, cartoonQueueName);


            titleIds[0] = (Convert.ToInt32(titleIds[0]) - 100).ToString();
            jTitleIds = JArray.FromObject(titleIds);

            PostPackage(jTitleIds, destination);

            CheckPackageQueueDelivery(_cartoonAiring, true, cartoonQueueName);
        }

        private void PostPackage(JArray jTitleIds, string destination)
        {
            JObject packageJson = JObject.Parse(Resources.Resources.ValidPackage);
            packageJson["TitleIds"] = jTitleIds;
            packageJson["DestinationCode"] = destination;
            var requestPackage = new RestRequest("/v1/package", Method.POST);
            requestPackage.AddParameter("application/json", packageJson, ParameterType.RequestBody);

            Task.Run(async () =>
            {
                await _client.RetrieveRecord(requestPackage);
            }).Wait();
        }

        private void CheckPackageQueueDelivery(JObject jAiring, bool isSuccessTest, string queueName)
        {
            var airingId = jAiring.Value<string>(@"airingId");

            var _airingService = _fixture.Container.GetInstance<IAiringService>();

            if (isSuccessTest)
            {
                if (_airingService.IsAiringDistributed(airingId, queueName))
                {

                    var message = string.Format("Package Queue reset test: Queue Name {0} not removed for airing {1}.", queueName,
                                            airingId);

                    Assert.True(false, message);
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
                    var message = string.Format("Package Queue reset test: DeliveredTo {0} has been removed for airing {1}.", queueName,
                                                airingId);
                    Assert.True(false, message);
                }
            }
        }

        #endregion
    }
}
