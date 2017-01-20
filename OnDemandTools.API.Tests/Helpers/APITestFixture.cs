using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace OnDemandTools.API.Tests.Helpers
{
    public class APITestFixture : IDisposable
    {
        public HttpClient RestClient { get; set; }
        public IConfigurationRoot Configuration { get; }

        public APITestFixture()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            RestClient = new HttpClient();
            RestClient.BaseAddress = new Uri(Configuration["APIEndPoint"]);
            RestClient.DefaultRequestHeaders.Accept.Clear();
            RestClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
           
        }

        public void Dispose()
        {
            RestClient.Dispose();
        }
    }
}
