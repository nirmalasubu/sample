using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Web.SignalR
{
    [HubName("myChatHub")]
    public class MyChatHub : Hub
    {
        public void Hello()
        {
            Clients.All.sayHello($"Hello from {Context.ConnectionId}");
        }
    }
}
