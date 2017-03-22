using System;
using System.Collections.Generic;
using System.Linq;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.DAL.Modules.Airings.Queries;
using OnDemandTools.DAL.Modules.Reporting.Command;
using OnDemandTools.DAL.Modules.Reporting.Queries;

namespace OnDemandTools.Business.Modules.Reporting
{
    public class DfStatusDeporterService : IDfStatusDeporterService
    {
        private readonly IAiringService _airingService;
        private readonly IDfStatusQuery _statusQuery;
        private readonly IDfStatusMover _statusMover;
        private readonly CurrentAiringsQuery _currentAiringsQuery;

        public DfStatusDeporterService(
            IDfStatusQuery statusQuery,
            IAiringService airingService,
            IDfStatusMover statusMover,
            CurrentAiringsQuery currentAiringsQuery)
        {
            _statusQuery = statusQuery;
            _airingService = airingService;
            _statusMover = statusMover;
            _currentAiringsQuery = currentAiringsQuery;
        }

        /// <summary>
        /// Iterate thru all the DF Statuses and it deports expired airing statuses 
        /// </summary>
        public void DeportDfStatuses()
        {
            var modifiedTime = DateTime.Now;

            var expiredAirings = new Dictionary<string, bool>();

            var currentAirings = _currentAiringsQuery.GetAllAiringIds();

            while (true)
            {
                var dfStatuses = _statusQuery.GetDfStatusEnumByModifiedDate(modifiedTime);

                if (!dfStatuses.Any())
                    break;

                modifiedTime = dfStatuses.Last().ModifiedDate.Value;

                foreach (var dfStatus in dfStatuses)
                {
                    if (!currentAirings.Contains(dfStatus.AssetID))
                    {
                        _statusMover.MoveToExpireCollection(dfStatus);
                    }
                }
            }
        }
    }
}