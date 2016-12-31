using OnDemandTools.DAL.Modules.Reporting.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.Reporting
{
    public class DFReportingService : IReportingService
    {
        IReportStatusCommand reportStatusCommandSvc;

        public DFReportingService(IReportStatusCommand reportStatusCommandSvc)
        {
            this.reportStatusCommandSvc = reportStatusCommandSvc;
        }

        public void Report(string airingId, string statusMessage, int dfStatus = 13, int dfDestination = 18)
        {
            reportStatusCommandSvc.Report(airingId, statusMessage, dfStatus, dfDestination);
        }

        public void Report(string airingId, int statusEnum, int destinationEnum, string message, bool unique = false)
        {
            reportStatusCommandSvc.Report(airingId, statusEnum, destinationEnum, message, unique);
        }
    }
}
