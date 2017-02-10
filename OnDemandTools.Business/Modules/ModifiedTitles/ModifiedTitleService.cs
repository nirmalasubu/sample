using OnDemandTools.DAL.Modules.ModifiedTitles;
using OnDemandTools.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp;
using BLModel = OnDemandTools.Business.Modules.ModifiedTitles.Model;
using DQModel = OnDemandTools.DAL.Modules.Queue.Model;
using System.Threading.Tasks;
using OnDemandTools.Common.Extensions;

namespace OnDemandTools.Business.Modules.ModifiedTitles
{
    public class ModifiedTitleService: IModifiedTitlesService
    {
        private readonly IResetDeliveryCommand _resetDeliveryCommand;
        private readonly ITitleIdsQuery _titlIdsQuery;
        private readonly ITitleIdsCommand _titleIDsCommand;
        private readonly RestClient _client;
        AppSettings appSettings;

        public ModifiedTitleService(IResetDeliveryCommand resetDeliveryCommand,
            ITitleIdsQuery titlIdsQuery,
            ITitleIdsCommand titleIDsCommand)
        {
            _resetDeliveryCommand = resetDeliveryCommand;
            _titlIdsQuery = titlIdsQuery;
            _titleIDsCommand = titleIDsCommand;
            _client = new RestClient(appSettings.GetExternalService("Flow").Url);
        }

        public String Update(IQueryable<DQModel.Queue> queues, String sinceTitleBSONId, int limit)
        {

            //"Retrieving all titles from Flow that were modified since sinceTitleBSONId
            List<BLModel.UpdatedTitle> modifiedTitles = GetTitleIdsModifiedAfter(sinceTitleBSONId);
            List<int> modifiedTitleIds = new List<int>();
            if (!modifiedTitles.IsNullOrEmpty())
            {
                sinceTitleBSONId = modifiedTitles.Last()._id;
                modifiedTitleIds = modifiedTitles.Select(c => c.TitleId).ToList();
            }

            //"Saving modified titles in ODT.
            _titleIDsCommand.Save(modifiedTitleIds);

            //"Retrieving modified titles that were saved in ODT"
            var titleIds = _titlIdsQuery.Get(limit).ToList();

            //"Preparing to send modified titles to queues"
            var queueNames = queues.Select(q => q.Name);
            _resetDeliveryCommand.ResetFor(queueNames, titleIds);

            //"Deleting previously persisted modified titles in ODT"
            _titleIDsCommand.Delete(titleIds);

            return sinceTitleBSONId;
        }
        
        public List<BLModel.UpdatedTitle> GetTitleIdsModifiedAfter(string titleId)
        {
            // Retrieve titles modified since the date on which the given
            // titleid was modified
            var request = new RestRequest("/v2/title/since/{objectid}?api_key={api_key}", Method.GET);
            request.AddUrlSegment("objectid", titleId);
            request.AddUrlSegment("api_key", appSettings.GetExternalService("Flow").ApiKey);
            var updatedTitles = new List<BLModel.UpdatedTitle>();

            Task.Run(async () =>
            {
                var rs = await GetTitleIdsModifiedAfterAsync(_client, request) as List<BLModel.UpdatedTitle>;
                if (!rs.IsNullOrEmpty())
                {
                    updatedTitles.AddRange(rs);
                }
                                
            }).Wait();

            return updatedTitles;
        }
        
        public string GetLastModifiedTitleIdOnOrBefore(DateTime lastTitleProcessedDateTime)
        {
            var utcDateTime = lastTitleProcessedDateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

            var request = new RestRequest("/v2/title/maxbefore/" + utcDateTime + "?api_key={api_key}", Method.GET);
            request.AddUrlSegment("api_key", appSettings.GetExternalService("Flow").ApiKey);
            var updatedTitles = new List<BLModel.UpdatedTitle>();

            Task.Run(async () =>
            {
                var rs = await GetLastModifiedTitleIdOnOrBeforeAsync(_client, request) as List<BLModel.UpdatedTitle>;
                if (!rs.IsNullOrEmpty())
                {
                    updatedTitles.AddRange(rs);
                }

            }).Wait();

            if (updatedTitles != null && updatedTitles.Any())
            {
                return updatedTitles.First()._id;
            }

            return string.Empty;
        }


        #region "Private methods"

        private Task<List<BLModel.UpdatedTitle>> GetTitleIdsModifiedAfterAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<List<BLModel.UpdatedTitle>>();
            theClient.ExecuteAsync<List<BLModel.UpdatedTitle>>(theRequest, response => {
                tcs.SetResult(response.Data);
            });
            return tcs.Task;
        }

        private Task<List<BLModel.UpdatedTitle>> GetLastModifiedTitleIdOnOrBeforeAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<List<BLModel.UpdatedTitle>>();
            theClient.ExecuteAsync<List<BLModel.UpdatedTitle>>(theRequest, response => {
                tcs.SetResult(response.Data);
            });
            return tcs.Task;
        }

        #endregion
    }
}