using OnDemandTools.DAL.Modules.Airings.Model;

namespace OnDemandTools.DAL.Modules.Reporting.Command
{
    public interface IReportStatusCommand
    {
        void Report(string airingId, bool isActiveAiringStatus, int statusEnum, int destinationEnum, string message, bool unique = false);

        void Report(string airingId, bool isActiveAiringStatus, string statusMessage, int dfStatus = 13, int dfDestination = 18);

        void Report(Airing airing, bool isActiveAiringStatus);

        void BimReport(string airingId, bool isActiveAiringStatus, int statusEnum, int destinationEnum, string message = "");
    }
}
