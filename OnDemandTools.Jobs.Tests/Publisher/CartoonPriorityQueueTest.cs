using OnDemandTools.Jobs.Tests.AiringRoute.PostAiring;
using OnDemandTools.Jobs.Tests.Helpers;
using OnDemandTools.Jobs.Tests.Resources;
using RestSharp;
using System.Collections.Generic;
using Xunit;

namespace OnDemandTools.Jobs.Tests.Publisher
{

    [TestCaseOrderer("OnDemandTools.Jobs.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.Jobs.Tests")]
    [Collection("Job Collection")]
    public class CartoonPriorityQueueTest : BaseAiringRule
    {
        private readonly string _cartoonQueueKey;
        private readonly AiringObjectHelper _airingObjectHelper;


        RestClient client;
        private readonly string _jsonString;
        private static List<string> airingIds = new List<string>();

        public CartoonPriorityQueueTest(JobTestFixture fixture)
              : base("CARE", "CartoonFullAccessApiKey")
        {

            _jsonString = Resources.Resources.CartoonAiringWith3Flights;
            _airingObjectHelper = new AiringObjectHelper();
            _cartoonQueueKey = fixture.Configuration["CartoonPriorityQueueApiKey"];

        }

        [Fact, Order(1)]
        public void AiringStartedTest()
        {
            string airingId = PostAiringTest(_airingObjectHelper.UpdateDeliverImmedialtely(_jsonString,-3,false), "Priority: Airing Started Test");
      
            AiringDataStore.AddAiring(airingId, "Priority: Airing Started Test", _cartoonQueueKey, 7);
        }

        [Fact, Order(1)]
        public void AiringStartsTodayTest()
        {
            string airingId = PostAiringTest(_airingObjectHelper.UpdateDeliverImmedialtely(_jsonString, 0, false), "Priority: Airing Starts Today Test");
           
            AiringDataStore.AddAiring(airingId, "Priority: Airing Starts Today Test", _cartoonQueueKey, 5);
        }

        [Fact, Order(1)]
        public void AiringStartsInNext1DayTest()
        {
            string airingId = PostAiringTest(_airingObjectHelper.UpdateDeliverImmedialtely(_jsonString, 1, false), "Priority: Airing Starts In Next 1 Day Test");
           
            AiringDataStore.AddAiring(airingId, "Priority: Airing Starts In Next 1 Day Test", _cartoonQueueKey, 4);
        }

        [Fact, Order(1)]
        public void AiringStartsInNext2DayTest()
        {
            string airingId = PostAiringTest(_airingObjectHelper.UpdateDeliverImmedialtely(_jsonString,2, false), "Priority: Airing Starts In Next 2 Days Test");
          
            AiringDataStore.AddAiring(airingId, "Priority: Airing Starts In Next 2 Days Test", _cartoonQueueKey, 4);
        }

        [Fact, Order(1)]
        public void AiringStartsInNext3DayTest()
        {
            string airingId = PostAiringTest(_airingObjectHelper.UpdateDeliverImmedialtely(_jsonString, 3, false), "Priority: Airing Starts In Next 3 Day Test");
          
            AiringDataStore.AddAiring(airingId, "Priority: Airing Starts In Next 3 Day Test", _cartoonQueueKey, 3);
        }

        [Fact, Order(1)]
        public void AiringStartsInNext4DayTest()
        {
            string airingId = PostAiringTest(_airingObjectHelper.UpdateDeliverImmedialtely(_jsonString, 4, false), "Priority: Airing Starts In Next 4 Day Test");
           
            AiringDataStore.AddAiring(airingId, "Priority: Airing Starts In Next 4 Day Test", _cartoonQueueKey, 3);
        }

        [Fact, Order(1)]
        public void AiringStartsInNext7DayTest()
        {
            string airingId = PostAiringTest(_airingObjectHelper.UpdateDeliverImmedialtely(_jsonString,7, false), "Priority: Airing Starts In Next 7 Day Test");
         
            AiringDataStore.AddAiring(airingId, "Priority: Airing Starts In Next 7 Day Test", _cartoonQueueKey, 2);
        }


        [Fact, Order(1)]
        public void AiringStartsAfter1WeekTest()
        {
            string airingId = PostAiringTest(_airingObjectHelper.UpdateDeliverImmedialtely(_jsonString, 8, false), "Priority: Airing Starts After 1 Week Test");
         
            AiringDataStore.AddAiring(airingId, "Priority: Airing Starts After 1 Week Test ", _cartoonQueueKey, 2);
        }


        [Fact, Order(1)]
        public void AiringStartsInNext10DayTest()
        {
            string airingId = PostAiringTest(_airingObjectHelper.UpdateDeliverImmedialtely(_jsonString,10, false), "Priority: Airing Starts In Next 10 Day Test");
       
            AiringDataStore.AddAiring(airingId, "Priority: Airing Starts In Next 10 Day Test", _cartoonQueueKey, 2);
        }

        [Fact, Order(1)]
        public void AiringStartsWith2WeekTest()
        {
            string airingId = PostAiringTest(_airingObjectHelper.UpdateDeliverImmedialtely(_jsonString,14, false), "Priority: Airing Starts With 2 Week Test");
         
            AiringDataStore.AddAiring(airingId, "Priority: Airing Starts With 2 Week Test", _cartoonQueueKey, 1);
        }

        [Fact, Order(1)]
        public void AiringStartsAfter2WeekTest()
        {
            string airingId = PostAiringTest(_airingObjectHelper.UpdateDeliverImmedialtely(_jsonString, 15, false), "Priority: Airing Starts After 2 Week Test");
        
            AiringDataStore.AddAiring(airingId, "Priority: Airing Starts After 2 Week Test", _cartoonQueueKey, 1);
        }


        [Fact, Order(1)]
        public void AiringExpiredTest()
        {
            string airingId = PostAiringTest(_airingObjectHelper.UpdateDeliverImmedialtely(_jsonString, -100, true), "Priority: Airing Expired Test");
      
            AiringDataStore.AddAiring(airingId, "Priority: Airing Expired Test", _cartoonQueueKey, 0);
        }
    }
}
