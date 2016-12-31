using OnDemandTools.DAL.Modules.Handler.Model;

namespace OnDemandTools.DAL.Modules.Handler.Command
{
    /// <summary>
    /// Generic function for HandlerHistory
    /// </summary>
    public interface IHandlerHistoryCommand
    {

        /// <summary>
        /// Saves the specified encoding raw payload.
        /// </summary>
        /// <param name="encodingRawPayload">The encoding raw payload.</param>
        void Save(HandlerHistory encodingPayload, string userName, string handlerHistoryJSON);
    }
}
