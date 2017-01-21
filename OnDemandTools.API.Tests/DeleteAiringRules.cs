using OnDemandTools.API.Tests.Helpers;
using RestSharp;
using System;
using System.Net.Http;
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

        //[Fact, Order(2)]       
        //public async void PassingTest()
        //{

        //    Console.WriteLine(client.BaseAddress);
        //    String details = String.Empty;
        //    HttpResponseMessage response = await client.GetAsync("/whoami");
        //    if (response.IsSuccessStatusCode)
        //    {
        //        details = await response.Content.ReadAsStringAsync();
        //    }

        //    Console.WriteLine(details);
        //    Assert.True(!String.IsNullOrEmpty(details));
        //}


        [Fact, Order(1)]       
        public void FailingTest()
        {
            Console.WriteLine(client.BaseUrl);
            String response = String.Empty;

            try
            {
                var request = new RestRequest("/whoami", Method.GET);
                Task.Run(async () =>
                {
                    response = await client.SubmitRequest(request) as String;

                }).Wait();               

            }
            catch (Exception)
            {

            }
            
            Console.WriteLine(response);
            Assert.True(!String.IsNullOrEmpty(response));

        }


    }
}
