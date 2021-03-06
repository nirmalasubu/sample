﻿using OnDemandTools.Common.Model;
using OnDemandTools.DAL.Modules.Queue.Command;
using OnDemandTools.DAL.Modules.Queue.Queries;
using System.Collections.Generic;
using System.Linq;
using BLModel = OnDemandTools.Business.Modules.Queue.Model;
using DLModel = OnDemandTools.DAL.Modules.Queue.Model;
using OnDemandTools.DAL.Modules.QueueMessages;
using OnDemandTools.DAL.Modules.QueueMessages.Model;
using OnDemandTools.DAL.Modules.QueueMessages.Commands;
using System;
using OnDemandTools.DAL.Modules.Airings;
using OnDemandTools.DAL.Helpers;
using OnDemandTools.DAL.Modules.Airings.Queries;
using OnDemandTools.Business.Adapters.Hangfire;
using OnDemandTools.Business.Modules.AiringPublisher.Workflow;
using MongoDB.Bson;

namespace OnDemandTools.Business.Modules.Queue
{
    public class QueueService : IQueueService
    {

        IQueueQuery queueQueryHelper;
        IQueueCommand queueCommandHelper;
        IQueueLocker queueLocker;
        IGetQueueMessagesQuery queueMessages;
        IQueueMessageRecorder historyRecorder;
        IGetAiringQuery airingQueryHelper;
        IAiringMessagePusherQueueApi messagePusher;
        IQueueSaveCommand queueSaveCommand;
        CurrentAiringsQuery currentAiringQuery;
        IHangfireRecurringJobCommand hangfireCommand;
        IRemoteQueueHandler remoteQueueHandler;
        IQueueDeleteCommand queueDeleteCommand;

        public QueueService(
            IQueueQuery queueQueryHelper,
            IQueueCommand queueCommandHelper,
            IQueueLocker queueLocker,
            IGetQueueMessagesQuery queueMessages,
            IQueueMessageRecorder historyRecorder,
            IGetAiringQuery airingQueryHelper,
            IAiringMessagePusherQueueApi messagePusher,
            IQueueSaveCommand queueSaveCommand,
            CurrentAiringsQuery currentAiringQuery,
            IHangfireRecurringJobCommand hangfireCommand,
            IRemoteQueueHandler remoteQueueHandler,
            IQueueDeleteCommand queueDeleteCommand)
        {
            this.queueQueryHelper = queueQueryHelper;
            this.queueCommandHelper = queueCommandHelper;
            this.queueLocker = queueLocker;
            this.queueMessages = queueMessages;
            this.historyRecorder = historyRecorder;
            this.airingQueryHelper = airingQueryHelper;
            this.messagePusher = messagePusher;
            this.queueSaveCommand = queueSaveCommand;
            this.currentAiringQuery = currentAiringQuery;
            this.hangfireCommand = hangfireCommand;
            this.remoteQueueHandler = remoteQueueHandler;
            this.queueDeleteCommand = queueDeleteCommand;
        }

        /// <summary>
        /// Flags the given list of queues for redelivery
        /// </summary>
        /// <param name="queueNames">The queue names.</param>
        /// <param name="titleIds">The title ids.</param>
        /// <param name="destinationCode">The destination code.</param>
        public void FlagForRedelivery(IList<string> queueNames, IList<int> titleIds, string destinationCode)
        {
            queueCommandHelper.ResetFor(queueNames, titleIds, destinationCode);
        }

        /// <summary>
        /// Flags the given list of queues for redelivery
        /// </summary>
        /// <param name="queueNames">The queue names.</param>
        /// <param name="contentIds">The content ids.</param>
        /// <param name="destinationCode">The destination code.</param>
        public void FlagForRedelivery(IList<string> queueNames, IList<string> contentIds, string destinationCode)
        {
            queueCommandHelper.ResetFor(queueNames, contentIds, destinationCode);
        }

