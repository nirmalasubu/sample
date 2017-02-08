using OnDemandTools.Business.Modules.Queue.Model;
using OnDemandTools.DAL.Modules.Reporting.Command;

namespace OnDemandTools.Jobs.JobRegistry.Publisher
{
    public class QueueReporter : IQueueReporter
    {
        private readonly IReportStatusCommand _command;

        public QueueReporter(IReportStatusCommand command)
        {
            _command = command;
        }

        public void Report(Queue queue, string airingId, string message, int statusEnum, bool unique = false)
        {
            // If the queue requested to see statuses in digital fulfillment, proceed with
            // reporting to digital fulfillment
            if (queue.Report)
            {               
                // Airing destination is defaulted to 18 (NONE) as defined in digital fulfillment
                _command.Report(airingId, statusEnum, 18, message, unique);
            }
                
        }
        public void BimReport(Queue queue, string airingId, string message, int statusEnum)
        {
            // If the queue requested to see statuses in digital fulfillment, proceed with
            // reporting to digital fulfillment
            if (queue.Report)
            {
                // Airing destination is defaulted to 18 (NONE) as defined in digital fulfillment
                _command.BimReport(airingId, statusEnum, 18, message);
            }

        }
    }
}