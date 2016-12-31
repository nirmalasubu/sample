using OnDemandTools.Business.Modules.User.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.Handler
{
    public interface IHandlerHistoryService
    {
        /// <summary>
        /// Saves the specified handler history.
        /// </summary>
        /// <param name="handlerHistory">The handler history.</param>
        /// <param name="user">The user.</param>
        /// <param name="mediaId">The media identifier.</param>
        void Save(String handlerHistoryJSON, UserIdentity user, String mediaId);

    }
}
