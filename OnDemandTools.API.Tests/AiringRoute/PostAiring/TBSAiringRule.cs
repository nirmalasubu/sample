using OnDemandTools.API.Tests.Helpers;
using RestSharp;
using System.Collections.Generic;
using Xunit;

namespace OnDemandTools.API.Tests.AiringRoute.PostAiring
{
    /// <summary>
    /// Hint : order 1 runs first and  then order 2 runs
    /// </summary>
    [TestCaseOrderer("OnDemandTools.API.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.API.Tests")]
    [Collection("API Collection")]
    public class TBSAiringRule : BaseAiringRule
    {
        private readonly string _cartoonQueueKey;
        private readonly AiringObjectHelper _airingObjectHelper;
      
        RestClient client;
        private readonly string _jsonString;
        private static List<string> airingIds = new List<string>();
        public TBSAiringRule()
            : base("TBSE", "TBSFullAccessApiKey")
        {
            _jsonString = Resources.Resources.TBSAiringWithSingleFlight;
            _airingObjectHelper = new AiringObjectHelper();
        }

        [Fact, Order(1)]
        public void ActiveAndExpiredAiringTest()
        {
            string airingId = PostAiringTest(_airingObjectHelper.UpdateDates(_jsonString, -8), "Active and Expired Airing test");
            airingIds.Add(airingId);
        }

        [Fact, Order(1)]
        public void ActiveAiringTest()
        {
            string airingId = PostAiringTest(_airingObjectHelper.UpdateDates(_jsonString, 0), "Active  Airing test");
            airingIds.Add(airingId);
        }

        [Fact, Order(1)]
        public void FutureAiringTest()
        {
            string airingId = PostAiringTest(_airingObjectHelper.UpdateDates(_jsonString, 101), "Furture Airing test");
            airingIds.Add(airingId);
        }

        [Fact, Order(1)]
        public void ExpiredAiringTest()
        {
            string airingId = PostAiringTest(_airingObjectHelper.UpdateDates(_jsonString, -365), "Expired Airing test");
            airingIds.Add(airingId);
        }

        [Fact, Order(1)]
        public void ActiveToActiveAiringTest()
        {
            string airing = PostAiringTest(_airingObjectHelper.UpdateDates(_jsonString, 0), "Active to Active Airing test- Step 1");

            string updatedairing = _airingObjectHelper.UpdateAiringId(airing, _jsonString);

            string airingId = PostAiringTest(_airingObjectHelper.UpdateDates(updatedairing, 0), "Active to Active Airing test- Step 2");
            airingIds.Add(airingId);
        }

        [Fact, Order(1)]
        public void ActiveToExpiredAiringTest()
        {
            string airing = PostAiringTest(_airingObjectHelper.UpdateDates(_jsonString, 0), "Active to Expired Airing test- Step 1");

            string updatedairing = _airingObjectHelper.UpdateAiringId(airing, _jsonString);

            string airingId = PostAiringTest(_airingObjectHelper.UpdateDates(updatedairing, -365), "Active to Expired Airing test- Step 2");
            airingIds.Add(airingId);
        }

        [Fact, Order(1)]
        public void ExpiredToActiveAiringTest()
        {
            string airing = PostAiringTest(_airingObjectHelper.UpdateDates(_jsonString, -365), "Expired to Active Airing test- Step 1");

            string updatedairing = _airingObjectHelper.UpdateAiringId(airing, _jsonString);

            string airingId = PostAiringTest(_airingObjectHelper.UpdateDates(updatedairing, 0), "Expired to Active Airing test- Step 2");
            airingIds.Add(airingId);
        }

        [Fact, Order(1)]
        public void ExpiredToExpiredAiringTest()
        {
            string airing = PostAiringTest(_airingObjectHelper.UpdateDates(_jsonString, -365), "Expired to Expired Airing test- Step 1");

            string updatedairing = _airingObjectHelper.UpdateAiringId(airing, _jsonString);

            string airingId = PostAiringTest(_airingObjectHelper.UpdateDates(updatedairing, -365), "Expired to Expired Airing test- Step 2");
            airingIds.Add(airingId);
        }

        [Fact, Order(2)]
        public void DeleteTBSAiringTest()
        {
            foreach (string airingid in airingIds)
            {
                DeleteAiringRequest(airingid, "Delete Airing failed for  :" + airingid);
            }
            Dispose();
        }
        private void Dispose()
        {
            airingIds = null;
        }
    }
}
