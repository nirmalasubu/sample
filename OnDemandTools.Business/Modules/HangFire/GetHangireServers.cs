using OnDemandTools.Business.Modules.HangFire.Model;
using OnDemandTools.Common.Configuration;
using OnDemandTools.Common.Extensions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.HangFire
{
    public class GetHangireServers:IGetHangireServers
    {
       
        private readonly RestClient _client;

        public GetHangireServers(AppSettings appsettings)
        {
            _client = new RestClient(appsettings.Hangfire.Url);
        }

        public List<HangfireServerModel> Get()
        {
            RestRequest request = new RestRequest("/hangfireservers", Method.GET);
            List<HangfireServerModel> servers = new List<HangfireServerModel> ();
            Task.Run(async () =>
            {
                var rs = await GetServersAsync(_client, request) as List<HangfireServerModel>;
                if (!rs.IsNullOrEmpty())
                {
                    servers.AddRange(rs);
                }

            }).Wait();

            return (servers);
        }

        public HangFireStatusModel GetStatus()
        {
            var servers = Get();

            var currentTime = DateTime.UtcNow.AddMinutes(-1);

            var activeServers = servers.Where(e => e.Heartbeat > currentTime).ToList();

            return new HangFireStatusModel
            {
                Count = activeServers.Count,
                LastHeartbeat = activeServers.Max(e => e.Heartbeat)
            };
        }

        private Task<List<HangfireServerModel>> GetServersAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<List<HangfireServerModel>>();
            theClient.ExecuteAsync<List<HangfireServerModel>>(theRequest, response =>
            {
                tcs.SetResult(response.Data);
            });
            return tcs.Task;
        }
    }
}
