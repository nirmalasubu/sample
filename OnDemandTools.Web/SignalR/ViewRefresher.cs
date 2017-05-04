using System;
using System.Linq;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using OnDemandTools.Web.Models.DeliveryQueue;

namespace OnDemandTools.Web.SignalR
{
    public class ViewRefresher
    {

        public void Refresh(QueuesHubModel dataToBroadCast)
        {
            try
            {
                IHubContext context = Startup.ConnectionManager.GetHubContext<DeliveryQueueCountHub>();
               
                context.Clients.All.GetQueueDeliveryCount(dataToBroadCast);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
