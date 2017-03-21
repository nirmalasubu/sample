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

            var expiredAirings = new Dictionary<string, bool>();

            while (true)
            {
                var dfStatuses = _statusQuery.GetDfStatusEnumByModifiedDate(modifiedTime);

                if (!dfStatuses.Any())
                    break;

                modifiedTime = dfStatuses.Last().ModifiedDate.Value;

                foreach (var dfStatus in dfStatuses)
                {
                    if (!expiredAirings.ContainsKey(dfStatus.AssetID))
                    {
                        expiredAirings[dfStatus.AssetID] = !_airingService.IsAiringExists(dfStatus.AssetID);
                    }

                    if (expiredAirings[dfStatus.AssetID])
                    {
                        _statusMover.MoveToExpireCollection(dfStatus);
                    }
                }
            }
        }
    }
}