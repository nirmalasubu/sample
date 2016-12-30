using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.Reporting
{
    public interface IReportingService
    {
        /// <summary>
        /// Reports a status of 'Note' (enum = 13), along with the provided
        /// status message to reporting system. Status will be applied to the provided
        /// airing identifier.
        /// </summary>
        /// <param name="airingId">The airing identifier.</param>
        /// <param name="statusMessage">The status message.</param>
        /// <param name="dfStatus">The df status.</param>
        /// <param name="dfDestination">The df destination.</param>
        void Report(string airingId, string statusMessage, int dfStatus = 13, int dfDestination = 18);
    }
}
