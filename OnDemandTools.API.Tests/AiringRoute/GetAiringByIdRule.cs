using Newtonsoft.Json.Linq;
using OnDemandTools.API.Tests.Helpers;
using RestSharp;
using System;
using System.Threading.Tasks;
using Xunit;


namespace OnDemandTools.API.Tests.AiringRoute
{
    [TestCaseOrderer("OnDemandTools.API.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.API.Tests")]
    [Collection("API Collection")]
    public class GetAiringByIdRule
    {
        APITestFixture fixture;
        RestClient client;
        public GetAiringByIdRule(APITestFixture fixture)
        {
            this.fixture = fixture;
            this.client = this.fixture.restClient;
        }

        [Fact]
        public void GetAiringById_PassingWithValidId_With_NoOptions()
        {

            JObject response = new JObject();
            var request = new RestRequest("/v1/airing/CARE1007291600012447", Method.GET);
            Task.Run(async () =>
            {
                response = await client.RetrieveRecord(request);

            }).Wait();

            string value = response.Value<String>(@"StatusCode");
            if (value != null)
            {
                Assert.True(false, "AiringId : CARE1007291600012447 is deleted");
            }


            // Assert
            Assert.True(response.Value<string>(@"airingId") == "CARE1007291600012447", string.Format("Model Airing Id should be 'CARE1007291600012447' and but the returned {0}", response.Value<string>(@"airingId")));
            Assert.True(response.Value<string>(@"type") == "Feature Film", string.Format("Type should be 'Feature Film' and but the returned {0}", response.Value<string>(@"type")));
            Assert.True(response.Value<string>(@"brand") == "Cartoon", string.Format("Brand should be 'Cartoon' and but the returned {0}", response.Value<string>(@"brand")));
            Assert.True(response.Value<string>(@"platform") == "Broadband", string.Format("Platform should be 'Broadband' and but the returned {0}", response.Value<string>(@"platform")));


        }

        [Fact]
        public void GetAiringById_PassingWithInValidId_With_NoOptions()
        {

            JObject response = new JObject();
            var request = new RestRequest("/v1/airing/CARE1007291600012446", Method.GET);
            Task.Run(async () =>
            {
                response = await client.RetrieveRecord(request);

            }).Wait();

            string value = response.Value<string>(@"StatusCode");
            if (value != null)
            {
                Assert.True(true, "AiringId : CARE1007291600012447 is deleted");
            }


        }

        [Fact]
        public void GetAiringById_PassingValidId_With_Title()
        {
            JObject response = new JObject();
            var request = new RestRequest("/v1/airing/CARE1007291600012447?options=title", Method.GET);
            Task.Run(async () =>
            {
                response = await client.RetrieveRecord(request);

            }).Wait();

            string value = response.Value<string>(@"StatusCode");
            if (value != null)
            {
                Assert.True(false, "AiringId : CARE1007291600012447 is deleted");
            }
            var titleToken = response[@"options"]["titles"];

            Assert.NotNull(titleToken.First);


        }

        [Fact]
        public void GetAiringById_PassingValidId_With_series()
        {
            JObject response = new JObject();
            var request = new RestRequest("/v1/airing/CARE1007291600012447?options=series", Method.GET);
            Task.Run(async () =>
            {
                response = await client.RetrieveRecord(request);

            }).Wait();

            // Assert 
            string value = response.Value<String>(@"StatusCode");
            if (value != null)
            {
                Assert.True(false, "AiringId : CARE1007291600012447 is deleted");
            }
            var seriesToken = response[@"options"]["series"];
            Assert.Null(seriesToken.First);


        }

        [Fact]
        public void GetAiringById_PassingValidId_With_Files()
        {
            JObject response = new JObject();
            var request = new RestRequest("/v1/airing/CARE1007291600012447?options=file", Method.GET);
            Task.Run(async () =>
            {
                response = await client.RetrieveRecord(request);

            }).Wait();

            // Assert  
            string v = response.Value<String>(@"StatusCode");
            if (v != null)
            {
                Assert.True(false, "AiringId : CARE1007291600012447 is deleted");
            }
            var fileToken = response[@"options"]["files"];
            Assert.NotNull(fileToken.First);

        }

        [Fact]
        public void GetAiringById_PassingInValidId_With_seriesandFiles()
        {
            JObject response = new JObject();
            var request = new RestRequest("/v1/airing/CARE1007291600012447?options=file|series", Method.GET);
            Task.Run(async () =>
            {
                response = await client.RetrieveRecord(request);

            }).Wait();

            //Airing model = response.ToObject<Airing>();
            string v = response.Value<String>(@"StatusCode");
            if (v != null)
            {
                Assert.True(true, "AiringId : CARE1007291600012447 is deleted");
            }

        }

        [Fact]
        public void GetAiringById_PassingValidId_With_seriesandFiles_returnsEmptySeries()
        {
            JObject response = new JObject();
            var request = new RestRequest("/v1/airing/CARE1007291600012447?options=file|series", Method.GET);
            Task.Run(async () =>
            {
                response = await client.RetrieveRecord(request);

            }).Wait();

            // Assert
            string v = response.Value<String>(@"StatusCode");
            if (v != null)
            {
                Assert.True(false, "AiringId : CARE1007291600012447 is deleted");
            }
            var fileToken = response[@"options"]["files"];
            var seriesToken = response[@"options"]["series"];
            Assert.NotNull(fileToken.First);
            Assert.Null(seriesToken.First);

        }

        [Fact]
        public void GetAiringById_PassingValidId_With_DestinationAndProperties()
        {
            JObject response = new JObject();
            var request = new RestRequest("/v1/airing/CARE1007291600012449?options=destination", Method.GET);
            Task.Run(async () =>
            {
                response = await client.RetrieveRecord(request);

            }).Wait();

            // Assert  
            string v = response.Value<String>(@"StatusCode");
            if (v != null)
            {
                Assert.True(false, "AiringId : CARE1007291600012449 is deleted");
            }
            var destinationToken = response[@"options"]["destinations"];            
            var flightsDestinationToken = response[@"flights"].First["destinations"];
            var propertiesToken = flightsDestinationToken.First["properties"];
            var deliverablesToken = flightsDestinationToken.First["deliverables"];

            Assert.True((flightsDestinationToken.First["name"].ToString() == "DISHB"),
                string.Format("Destination should be " + flightsDestinationToken.First["name"].ToString() + "and but the returned {0}", flightsDestinationToken.First["name"].ToString()));
            Assert.True((propertiesToken.First["name"].ToString() == "Description"), 
                string.Format("Property name should be "+ propertiesToken.First["name"].ToString() + "and but the returned {0}", propertiesToken.First["name"].ToString()));
            Assert.True((propertiesToken.First["value"].ToString() == "Dish for Broadband"), 
                string.Format("Property value should be " + propertiesToken.First["value"].ToString() + "and but the returned {0}", propertiesToken.First["value"].ToString()));
            
            Assert.Null(deliverablesToken.First);
        }

    }
}
