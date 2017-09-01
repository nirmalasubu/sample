using Newtonsoft.Json.Linq;
using OnDemandTools.API.Tests.Helpers;
using RestSharp;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OnDemandTools.API.Tests.DestinationRoute
{

    [TestCaseOrderer("OnDemandTools.API.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.API.Tests")]
    [Collection("API Collection")]
    public class GetDestinationRule
    {

        APITestFixture _fixture;
        RestClient _client;

        public GetDestinationRule(APITestFixture fixture)
        {
            _fixture = fixture;
            _client = _fixture.restClient;
        }


        /// <summary>
        /// Valid destination test by passing valid destination name "UTEST". 
        /// If the name does not exists create a "UTEST" destination  and give destination permission to the user to pass the test
        /// </summary>
        [Fact, Order(1)]
        public void GetDestinationByNamePassingValidDestinationReturnsValidDestination()
        {
            string destinationName = "UTEST";
            JObject response = new JObject();
            var request = new RestRequest("/v1/destination/" + destinationName, Method.GET);
            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);

            }).Wait();

            string value = response.Value<string>(@"StatusCode");
            if (value != null)
            {
                Assert.True(false, "Error in getting Destination :" + destinationName);
            }

            string responseDestinationName = response.Value<string>(@"name");
            JArray properties = response.Value<JArray>(@"properties");


            Assert.True(responseDestinationName == destinationName, string.Format("UTEST destination name is passed but it is returned as {0}", responseDestinationName));

        }


        /// <summary>
        /// Invalid destination test by  passing Invalid destination name.
        /// If the destination name "UTEST2XXX12" then remove the permission for this destination  for the user.
        /// </summary>
        [Fact, Order(1)]
        public void GetDestinationByNamePassingInValidDestinationReturnsRequestDenied()
        {
            string destinationName = "UTEST2XXX12";
            JObject response = new JObject();
            var request = new RestRequest("/v1/destination/" + destinationName, Method.GET);
            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);

            }).Wait();

            string value = response.Value<string>(@"StatusCode");
            if (value == null)
            {
                Assert.True(false, string.Format("destination name has been created {0}", destinationName));
            }

            Assert.True(value != null, string.Format("destination name does not exists or user has no permission to the destination :{0}", destinationName));
        }

        /// <summary>
        /// To Test existance of category within the properties of the destination
        /// If the test fails create category "UNITTESTCategory" for destination "UTEST"
        /// </summary>
        [Fact, Order(1)]
        public void GetDestinationByNamehavingCategoryWithinProperty()
        {
            string destinationName = "UTEST";
            JObject response = new JObject();
            var request = new RestRequest("/v1/destination/" + destinationName, Method.GET);
            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);

            }).Wait();

            string value = response.Value<string>(@"StatusCode");
            if (value != null)
            {
                Assert.True(false, "Error in getting Destination :" + destinationName);
            }

            string responseDestinationName = response.Value<string>(@"name");
            JArray properties = response.Value<JArray>(@"properties");
            bool isCategoryExists = false;
            foreach (var item in properties.Children())
            {
                var itemProperties = item.Children<JProperty>();
                var nameProperty = itemProperties.FirstOrDefault(x => x.Name == "name");
                var valueProperty = itemProperties.FirstOrDefault(x => x.Name == "value");
                if (nameProperty.Value.ToString().Equals("Category") && valueProperty.Value.ToString().Equals("UNITTESTCategory"))
                {
                    isCategoryExists = true;
                }

            }
            Assert.True(isCategoryExists, string.Format("Category name 'UNITTESTCategory' does not eixists with the destinstion properties"));
        }

    }
}

