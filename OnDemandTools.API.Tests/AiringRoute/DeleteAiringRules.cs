using Newtonsoft.Json.Linq;
using OnDemandTools.API.Tests.Helpers;
using RestSharp;
using System;
using System.Threading.Tasks;
using Xunit;

namespace OnDemandTools.API.Tests
{
    
    [TestCaseOrderer("OnDemandTools.API.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.API.Tests")]   
    [Collection("API Collection")]
    public class DeleteAiringRules
    {
        APITestFixture fixture;
        RestClient client;
        public DeleteAiringRules(APITestFixture fixture)
        {
            this.fixture = fixture;
            this.client = this.fixture.restClient;
        }


        // Some example code for you to get started, not necessarily actual code
        [Fact, Order(2)]       
        public void PassingTest()
        {

            Console.WriteLine(client.BaseUrl);
            JArray response = default(JArray);
            var request = new RestRequest("/v1/destinations", Method.GET);
            Task.Run(async () =>
                {
                    response = await client.RetrieveRecords(request);

                }).Wait(); 
            
            if(response != null)
            {
                Console.WriteLine(response.ToString());
                Assert.True(response.Count > 1);
            }
            else
            {
                Assert.True(false);
            }
        }


        // Some example code for you to get started, not necessarily actual code
        [Fact, Order(1)]       
        public void PassingTest2()
        {
            Console.WriteLine(client.BaseUrl);
            JObject response = new JObject();
            var request = new RestRequest("/something", Method.GET);
            Task.Run(async () =>
                {
                    response = await client.RetrieveRecord(request);

                }).Wait();
            
            Console.WriteLine(response.ToString());
            Assert.True((response.GetValue("StatusCode").ToString() != "OK") );

        }


    }
}
