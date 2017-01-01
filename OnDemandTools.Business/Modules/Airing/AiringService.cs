using System;
using System.Collections.Generic;
using OnDemandTools.DAL.Modules.Airings;
using BLModel = OnDemandTools.Business.Modules.Airing.Model;
using DLModel = OnDemandTools.DAL.Modules.Airings.Model;
using System.Linq;
using OnDemandTools.Common.Model;
using OnDemandTools.DAL.Modules.Queue.Queries;
using System.Text;
using OnDemandTools.Common;
using OnDemandTools.DAL.Modules.File.Queries;
using AutoMapper;
using DLFileModel = OnDemandTools.DAL.Modules.File.Model;

namespace OnDemandTools.Business.Modules.Airing
{
    public class AiringService : IAiringService
    {
        IGetAiringQuery airingQueryHelper;
        IAiringSaveCommand airingSaveCommandHelper;
        IAiringDeleteCommand airingDeleteCommandHelper;
        IAiringMessagePusher airingMessagePusherCommandHelper;
        IQueueQuery queueQueryHelper;
        ITaskUpdater taskUpdaterCommand;
        IFileQuery fileQueryHelper;

        public AiringService(IGetAiringQuery airingQueryHelper, 
            IAiringSaveCommand airingSaveCommandHelper,
            IAiringDeleteCommand airingDeleteCommandHelper, IAiringMessagePusher airingMessagePusherCommandHelper, IQueueQuery queueQueryHelper,
            ITaskUpdater taskUpdaterCommand,
            IFileQuery fileQueryHelper)
        {
            this.airingQueryHelper = airingQueryHelper;
            this.airingSaveCommandHelper = airingSaveCommandHelper;
            this.airingDeleteCommandHelper = airingDeleteCommandHelper;
            this.airingMessagePusherCommandHelper = airingMessagePusherCommandHelper;
            this.queueQueryHelper = queueQueryHelper;
            this.taskUpdaterCommand = taskUpdaterCommand;
            this.fileQueryHelper = fileQueryHelper;
        }

        public BLModel.Airing Delete(BLModel.Airing airing)
        {
            return
            airingDeleteCommandHelper.Delete(airing.ToDataModel<BLModel.Airing, DLModel.Airing>())
                .ToBusinessModel<DLModel.Airing, BLModel.Airing>();
        }

        public BLModel.Airing GetBy(string assetId, AiringCollection getFrom = AiringCollection.CurrentOrExpiredCollection)
        {
            return 
            airingQueryHelper.GetBy(assetId, getFrom)
                .ToBusinessModel<DLModel.Airing, BLModel.Airing>();
        }

        public List<BLModel.Airing> GetByMediaId(string mediaId)
        {
            return
            (airingQueryHelper.GetByMediaId(mediaId).ToList<DLModel.Airing>()
                .ToBusinessModel<List<DLModel.Airing>, List<BLModel.Airing>>());

        }

        public List<Model.Airing> GetNonExpiredBy(int titleId, DateTime cutOffDateTime, bool isSeries = false)
        {
            return
            (airingQueryHelper.GetNonExpiredBy(titleId, cutOffDateTime, isSeries).ToList<DLModel.Airing>()
                .ToBusinessModel<List<DLModel.Airing>, List<BLModel.Airing>>());
        }

        public bool IsAiringExists(string assetId)
        {
            return airingQueryHelper.IsAiringExists(assetId);
        }

        public BLModel.Airing Save(BLModel.Airing airing, bool hasImmediateDelivery, bool updateHistorical)
        {
            return
            (airingSaveCommandHelper.Save(airing.ToDataModel<BLModel.Airing, DLModel.Airing>(), hasImmediateDelivery, updateHistorical)
                .ToBusinessModel<DLModel.Airing, BLModel.Airing>());
        }

        public void PushToQueue(string queueName, IList<string> airingIds)
        {
            airingMessagePusherCommandHelper.PushBy(queueName, airingIds);
        }

        public void PushToQueues(IList<string> airingIds)
        {
            var queues = queueQueryHelper
                .Get()
                .Where(q => q.Active);

            foreach (var deliveryQueue in queues)
            {
                airingMessagePusherCommandHelper.PushBy(deliveryQueue.Name, deliveryQueue.Query, deliveryQueue.HoursOut, airingIds);
            }

        }

        public void AugmentMediaId(ref BLModel.Airing airing)
        {
            var contentIds = new StringBuilder();

            foreach (var version in airing.Versions)
            {
                contentIds.Append(version.ContentId);
            }

            airing.MediaId = IdGenerator.Generate(string.Concat(contentIds.ToString(), airing.Network));
        }

        public void UpdateTask(List<string> airingIds, List<string> tasks)
        {
            taskUpdaterCommand.UpdateFor(airingIds, tasks);
        }

        public List<BLModel.Airing> GetAiringsByMediaId(string mediaId, DateTime startDate, DateTime endDate)
        {
            return
                (airingQueryHelper.GetAiringsByMediaId(mediaId, startDate, endDate)
                .ToList<DLModel.Airing>()
                .ToBusinessModel<List<DLModel.Airing>, List<BLModel.Airing>>());
        }

        public List<BLModel.Airing> GetNonExpiredBy(string destination, DateTime cutOffDateTime)
        {
            return
                (airingQueryHelper.GetNonExpiredBy(destination, cutOffDateTime)
                .ToList<DLModel.Airing>()
                .ToBusinessModel<List<DLModel.Airing>, List<BLModel.Airing>>());
        }

        public List<BLModel.Airing> GetBy(string brand, string destination, DateTime startDate, DateTime endDate, string airingStatus = "")
        {
            return
                (airingQueryHelper.GetBy(brand, destination, startDate, endDate, airingStatus)
                .ToList<DLModel.Airing>()
                .ToBusinessModel<List<DLModel.Airing>, List<BLModel.Airing>>());
        }

        public List<BLModel.Alternate.Long.File> RetrieveFile(BLModel.Alternate.Long.Airing airing)
        {
            if (String.IsNullOrWhiteSpace(airing.MediaId))
            {
                airing.MediaId = String.Empty;
            }

            var titleIds = ExtractTitleAndSeriesIdsFrom(airing);
            var versionIds = ExtractVersionIdsFrom(airing);
            var files = fileQueryHelper.GetBy(versionIds, titleIds, airing.AiringId, airing.MediaId).ToList();            
            return Mapper.Map<List<DLFileModel.File>, List<BLModel.Alternate.Long.File>>(files);            
        }


        private List<string> ExtractVersionIdsFrom(BLModel.Alternate.Long.Airing airing)
        {
            return airing.Versions
                .Select(t => t.ContentId)
               .ToList();
        }

        private static List<int> ExtractTitleAndSeriesIdsFrom(BLModel.Alternate.Long.Airing airing)
        {
            var titleIds = airing.Title.TitleIds
                .Where(t => t.Authority == "Turner" && t.Value != null)
                .Select(t => int.Parse(t.Value))
                .ToList();

            if (airing.Title.Series.Id.HasValue)
            {
                titleIds.Add(airing.Title.Series.Id.Value);
            }

            return titleIds;
        }
    }
}
