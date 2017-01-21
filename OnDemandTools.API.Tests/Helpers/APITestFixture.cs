using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using RestSharp;

namespace OnDemandTools.API.Tests.Helpers
{
    public class APITestFixture : IDisposable
    {
        public RestClient restClient { get; set; }
        public IConfigurationRoot Configuration { get; }

        public APITestFixture()
        {
            var builder = new ConfigurationBuilder()
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

    public static class RestClientExtension
    {
        public static Task<T> SubmitRequest<T>(this RestClient client, RestRequest request) where T:new()
        {
            var tcs = new TaskCompletionSource<T>();
            client.ExecuteAsync<T>(request, response =>
            {
                tcs.SetResult(response.Data);
            });

            return tcs.Task;    
        }


        public static Task<String> SubmitRequest(this RestClient client, RestRequest request) 
        {
            var tcs = new TaskCompletionSource<String>();
            client.ExecuteAsync(request, response =>
            {
                tcs.SetResult(response.Content);
            });

            return tcs.Task;
        }
    }
}
