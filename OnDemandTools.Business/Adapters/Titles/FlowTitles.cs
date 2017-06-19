using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnDemandTools.Business.Modules.Airing.Model.Alternate.Title;
using OnDemandTools.Common.Configuration;
using OnDemandTools.Common.Extensions;
using RestSharp;

namespace OnDemandTools.Business.Adapters.Titles
{
    public class FlowTitles : IFlowTitles
    {
        AppSettings _appSettings;
        public FlowTitles(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public List<Title> GetFlowTitlesFor(IEnumerable<int> titleIds)
        {
            // The Distinct() here is neccessary because of a limitation we discovered.
            // The titles API returns only distinct FlowTitle objects per request.
            // If there are more then 5 titles (because of the partition), you can potentially
            // end up with duplicates. (Titles API limits us to 25. Consult titles api wiki.)
            var listsOfTitleIds = titleIds.Distinct().Partition(25).ToList();
            RestClient client = new RestClient(_appSettings.GetExternalService("Flow").Url);
            var titles = new List<Title>();

            foreach (var list in listsOfTitleIds)
            {
                var request = new RestRequest("/v2/title/{ids}?api_key={api_key}", Method.GET);
                request.AddUrlSegment("ids", string.Join(",", list));
                request.AddUrlSegment("api_key", _appSettings.GetExternalService("Flow").ApiKey);

                Task.Run(async () =>
                {
                    var rs = await GetFlowTitleAsync(client, request) as List<Title>;
                    if (!rs.IsNullOrEmpty())
                    {
                        titles.AddRange(rs);
                    }

                }).Wait();
            }

            return titles;
        }

        private Task<List<Title>> GetFlowTitleAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<List<Title>>();
            theClient.ExecuteAsync<List<Title>>(theRequest, response =>
            {
                tcs.SetResult(response.Data);
            });
            return tcs.Task;
        }
    }
}
