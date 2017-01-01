using OnDemandTools.Common.Model;
using OnDemandTools.DAL.Modules.Reporting.Command;
using BLAiringModel = OnDemandTools.Business.Modules.Airing.Model;
using DLAiringModel = OnDemandTools.DAL.Modules.Airings.Model;

namespace OnDemandTools.Business.Modules.Reporting
{
    public class DFReportingService : IReportingService
    {
        IReportStatusCommand reportStatusCommandSvc;

        public DFReportingService(IReportStatusCommand reportStatusCommandSvc)
        {
            this.reportStatusCommandSvc = reportStatusCommandSvc;
        }

        public void Report(BLAiringModel.Airing airing)
        {
            reportStatusCommandSvc.Report(airing.ToDataModel<BLAiringModel.Airing, DLAiringModel.Airing>());
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
