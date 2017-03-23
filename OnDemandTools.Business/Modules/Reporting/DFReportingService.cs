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

        public void Report(BLAiringModel.Airing airing, bool isActiveAiringStatus)
        {
            reportStatusCommandSvc.Report(airing.ToDataModel<BLAiringModel.Airing, DLAiringModel.Airing>(), isActiveAiringStatus);
        }

        public void Report(string airingId, bool isActiveAiringStatus, string statusMessage, int dfStatus = 13, int dfDestination = 18)
        {
            reportStatusCommandSvc.Report(airingId, isActiveAiringStatus, statusMessage, dfStatus, dfDestination);
        }

        public void Report(string airingId, bool isActiveAiringStatus, int statusEnum, int destinationEnum, string message, bool unique = false)
        {
            reportStatusCommandSvc.Report(airingId, isActiveAiringStatus, statusEnum, destinationEnum, message, unique);
        }
    }
}
