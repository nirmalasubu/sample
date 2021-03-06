﻿using OnDemandTools.DAL.Modules.Airings.Model;
using OnDemandTools.DAL.Modules.Queue.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnDemandTools.DAL.Modules.Airings
{
    public interface IAiringDeleteCommand
    {
        Airing Delete(Airing airing);
    }
    public interface IAiringSaveCommand
    {
        Airing Save(Airing airing, bool hasImmediateDelivery, bool updateHistorical);
    }

    public interface IGetAiringQueryPrimaryDb
    {
        Airing GetBy(string assetId, AiringCollection getFrom = AiringCollection.CurrentOrExpiredCollection);

        bool IsAiringDeleted(string assetId);

        bool IsAiringExpired(string assetId);

        bool IsAiringExists(string assetId);

    }

    public interface IAiringMessagePusher
    {
        void PushBy(DeliverCriteria criteria);
        void PushBy(string queueName, IList<string> airingIds);
        void PushBy(string queueName, string query, int hoursOut, IList<string> airingIds);
    }

    public interface IGetAiringQuery
    {
        Airing GetBy(string assetId, AiringCollection getFrom = AiringCollection.CurrentOrExpiredCollection);

        bool IsAiringDeleted(string assetId);

        bool IsAiringExpired(string assetId);

        bool IsAiringExists(string assetId);

        IQueryable<Airing> GetNonExpiredBy(int titleId, DateTime cutOffDateTime, bool isSeries = false);

        IQueryable<Airing> GetNonExpiredBy(string destination, DateTime cutOffDateTime);

        List<Airing> GetBy(string brand, string destination, DateTime startDate, DateTime endDate, string airingStatus = "");

        IQueryable<Airing> GetByMediaId(string mediaId);

        IList<Airing> GetAiringsByMediaId(string mediaId, DateTime startDate, DateTime endDate);

        long GetPendingDeliveryCountBy(string queueName);
    }

    public interface ITaskUpdater
    {
        void UpdateFor(List<string> airingIds, List<string> tasks);
    }

    public interface IAiringQuery
    {
        Airing GetExampleBy(string jsonQuery);
        void DeleteIgnoredQueue(string queueName);
    }

    public interface IChangeDeletedAiringQuery
    {
        IQueryable<Airing> Query();
    }

    public interface IChangeHistoricalAiringQuery
    {
        IEnumerable<Airing> Find(IEnumerable<string> assetIdList);
    }

    public interface IDeletedAiringsQuery
    {
        void DeleteIgnoredQueue(string queueName);
    }

    public interface IGetModifiedAiringQuery
    {
        IQueryable<Airing> GetNonExpiredBy(IList<int> titleIds, IQueryable<string> queueNames, DateTime cutOffDateTime);
    }

    public interface IDeportExpiredAiring
    {
        void Deport(int airingDeportGraceDays);
    }
}
