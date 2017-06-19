using System.Collections.Generic;
using System.Linq;
using RestSharp;
using OnDemandTools.Common.Configuration;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using OnDemandTools.Business.Adapters.Titles;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Business.Modules.Airing.Model.Alternate.Title;
using System;

namespace OnDemandTools.Business.Adapters.Titles
{
    public class TitleFinder : ITitleFinder
    {
        private readonly RestClient _client;

        private readonly IFlowTitles _flowTitles;

        public TitleFinder(AppSettings settings, IFlowTitles flowTitles)
        {
            if (settings.PortalSettings != null && !string.IsNullOrEmpty(settings.PortalSettings.TitleSearchApiUrl))
                _client = new RestClient(settings.PortalSettings.TitleSearchApiUrl);

            _flowTitles = flowTitles;
        }

        public IList<Title> Find(string terms)
        {
            var titleIds = GetTitlesIds(terms);

            return _flowTitles.GetFlowTitlesFor(titleIds);
        }

        private IList<int> GetTitlesIds(string terms)
        {
            var request = new RestRequest(string.Format("/select/?q=Type:T Name:({0})&wt=json&rows=3000", terms), Method.GET);
            //var request = new RestRequest(string.Format("/select/?q={0}&wt=json&rows=300", terms), Method.GET);
            //http://titlessolr/live/select?q=the+matrix&wt=json&indent=true&qt=OnDemandTools%2FMultiFieldSearch
            //&qt=Title/MultiFieldSearch

            JObject response = new JObject();

            Task.Run(async () =>
            {
                response = await _client.RetrieveRecord(request);
            }).Wait();


            var json = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseObject>(response.ToString());

            return json.response.docs.Select(d => d.Id).ToList();
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