        /// <summary>
        /// Flags the given list of queues for redelivery
        /// </summary>
        /// <param name="queueNames">The queue names</param>
        /// <param name="airingId">The airing id</param>
        /// <param name="destinationCode">The destination code</param>
        public void FlagForRedelivery(IList<string> queueNames, string airingId, string destinationCode)
        {
            queueCommandHelper.ResetFor(queueNames, airingId, destinationCode);
        }

        /// <summary>
        /// Updates Queue processing date with current time
        /// </summary>
        /// <param name="name"></param>
        public void UpdateQueueProcessedTime(string name)
        {
            queueCommandHelper.UpdateQueueProcessedTime(name);
        }


        /// <summary>
        /// Retrieves those queues that match the provided
        /// active flag (true/false)
        /// </summary>
        /// <param name="active">if set to <c>true</c> [active].</param>
        /// <returns></returns>
        public List<Model.Queue> GetByStatus(bool active)
        {
            return
            (queueQueryHelper.GetByStatus(active).ToList<DLModel.Queue>()
                .ToBusinessModel<List<DLModel.Queue>, List<BLModel.Queue>>());

        }

        /// <summary>
        /// Retrieves those queues that are subscribed to receive package notification
        /// </summary>
        /// <returns></returns>
        public List<Model.Queue> GetPackageNotificationSubscribers()
        {
            return
                (queueQueryHelper.GetPackageQueues().ToList<DLModel.Queue>()
                .ToBusinessModel<List<DLModel.Queue>, List<BLModel.Queue>>());
        }

        /// <summary>
        /// Retrieves those queues that are subscribed to receive status notification
        /// </summary>
        /// <returns></returns>
        public List<Model.Queue> GetStatusNotificationSubscribers()
        {
            return
                (queueQueryHelper.GetStatusQueues().ToList<DLModel.Queue>()
                .ToBusinessModel<List<DLModel.Queue>, List<BLModel.Queue>>());
        }

        /// <summary>
        /// Flags the given list of queues for redelivery. Assets selected for delivery
        /// depends on 'titleIds'
        /// </summary>
        /// <param name="queueNames">The queue names.</param>
        /// <param name="titleIds">The title ids.</param>
        /// <param name="changeNotificationType">the file/title</param>
        public void FlagForRedelivery(IList<string> queueNames, IList<int> titleIds, ChangeNotificationType changeNotificationType)
        {
            queueCommandHelper.ResetFor(queueNames, titleIds, changeNotificationType);
        }

        /// <summary>
        /// Flags the given list of queues for redelivery. Assets selected for delivery
        /// depends on 'airingIds'
        /// </summary>
        /// <param name="queueNames">The queue names.</param>
        /// <param name="airingIds">The airing ids.</param>
        /// <param name="changeNotificationType">the file/title</param>
        public void FlagForRedelivery(IList<string> queueNames, IList<string> airingIds, ChangeNotificationType changeNotificationType)
        {
            queueCommandHelper.ResetFor(queueNames, airingIds, changeNotificationType);
        }

        /// <summary>
        /// Flags the given queue for redelivery. Assets selected for delivery
        /// depends on 'queueName'
        /// </summary>
        /// <param name="queueName">The queue name.</param>
        public void FlagForRedelivery(string queueName)
        {
            queueCommandHelper.ResetFor(queueName);
        }

        /// <summary>
        /// Flags re deliver for given queue name and airing id
        /// </summary>
        /// <param name="queueName">the queue name</param>
        /// <param name="airingId">the airing Id</param>
        public void FlagForRedelivery(string queueName, string airingId)
        {
            queueCommandHelper.ResetFor(queueName, airingId);
        }

        public BLModel.Queue GetByApiKey(string apiKey)
        {
            return
            queueQueryHelper.GetByApiKey(apiKey)
                .ToBusinessModel<DLModel.Queue, BLModel.Queue>();

        }

