using System.Collections.Generic;
using System.Linq;
using RestSharp;
using OnDemandTools.Common.Configuration;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using OnDemandTools.Business.Adapters.Titles;

namespace OnDemandTools.Business.Adapters.Titles
{
    public class TitleFinder : ITitleFinder
    {
        private readonly RestClient _client;

        public TitleFinder(AppSettings settings)
        {
            if (settings.PortalSettings != null && !string.IsNullOrEmpty(settings.PortalSettings.TitleSearchApiUrl))
                _client = new RestClient(settings.PortalSettings.TitleSearchApiUrl);
        }

        public IList<int> Find(string terms)
        {
            var request = new RestRequest(string.Format("/select/?q=Type:T Name:({0})&wt=json&rows=300", terms), Method.GET);
            //var request = new RestRequest(string.Format("/select/?q={0}&wt=json&rows=300", terms), Method.GET);
            //http://titlessolr/live/select?q=the+matrix&wt=json&indent=true&qt=OnDemandTools%2FMultiFieldSearch
            //&qt=Title/MultiFieldSearch

            JObject response = new JObject();

            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);
            }).Wait();
            
            return new List<int>();
        }
    }

    public class ResponseObject
    {
        public Response response { get; set; }
    }

    public class Response
    {
        public List<Doc> docs { get; set; }
    }

    public class Doc
    {
        public int Id { get; set; }
    }
}