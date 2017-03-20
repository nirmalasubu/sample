using System;
using System.Collections.Generic;
using System.Linq;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.DAL.Modules.Reporting.Queries;

namespace OnDemandTools.Business.Modules.Reporting
{
    public class DfStatusDeporter : IDfStatusDeporter
    {
        private readonly IAiringService _airingService;
        private readonly IDfStatusQuery _statusQuery;

        public DfStatusDeporter(IDfStatusQuery statusQuery, IAiringService airingService)
        {
            _statusQuery = statusQuery;
            _airingService = airingService;
        }

        public void DeportDfStatus()
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

                    }
                }
            }
        }
    }
}