using System;
using OnDemandTools.Business.Modules.User.Model;
using OnDemandTools.DAL.Modules.Handler.Command;
using OnDemandTools.DAL.Modules.Handler.Model;

namespace OnDemandTools.Business.Modules.Handler
{
    public class HandlerHistoryService : IHandlerHistoryService
    {
        IHandlerHistoryCommand handlerHistoryCommand;

        public HandlerHistoryService(IHandlerHistoryCommand handlerHistoryCommand)
        {
            this.handlerHistoryCommand = handlerHistoryCommand;
        }

        public void Save(string handlerHistoryJSON, UserIdentity user, string mediaId)
        {
            HandlerHistory handHist = new HandlerHistory();
            handHist.MediaId = mediaId;           
            handlerHistoryCommand.Save(handHist, user.UserName, handlerHistoryJSON);
        }
    }
}
