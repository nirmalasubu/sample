using System;
using System.Linq;
using OnDemandTools.DAL.Modules.Airings.Queries;
using OnDemandTools.DAL.Modules.Reporting.Command;
using OnDemandTools.DAL.Modules.Reporting.Queries;

namespace OnDemandTools.Business.Modules.Reporting
{
    public class DfStatusService : IDfStatusService
    {
        private readonly IDfStatusQuery _statusQuery;
        private readonly IDfStatusMover _statusMover;
        private readonly CurrentAiringsQuery _currentAiringsQuery;

        public DfStatusService(
            IDfStatusQuery statusQuery,
            IDfStatusMover statusMover,
            CurrentAiringsQuery currentAiringsQuery)
        {
            _statusQuery = statusQuery;
            _statusMover = statusMover;
            _currentAiringsQuery = currentAiringsQuery;
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