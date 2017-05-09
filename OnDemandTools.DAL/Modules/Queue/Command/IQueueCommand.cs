using System.Collections.Generic;
using System.Linq;

namespace OnDemandTools.DAL.Modules.Queue.Command
{
    public interface IQueueCommand
    {
        void ResetFor(IList<string> queueNames, IList<int> titleIds, string destinationCode);
        void ResetFor(IList<string> queueNames, IList<string> contentIds, string destinationCode);
        void ResetFor(IList<string> queueNames, string airingId, string destinationCode);
        void ResetFor(IList<string> queueNames, IList<int> titleIds,ChangeNotificationType changeNotificationType);
        void ResetFor(IList<string> queueNames, IList<string> airingIds, ChangeNotificationType changeNotificationType);
        void ResetFor(string queueName);
        void CancelDeliverFor(string queueName);
        void UpdateQueueProcessedTime(string name);
        
    }
}