        /// <summary>
        /// Locks the queue
        /// </summary>
        /// <param name="queueName">queue name to lock</param>
        /// <param name="processorId">processor id to lock</param>
        /// <returns>Returns true if locked sucessfully</returns>
        public bool Lock(string queueName, string processId)
        {
            var qLock = queueLocker.AquireLockFor(queueName, processId);
            return qLock.IsLockedBy(processId);
        }

        /// <summary>
        /// Unlocks the queue
        /// </summary>
        /// <param name="queueName">queue name to lock</param>
        /// <param name="processorId">processor id to lock</param>
        public void Unlock(string queueName, string processId)
        {
            queueLocker.ReleaseLockFor(queueName, processId);
        }

        /// <summary>
        /// Unlocks the queue
        /// </summary>
        /// <param name="queueName">queue name to lock</param>
        public void Unlock(string queueName)
        {
            queueLocker.ReleaseLockFor(queueName, null);
        }

        /// <summary>
        /// Check and returns any message delived for given Queue and MediaId
        /// </summary>
        /// <param name="mediaId">media id to check</param>
        /// <param name="queueName">queue name to check</param>
        public bool AnyMessageDeliveredForMediaId(string mediaId, string queueName)
        {
            return queueMessages.GetByMediaId(mediaId, queueName).Any();
        }

        /// <summary>
        /// Check and returns any message delived for given Queue and AiringId
        /// </summary>
        /// <param name="airingId">media id to check</param>
        /// <param name="queueName">queue name to check</param>
        public bool AnyMessageDeliveredForAiringId(string airingId, string queueName)
        {
            return queueMessages.GetBy(queueName, airingId).Any();
        }

        /// <summary>
        /// Adds the historical message for the queue delivery
        /// </summary>
        /// <param name="airingId">the airing id</param>
        /// <param name="mediaId">the media id</param>
        /// <param name="message">the message</param>
        /// <param name="remoteQueueName">the queue name</param>
        /// <param name="messagePriority">message priority</param>
        public void AddHistoricalMessage(string airingId, string mediaId, string message, string remoteQueueName, byte? messagePriority)
        {
            var historicalMessage = new HistoricalMessage(airingId, mediaId, message, remoteQueueName, messagePriority);

            historyRecorder.Record(historicalMessage);
        }

        /// <summary>
        /// Deletes messages by media id
        /// </summary>
        /// <param name="mediaId"></param>
        public void DeleteHistoricalMessage(string mediaId)
        {
            historyRecorder.Remove(mediaId);
        }


        /// <summary>
        ///  returns any message delived for given Queue and AiringId
        /// </summary>
        /// <param name="airingId">media id to check</param>
        /// <param name="queueName">queue name to check</param>
        public BLModel.HistoricalMessage GetMessageDeliveredForAiringId(string airingId, string queueName)
        {
            HistoricalMessage historicalMessage = queueMessages.GetBy(queueName, airingId).First();
            return historicalMessage.ToBusinessModel<HistoricalMessage, BLModel.HistoricalMessage>();
        }

        public List<BLModel.HistoricalMessage> GetAllMessagesDeliveredForAiringId(string airingId, string queueName)
        {
            var historicalMessages = queueMessages.GetBy(queueName, airingId).ToList();
            return historicalMessages.ToBusinessModel<List<HistoricalMessage>, List<BLModel.HistoricalMessage>>();
        }

        public List<BLModel.HistoricalMessage> GetAllMessagesDeliveredForAiringId(List<string> airingIds, string queueName)
        {
            var historicalMessages = queueMessages.GetBy(queueName, airingIds).ToList();
            return historicalMessages.ToBusinessModel<List<HistoricalMessage>, List<BLModel.HistoricalMessage>>();
        }

