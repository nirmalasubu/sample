using Nancy;
using Nancy.Security;
using OnDemandTools.API.Helpers;
using OnDemandTools.API.v1.Models.Queue;
using OnDemandTools.Business.Modules.Queue;
using OnDemandTools.Business.Modules.Queue.Model;
using OnDemandTools.Common.Model;
using System.Collections.Generic;
using System.Net.Http;

namespace OnDemandTools.API.v1.Routes
{

    public class QueueRoutes : NancyModule
    {
        public QueueRoutes(IQueueService queueSvc)
            : base("v1")
        {
            this.RequiresAuthentication();

            Get("/queues", _ =>
            {
                this.RequiresClaims(c => c.Type == HttpMethod.Get.Verb());

                var queues = queueSvc.GetByStatus(true)
                               .ToViewModel<List<Queue>,List<QueueViewModel>>();

                return queues;              
            });
        }
    }
}
