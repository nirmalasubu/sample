using System.Collections.Generic;
using System.Linq;

namespace OnDemandTools.DAL.Modules.Queue.Command
{
    public interface IQueueCommand
    {
        void ResetFor(IList<string> queueNames, IList<int> titleIds, string destinationCode);
        void ResetFor(IList<string> queueNames, IList<string> contentIds, string destinationCode);
        void ResetFor(IList<string> queueNames, IList<int> titleIds);
        void ResetFor(IList<string> queueNames, IList<string> airingIds);
        void UpdateQueueProcessedTime(string name);
    }
}
