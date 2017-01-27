using Newtonsoft.Json.Linq;
using OnDemandTools.API.Tests.Helpers;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OnDemandTools.API.Tests.AiringRoute.PostAiring
{
    [TestCaseOrderer("OnDemandTools.API.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.API.Tests")]
    [Collection("API Collection")]
    public class CartoonAiringRules : BasePostAiringRule
    {
        private readonly string _cartoonQueueKey;
        private readonly AiringObjectHelper _airingObjectHelper;
        APITestFixture fixture;
        RestClient client;
        private readonly string _jsonString;
        private static List<string> airingIds = new List<string>();
        public CartoonAiringRules(APITestFixture fixture)
            : base("CARE", fixture)
        {
            _jsonString = Resources.Resources.CartoonAiringWith3Flights;
            _airingObjectHelper = new AiringObjectHelper();
        }

        [Fact, Order(4)]
        public void ActiveAndExpiredAiringTest()
        {
            string airingId = PostAiringTest(_airingObjectHelper.UpdateDates(_jsonString, -8), "Active and Expired Airing test");
            airingIds.Add(airingId);
        }

        [Fact, Order(4)]
        public void ActiveAiringTest()
        {
            string airingId = PostAiringTest(_airingObjectHelper.UpdateDates(_jsonString, 0), "Active  Airing test");
            airingIds.Add(airingId);
        }

        [Fact, Order(4)]
        public void FutureAiringTest()
        {
            string airingId = PostAiringTest(_airingObjectHelper.UpdateDates(_jsonString, 101), "Furture Airing test");
            airingIds.Add(airingId);
        }

        [Fact, Order(4)]
        public void ExpiredAiringTest()
        {
            string airingId = PostAiringTest(_airingObjectHelper.UpdateDates(_jsonString, -365), "Expired Airing test");
            airingIds.Add(airingId);
        }

        [Fact, Order(4)]
        public void ActiveToActiveAiringTest()
        {
            string airing = PostAiringTest(_airingObjectHelper.UpdateDates(_jsonString, 0), "Active to Active Airing test- Step 1");

            string updatedairing = _airingObjectHelper.UpdateAiringId(airing, _jsonString);

            string airingId = PostAiringTest(_airingObjectHelper.UpdateDates(updatedairing, 0), "Active to Active Airing test- Step 2");
            airingIds.Add(airingId);
        }

        [Fact, Order(4)]
        public void ActiveToExpiredAiringTest()
        {
            string airing = PostAiringTest(_airingObjectHelper.UpdateDates(_jsonString, 0), "Active to Expired Airing test- Step 1");

            string updatedairing = _airingObjectHelper.UpdateAiringId(airing, _jsonString);

            string airingId = PostAiringTest(_airingObjectHelper.UpdateDates(updatedairing, -365), "Active to Expired Airing test- Step 2");
            airingIds.Add(airingId);
        }
        
        [Fact, Order(4)]
        public void ExpiredToActiveAiringTest()
        {
            string airing = PostAiringTest(_airingObjectHelper.UpdateDates(_jsonString, -365), "Expired to Active Airing test- Step 1");

            string updatedairing = _airingObjectHelper.UpdateAiringId(airing, _jsonString);

            string airingId = PostAiringTest(_airingObjectHelper.UpdateDates(updatedairing, 0), "Expired to Active Airing test- Step 2");
            airingIds.Add(airingId);
        }

        [Fact, Order(4)]
        public void ExpiredToExpiredAiringTest()
        {
            string airing = PostAiringTest(_airingObjectHelper.UpdateDates(_jsonString, -365), "Expired to Expired Airing test- Step 1");

            string updatedairing = _airingObjectHelper.UpdateAiringId(airing, _jsonString);

            string airingId = PostAiringTest(_airingObjectHelper.UpdateDates(updatedairing, -365), "Expired to Expired Airing test- Step 2");
            airingIds.Add(airingId);
        }

        [Fact, Order(4)]
        public void DeleteAiringTest()
        {
            foreach (string airingid in airingIds)
            {
                DeleteAiringRequest(airingid, "Delete Airing failed for  :"+airingid);
            }
            Dispose();
        }
       
        private void Dispose()
        {
            airingIds = null;
        }
    }

}
