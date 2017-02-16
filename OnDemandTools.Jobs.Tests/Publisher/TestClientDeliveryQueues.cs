using OnDemandTools.Jobs.Tests.Helpers;
using RestSharp;
using Xunit;
using System.Linq;
using OnDemandTools.Business.Modules.Queue;
using OnDemandTools.Business.Modules.Airing;
using System.Threading;

namespace OnDemandTools.Jobs.Tests.Publisher
{
    [TestCaseOrderer("OnDemandTools.Jobs.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.Jobs.Tests")]
    [Collection("Job Collection")]
    public class TestClientDeliveryQueues
    {
        JobTestFixture _fixture;
        RestClient _client;

       public TestClientDeliveryQueues(JobTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact, Order(10)]
        public void VerifyClientQueueDelivery1()
        {
        }


        [Fact, Order(10)]
        public void VerifyClientQueueDelivery()
        {
            
            IQueueService queueService = _fixture.container.GetInstance<IQueueService>();
            OnDemandTools.Jobs.JobRegistry.Publisher.Publisher publisher = _fixture.container.GetInstance<OnDemandTools.Jobs.JobRegistry.Publisher.Publisher>();

           
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

                publisher.Execute(deliveryQueue.Name);
            }

            ProhibitResendMediaIdTest();

        }

        private void ProhibitResendMediaIdTest()    
        {
            IAiringService airingService = _fixture.container.GetInstance<IAiringService>();
            
            foreach (var activeAiring in AiringDataStore.ProcessedAirings)
            {
                if (!activeAiring.IgnoredQueues.Any()) continue;

                var airing = airingService.GetBy(activeAiring.Airing,
                                         activeAiring.IsDeleted
                                             ? AiringCollection.DeletedCollection
                                             : AiringCollection.CurrentOrExpiredCollection);


                foreach (var ignoredQueue in activeAiring.IgnoredQueues)
                {
                    if (!airing.IgnoredQueues.Contains(ignoredQueue))
                    {

                        var failureMessage = string.Format("{0}. Airing {1} not delivered to queue {2}", activeAiring.TestName,
                                                       activeAiring.Airing, ignoredQueue);

                       // activeAiring.AddMessage(failureMessage);
                        Assert.True(false,failureMessage);

                    }
                    else
                    {
                        activeAiring.AddMessage(string.Format("Airing successfully delivered to Ignored Queue {0}", ignoredQueue));
                    }
                }
            }
        }
    }
    
}
