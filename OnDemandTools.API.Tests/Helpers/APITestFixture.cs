using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Net;

namespace OnDemandTools.API.Tests.Helpers
{
    public class APITestFixture : IDisposable
    {
        public RestClient restClient { get; set; }
        public IConfigurationRoot Configuration { get; set; }
        public IConfigurationBuilder builder { get; set; }

        public APITestFixture()
        {
            BuildAPISettings();
            restClient.AddDefaultHeader("Authorization", Configuration["TesterAPIKey"]);
        }

        public APITestFixture(string apiKeyName)
        {
            BuildAPISettings();
            restClient.AddDefaultHeader("Authorization", Configuration[apiKeyName]);
        }

        private void BuildAPISettings()
        {
            builder = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            restClient = new RestClient(Configuration["APIEndPoint"]);
            restClient.AddDefaultHeader("Content-Type", "application/json");
        }

        public void Dispose()
        {
            restClient = null;
        }
    }

   
}
