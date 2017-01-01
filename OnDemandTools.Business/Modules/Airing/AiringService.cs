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

        public AiringService(IGetAiringQuery airingQueryHelper, 
            IAiringSaveCommand airingSaveCommandHelper,
            IAiringDeleteCommand airingDeleteCommandHelper, IAiringMessagePusher airingMessagePusherCommandHelper, IQueueQuery queueQueryHelper,
            ITaskUpdater taskUpdaterCommand)
        {
            this.airingQueryHelper = airingQueryHelper;
            this.airingSaveCommandHelper = airingSaveCommandHelper;
            this.airingDeleteCommandHelper = airingDeleteCommandHelper;
            this.airingMessagePusherCommandHelper = airingMessagePusherCommandHelper;
            this.queueQueryHelper = queueQueryHelper;
            this.taskUpdaterCommand = taskUpdaterCommand;
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
    }
}
