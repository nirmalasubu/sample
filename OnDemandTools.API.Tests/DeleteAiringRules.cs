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

        [Fact, Order(2)]       
        public void PassingTest()
        {

            Console.WriteLine(client.BaseUrl);
            String response = String.Empty;
            var request = new RestRequest("/whoami", Method.GET);
            Task.Run(async () =>
                {
                    response = await client.SubmitRequest(request) as String;

                }).Wait(); 
            
            Console.WriteLine(response);
            Assert.True(!String.IsNullOrEmpty(response));

        }
        


        [Fact, Order(1)]       
        public void FailingTest()
        {
            Console.WriteLine(client.BaseUrl);
            String response = String.Empty;
            var request = new RestRequest("/something", Method.GET);
            Task.Run(async () =>
                {
                    response = await client.SubmitRequest(request) as String;

                }).Wait();
            
            Console.WriteLine(response);
            Assert.True(!String.IsNullOrEmpty(response));

        }


    }
}
