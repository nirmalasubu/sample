using System;
using System.Collections.Generic;
using System.Linq;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.DAL.Modules.Reporting.Command;
using OnDemandTools.DAL.Modules.Reporting.Queries;

namespace OnDemandTools.Business.Modules.Reporting
{
    public class DfStatusDeporterService : IDfStatusDeporterService
    {
        private readonly IAiringService _airingService;
        private readonly IDfStatusQuery _statusQuery;
        private readonly IDfStatusMover _statusMover;

        public DfStatusDeporterService(IDfStatusQuery statusQuery, IAiringService airingService, IDfStatusMover statusMover)
        {
            _statusQuery = statusQuery;
            _airingService = airingService;
            _statusMover = statusMover;
        }

        /// <summary>
        /// Iterate thru all the DF Statuses and it deports expired airing statuses 
        /// </summary>
        public void DeportDfStatuses()
        {
            var modifiedTime = DateTime.Now;

            var expiredAirings = new List<string>();

            while (true)
            {
                var dfStatuses = _statusQuery.GetDfStatusEnumByModifiedDate(modifiedTime);

                if (!dfStatuses.Any())
                    break;

                modifiedTime = dfStatuses.Last().ModifiedDate.Value;

                foreach (var dfStatus in dfStatuses)
                {
                    var isExpiredSatus = false;

                    if (expiredAirings.Contains(dfStatus.AssetID))
                    {
                        isExpiredSatus = true;
                    }
                    else
                    {
                        if (!_airingService.IsAiringExists(dfStatus.AssetID))
                        {
                            isExpiredSatus = true;
                            expiredAirings.Add(dfStatus.AssetID);
                        }
                    }

                    if (isExpiredSatus)
                    {
                        _statusMover.MoveToExpireCollection(dfStatus);
                    }
                }
            }
        }

        /// <summary>
        /// Deports the airing status by given airingId
        /// </summary>
        /// <param name="airingId">the airing id</param>
        public void DeportByAssetId(string airingId)
        {
            foreach (var dfStatus in _statusQuery.GetDfStatuses(airingId))
            {
                _statusMover.MoveToExpireCollection(dfStatus);
            }
        }
    }
}