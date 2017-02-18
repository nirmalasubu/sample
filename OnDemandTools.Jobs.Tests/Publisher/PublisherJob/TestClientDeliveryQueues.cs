using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Business.Modules.Queue;
using OnDemandTools.Jobs.Tests.Helpers;
using RestSharp;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OnDemandTools.Jobs.Tests.Publisher.PublisherJob
{
    [TestCaseOrderer("OnDemandTools.Jobs.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.Jobs.Tests")]
    [Collection("Jobs")]
    [Order(1)]
    public class TestClientDeliveryQueues
    {
        JobTestFixture _fixture;
        RestClient _jobClient;

        public TestClientDeliveryQueues(JobTestFixture fixture)
        {
            _fixture = fixture;
            _jobClient = _fixture.jobRestClient;
        }


        [Fact, Order(1)]
        public void VerifyClientQueueDelivery()
        {

            IQueueService queueService = _fixture.container.GetInstance<IQueueService>();

            var queues = AiringDataStore.ProcessedAirings.SelectMany(e => e.ExpectedQueues.ToArray()).Distinct().ToList();

            queues.AddRange(AiringDataStore.ProcessedAirings.SelectMany(e => e.UnExpectedQueues.ToArray()).Distinct().ToList());

            queues = queues.Distinct().ToList();

            foreach (var queue in queues)
            {
                var deliveryQueue = queueService.GetByApiKey(queue);

                if (deliveryQueue == null)
                {
                    Assert.True(false, string.Format("Unit test delivery queue not found: API Key {0}", queue));
                }

                var request = new RestRequest("/api/unittest/" + deliveryQueue.Name, Method.GET);
                request.Timeout = (10 * 60 * 1000); // 10minutes

                Task.Run(async () =>
                {
                    string response = await _jobClient.RetrieveString(request);

                }).Wait();
            }

            ProhibitResendMediaIdTest();

            ActiveAiringDeliveryTest();

            ExpiredAiringDelieryTest();

            PriorityQueueTest();

        }

        private void ProhibitResendMediaIdTest()
        {
            IAiringService airingService = _fixture.container.GetInstance<IAiringService>();

            foreach (var activeAiring in AiringDataStore.ProcessedAirings)
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

                        var failureMessage = string.Format("{0}. Airing {1} not delivered to queue {2}", activeAiring.TestName,
                                                       activeAiring.AiringId, ignoredQueue);

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
            IAiringService airingService = _fixture.container.GetInstance<IAiringService>();
            foreach (var activeAiring in AiringDataStore.ProcessedAirings)
            {
                if (!activeAiring.ExpectedQueues.Any()) continue;

                var airing = airingService.GetBy(activeAiring.AiringId,
                                          activeAiring.IsDeleted
                                              ? AiringCollection.DeletedCollection
                                              : AiringCollection.CurrentOrExpiredCollection);

                foreach (var expectedQueue in activeAiring.ExpectedQueues)
                {
                    if (!airing.DeliveredTo.Contains(expectedQueue))
                    {
                        var failureMessage = string.Format("{0}. Airing {1} not delivered to queue {2}", activeAiring.TestName,
                                                       activeAiring.AiringId, expectedQueue);

                        activeAiring.AddMessage(failureMessage);
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
            IAiringService airingService = _fixture.container.GetInstance<IAiringService>();
            foreach (var expiredAiring in AiringDataStore.ProcessedAirings)
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
            IQueueService queueService = _fixture.container.GetInstance<IQueueService>();
            foreach (var airingWithPriority in AiringDataStore.ProcessedAirings)
            {
                if (airingWithPriority.Priority == null) continue;

                var messageDeliveryHistory =
                    queueService.GetMessageDeliveredForAiringId( airingWithPriority.AiringId,airingWithPriority.ExpectedQueues.First());
            

                if (messageDeliveryHistory == null) continue;

                if (airingWithPriority.Priority != messageDeliveryHistory.MessagePriority)
                {
                    airingWithPriority.AddMessage(string.Format(
                            "Airing delivered with incorrect priority {0}, expected priority {1}"
                            , messageDeliveryHistory.MessagePriority,
                            airingWithPriority.Priority
                          ));

                    Assert.True(false, airingWithPriority.TestName + ". " + airingWithPriority.Messages.Last());
                }
                else
                {
                    airingWithPriority.AddMessage(
                        string.Format("Airing successfully delivered to with priority {0} for the start date {1}", messageDeliveryHistory.MessagePriority, ""));
                }
            }
        }
    }
}
