using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR.Json;
using OnDemandTools.API.Utilities.Serialization;
using OnDemandTools.Business.Modules.HangFire;
using OnDemandTools.Business.Modules.Queue;
using OnDemandTools.Business.Modules.Queue.Model;
using OnDemandTools.Common.Model;
using OnDemandTools.Web.Models.DeliveryQueue;
using System.Collections.Generic;

namespace OnDemandTools.Web.SignalR
{
    [HubName("deliveryQueueCountHub")]
    public class DeliveryQueueCountHub : Hub
    {
    }
}
