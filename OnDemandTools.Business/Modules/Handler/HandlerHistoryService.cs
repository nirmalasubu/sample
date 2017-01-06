using OnDemandTools.Common.Configuration;
using OnDemandTools.DAL.Modules.Handler.Command;
using OnDemandTools.DAL.Modules.Handler.Model;

namespace OnDemandTools.Business.Modules.Handler
{
    public class HandlerHistoryService : IHandlerHistoryService
    {
        IHandlerHistoryCommand handlerHistoryCommand;
        IApplicationContext cntx;

        public HandlerHistoryService(IHandlerHistoryCommand handlerHistoryCommand, IApplicationContext cntx)
        {
            this.handlerHistoryCommand = handlerHistoryCommand;
            this.cntx = cntx;
        }

        public void Save(string handlerHistoryJSON, string mediaId)
        {
            HandlerHistory handHist = new HandlerHistory();
            handHist.MediaId = mediaId;           
            handlerHistoryCommand.Save(handHist, cntx.GetUser().UserName, handlerHistoryJSON);
        }
    }
}
