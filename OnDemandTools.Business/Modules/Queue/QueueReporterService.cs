using OnDemandTools.DAL.Modules.Reporting.Command;

namespace OnDemandTools.Business.Modules.Queue
{
    public class QueueReporterService : IQueueReporterService
    {
        private readonly IReportStatusCommand _command;

        public QueueReporterService(IReportStatusCommand command)
        {
            _command = command;
        }

        public void Report(Model.Queue queue, string airingId, bool isActiveAiringStatus, string message, int statusEnum, bool unique = false)
        {
            // If the queue requested to see statuses in digital fulfillment, proceed with
            // reporting to digital fulfillment
            if (queue.Report)
            {
                // Airing destination is defaulted to 18 (NONE) as defined in digital fulfillment
                _command.Report(airingId, isActiveAiringStatus, statusEnum, 18, message, unique);
            }

        }
        public void BimReport(Model.Queue queue, string airingId, bool isActiveAiringStatus, string message, int statusEnum)
        {
            // If the queue requested to see statuses in digital fulfillment, proceed with
            // reporting to digital fulfillment
            if (queue.Report)
            {
                // Airing destination is defaulted to 18 (NONE) as defined in digital fulfillment
                _command.BimReport(airingId, isActiveAiringStatus, statusEnum, 18, message);
            }

        }
    }
}