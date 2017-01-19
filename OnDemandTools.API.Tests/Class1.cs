using RestSharp;
using System;
using System.Threading.Tasks;
using Xunit;

namespace OnDemandTools.API.Tests
{
    // see example explanation on xUnit.net website:
    // https://xunit.github.io/docs/getting-started-dotnet-core.html
    public class Class1
    {
        [Fact]
        public void PassingTest()
        {
            RestClient client = new RestClient("http://localhost:5000/whoami");
            var request = new RestRequest(Method.GET);
            Task.Run(async () =>
            {
                var rs = await GetHostingProviderDetails(client, request) as String;
                var kk = 4;

            }).Wait();
        }

        private Task<String> GetHostingProviderDetails(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<String>();
            theClient.ExecuteAsync(theRequest, response => {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    tcs.SetResult(response.Content);
                }
                else
                {
                    tcs.SetResult(String.Empty);
                }

            });
            return tcs.Task;
        }


        int Add(int x, int y)
        {
            return x + y;
        }
    }
}
