using System.Collections.Generic;

namespace OnDemandTools.DAL.Modules.Queue.Command
{
    public interface IQueueCommand
    {
        void ResetFor(IList<string> queueNames, IList<int> titleIds, string destinationCode);
    }
}