        public List<BLModel.HistoricalMessage> GetTop50MessagesDeliveredForQueue(string queueName)
        {
            var historicalMessages = queueMessages.GetBy(queueName, DateTime.UtcNow.AddDays(-7), 50).ToList();
            return historicalMessages.ToBusinessModel<List<HistoricalMessage>, List<BLModel.HistoricalMessage>>();
        }

        public List<Model.Queue> GetQueues()
        {
            return
            (queueQueryHelper.Get().ToList<DLModel.Queue>()
            .ToBusinessModel<List<DLModel.Queue>, List<BLModel.Queue>>());
        }

        public void ClearPendingDeliveries(string queueName)
        {
            queueCommandHelper.CancelDeliverFor(queueName);
        }

        public List<Model.Queue> PopulateMessageCounts(List<Model.Queue> queues)
        {
            foreach (var queue in queues)
            {
                queue.PendingDeliveryCount = airingQueryHelper.GetPendingDeliveryCountBy(queue.Name);
            }
            return queues;
        }

        /// <summary>
        /// Flags the given queue for redelivery. Assets selected for delivery
        /// depends on 'criteria'
        /// </summary>
        /// <param name="criteria">The Deliver Criteria.</param>
        public bool FlagForRedeliveryByCriteria(BLModel.DeliverCriteria criteria)
        {
            try
            {
                messagePusher.PushBy(criteria.ToDataModel<BLModel.DeliverCriteria, DeliverCriteria>());

                return true;
            }
            catch (QueryValidationException ex)
            {
                return false;
            }

        }

        /// <summary>
        /// Defaults RoutingKey and Queue Name if it not exists
        /// </summary>
        /// <param name="queue">the queue to reset</param>
        private void DefaultQueueKeyFields(Model.Queue queue)
        {
            if (string.IsNullOrWhiteSpace(queue.RoutingKey))
                queue.RoutingKey = Guid.NewGuid().ToString();

            if (string.IsNullOrWhiteSpace(queue.Name))
                queue.Name = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Save's the queue
        /// </summary>
        /// <param name="queue">queue model to save</param>
        /// <returns>Returns saved queue</returns>
        public Model.Queue SaveQueue(Model.Queue queue)
        {
            DefaultQueueKeyFields(queue);

            DLModel.Queue dataModel = queue.ToDataModel<Model.Queue, DLModel.Queue>();

            var prioritySelectionChanged = false;
            var versionSelectionChanged = false;
            var prohibitResendMediaIdChanged = false;

            if (!string.IsNullOrEmpty(dataModel.Name))
            {
                var existingQueueSettings = queueQueryHelper.GetByApiKey(dataModel.Name);

                if (existingQueueSettings != null)
                {
                    prioritySelectionChanged = existingQueueSettings.IsPriorityQueue != dataModel.IsPriorityQueue;
                    versionSelectionChanged = !existingQueueSettings.AllowAiringsWithNoVersion && dataModel.AllowAiringsWithNoVersion;
                    prohibitResendMediaIdChanged = existingQueueSettings.IsProhibitResendMediaId &&
                                               !dataModel.IsProhibitResendMediaId;
                }
            }

            dataModel = queueSaveCommand.Save(dataModel);

            remoteQueueHandler.Create(queue, prioritySelectionChanged);

            if (dataModel.Active)
            {
                hangfireCommand.CreatePublisherJob(dataModel.Name);
            }
            else
            {
                hangfireCommand.DeletePublisherJob(dataModel.Name);
            }

            if (versionSelectionChanged || prohibitResendMediaIdChanged)
            {
                currentAiringQuery.DeleteIgnoredQueue(dataModel.Name);
            }

            return dataModel.ToBusinessModel<DLModel.Queue, Model.Queue>();
        }

        /// <summary>
        /// Delete's the queue
        /// </summary>
        /// <param name="id">Object Id to delete</param>
        public void DeleteQueue(string id)
        {
            queueDeleteCommand.Delete(new ObjectId(id));
        }
    }
}
