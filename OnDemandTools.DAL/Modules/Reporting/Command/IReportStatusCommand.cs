

using OnDemandTools.DAL.Modules.Airings.Model;

namespace OnDemandTools.DAL.Modules.Reporting.Command
{
   public interface IReportStatusCommand
    {
        void Report(string airingId, int statusEnum, int destinationEnum, string message, bool unique = false);

        void Report(string airingId, string statusMessage, int dfStatus = 13, int dfDestination = 18);

        void Report(Airing airing);

        void BimReport(string airingId, int statusEnum, int destinationEnum, string message = "");

    }
}
