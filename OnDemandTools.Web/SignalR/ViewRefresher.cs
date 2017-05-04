using System;
using System.Linq;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using Microsoft.AspNetCore.SignalR;

namespace OnDemandTools.Web.SignalR
{
    public class ViewRefresher
    {

        public void Refresh()
        {
            try
            {
                IHubContext context = Startup.ConnectionManager.GetHubContext<DeliveryQueueCountHub>();
               
                context.Clients.All.GetQueueDeliveryCount("Hello");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
