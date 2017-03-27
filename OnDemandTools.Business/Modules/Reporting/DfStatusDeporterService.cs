using System;
using System.Linq;
using OnDemandTools.DAL.Modules.Airings.Queries;
using OnDemandTools.DAL.Modules.Reporting.Command;
using OnDemandTools.DAL.Modules.Reporting.Queries;

namespace OnDemandTools.Business.Modules.Reporting
{
    public class DfStatusDeporterService : IDfStatusDeporterService
    {
        private readonly IDfStatusQuery _statusQuery;
        private readonly IDfStatusMover _statusMover;
        private readonly CurrentAiringsQuery _currentAiringsQuery;

        public DfStatusDeporterService(
            IDfStatusQuery statusQuery,
            IDfStatusMover statusMover,
            CurrentAiringsQuery currentAiringsQuery)
        {
            _statusQuery = statusQuery;
            _statusMover = statusMover;
            _currentAiringsQuery = currentAiringsQuery;
        }

        /// <summary>
        /// Iterate thru all the DF Statuses and it deports expired airing statuses 
        /// </summary>
        public void DeportDfStatuses()
        {
            var modifiedTime = DateTime.Now;

            var currentAirings = _currentAiringsQuery.GetAllAiringIds();

            while (true)
            {
                var dfStatuses = _statusQuery.GetDfStatusEnumByModifiedDate(modifiedTime);

                if (!dfStatuses.Any())
                    break;

                modifiedTime = dfStatuses.Last().ModifiedDate.Value;

                var expiredStatueses = dfStatuses.Where(e => !currentAirings.Contains(e.AssetID));

                foreach (var dfStatus in expiredStatueses)
                {
                    _statusMover.MoveToExpireCollection(dfStatus);
                }
            }
        }


        /// <summary>
        /// Checks the airing DF messages exists in Current or Expired DF Status collection
        /// </summary>
        /// <param name="airingId">the airing id</param>
        /// <param name="isActiveAiringCollection">is Active Airing Collection?</param>
        /// <returns></returns>
        public bool HasMessages(string airingId, bool isActiveAiringCollection)
        {
            return _statusQuery.GetDfStatuses(airingId, isActiveAiringCollection).Any();
        }
    }
}