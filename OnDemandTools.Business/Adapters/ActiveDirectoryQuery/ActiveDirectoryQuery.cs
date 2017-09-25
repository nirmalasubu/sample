using Newtonsoft.Json.Linq;
using OnDemandTools.Common.Configuration;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Adapters.ActiveDirectoryQuery
{
    public class ActiveDirectoryQuery : IActiveDirectoryQuery
    {
        AppSettings _appSettings;
        string _authToken;

        public ActiveDirectoryQuery(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public AzureAdUser GetUserByEmailId(string email)
        {
            if (string.IsNullOrEmpty(_authToken))
                _authToken = GetToken();

            return GetUserByEmail(_authToken, email);
        }

        private AzureAdUser GetUserByEmail(string token, string email)
        {

            // Sample requests are available in below documentation 
            // https://developer.microsoft.com/en-us/graph/graph-explorer

            RestClient client = new RestClient("https://graph.microsoft.com");
            RestRequest request = new RestRequest("/v1.0/users/" + email, Method.GET);


            request.AddHeader("Authorization", "Bearer " + token);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            JObject response = new JObject();

            Task.Run(async () =>
            {
                response = await client.RetrieveRecord(request);
            }).Wait();

            var apiStatusCode = response.Value<string>(@"StatusCode");

            if (!string.IsNullOrEmpty(apiStatusCode) && apiStatusCode.Equals("NotFound"))
            {
                return null; //User not found in Azure AD
            }

            var user = Newtonsoft.Json.JsonConvert.DeserializeObject<AzureAdUser>(response.ToString());

            return user;

        }

        public string GetToken()
        {
            RestClient client = new RestClient("https://login.microsoftonline.com");
            RestRequest request = new RestRequest(string.Format("/{0}/oauth2/token", _appSettings.AzureAd.Tenant), Method.POST);

            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("client_id", _appSettings.AzureAd.ClientId);
            request.AddParameter("client_secret", _appSettings.AzureAd.ClientSecret);
            request.AddParameter("resource", "https://graph.microsoft.com/");

            JObject response = new JObject();

            Task.Run(async () =>
            {
                response = await client.RetrieveRecord(request);
            }).Wait();

            string authToken = response.Value<String>(@"access_token");

            if (string.IsNullOrEmpty(authToken))
            {
                throw new Exception("Unable to get Auth token from Azure AD");
            }

            return authToken;
        }
    }
}
