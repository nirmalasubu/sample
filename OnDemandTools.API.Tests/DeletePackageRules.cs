using OnDemandTools.API.Tests.Helpers;
using RestSharp;
using System;
using System.Net.Http;
using Xunit;
namespace OnDemandTools.API.Tests
{
    [TestCaseOrderer("OnDemandTools.API.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.API.Tests")]
    [Collection("API Collection")]
    public class DeletePackageRules
    {
        APITestFixture fixture;
        RestClient client;
        public DeletePackageRules(APITestFixture fixture)
        {
            this.fixture = fixture;
            this.client = this.fixture.restClient;
        }

        //[Fact, Order(1)]
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

    }
}
