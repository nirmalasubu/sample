
using OnDemandTools.Jobs.Tests.Helpers;
using RestSharp;
using System.Collections.Generic;
using Xunit;

namespace OnDemandTools.Jobs.Tests.Publisher.PostAiring
{
    /// <summary>
    /// Hint : order 1 runs first and  then order 2 runs
    /// </summary>
    [TestCaseOrderer("OnDemandTools.Jobs.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.Jobs.Tests")]
    [Collection("Jobs")]
    [Order(1)]
    public class CartoonAiringRules : BaseAiringRule
    {
        private readonly string _cartoonQueueKey;
        private readonly AiringObjectHelper _airingObjectHelper;
        RestClient client;
        private readonly string _jsonString;
        private static List<string> airingIds = new List<string>();
        public CartoonAiringRules(JobTestFixture fixture)
            : base("CARE", "CartoonFullAccessApiKey")
        {
            _jsonString = Resources.Resources.CartoonAiringWith3Flights;
            _airingObjectHelper = new AiringObjectHelper();
            _cartoonQueueKey = fixture.Configuration["CartoonQueueApiKey"];
        }


        [Fact, Order(1)]
        public void ActiveAiringTest()
        {
            string airingId = PostAiringTest(_airingObjectHelper.UpdateDates(_jsonString, 1), "Active  Airing test");
            AiringDataStore.AddAiring(airingId, true, "Active Airing test", _cartoonQueueKey);
        }

        [Fact, Order(1)]
        public void FutureAiringTest()
        {
            string airingId = PostAiringTest(_airingObjectHelper.UpdateDates(_jsonString, 1), "Furture Airing test");
            AiringDataStore.AddAiring(airingId, true, "Furture Airing test", _cartoonQueueKey);
        }

        [Fact, Order(1)]
        public void ExpiredAiringTest()
        {
            string airingId = PostAiringTest(_airingObjectHelper.UpdateDates(_jsonString, -365), "Expired Airing test");
            AiringDataStore.AddAiring(airingId, true, "Expired Airing test", "", _cartoonQueueKey);
        }

        [Fact, Order(1)]
        public void ActiveToActiveAiringTest()
        {
            string airing = PostAiringTest(_airingObjectHelper.UpdateDates(_jsonString, 0), "Active to Active Airing test- Step 1");

            string updatedairing = _airingObjectHelper.UpdateAiringId(airing, _jsonString);

            string airingId = PostAiringTest(_airingObjectHelper.UpdateDates(updatedairing, 1), "Active to Active Airing test- Step 2");
            AiringDataStore.AddAiring(airingId, true, "Active to Active Airing test", _cartoonQueueKey); ;
        }

        [Fact, Order(1)]
        public void ActiveToExpiredAiringTest()
        {
            string airing = PostAiringTest(_airingObjectHelper.UpdateDates(_jsonString, 0), "Active to Expired Airing test- Step 1");

            string updatedairing = _airingObjectHelper.UpdateAiringId(airing, _jsonString);

            string airingId = PostAiringTest(_airingObjectHelper.UpdateDates(updatedairing, -365), "Active to Expired Airing test- Step 2");
            AiringDataStore.AddAiring(airingId, true, "Active to Expired Airing test", _cartoonQueueKey);
        }

        [Fact, Order(1)]
        public void ExpiredToActiveAiringTest()
        {
            string airing = PostAiringTest(_airingObjectHelper.UpdateDates(_jsonString, -365), "Expired to Active Airing test- Step 1");

            string updatedairing = _airingObjectHelper.UpdateAiringId(airing, _jsonString);

            string airingId = PostAiringTest(_airingObjectHelper.UpdateDates(updatedairing, 0), "Expired to Active Airing test- Step 2");
            AiringDataStore.AddAiring(airingId, true, "Expired to Active Airing test", _cartoonQueueKey); ;
        }

        [Fact, Order(1)]
        public void ExpiredToExpiredAiringTest()
        {
            string airing = PostAiringTest(_airingObjectHelper.UpdateDates(_jsonString, -365), "Expired to Expired Airing test- Step 1");

            string updatedairing = _airingObjectHelper.UpdateAiringId(airing, _jsonString);

            string airingId = PostAiringTest(_airingObjectHelper.UpdateDates(updatedairing, -365), "Expired to Expired Airing test- Step 2");
            AiringDataStore.AddAiring(airingId, true, "Expired to Expired Airing test", "", _cartoonQueueKey);
        }

    }

}
