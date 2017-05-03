using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnDemandTools.Business.Modules.AiringPublisher.Workflow;
using OnDemandTools.Common.Configuration;
using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Business.Modules.Queue;
using OnDemandTools.Business.Modules.Queue.Model;
using OnDemandTools.Web.Models.DeliveryQueue;
using OnDemandTools.Common.Model;
using Microsoft.AspNetCore.Authorization;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnDemandTools.Web.Controllers
{
    [Route("api/[controller]")]
    public class DeliveryQueueController : Controller
    {
        public IQueueService _queueSvc;
        public AppSettings appsettings;
        public IRemoteQueueHandler remoteQueueHandler;

        public DeliveryQueueController(IQueueService queueSvc,
            AppSettings appsettings,
            IRemoteQueueHandler remoteQueueHandler)
        {
            this.appsettings = appsettings;
            _queueSvc = queueSvc;
            this.remoteQueueHandler = remoteQueueHandler;
        }

        // GET: api/values
        [Authorize]
        [HttpGet]
        public IEnumerable<DeliveryQueueModel> Get()
        {
            List<Queue> deliveryqueues = _queueSvc.GetQueues();

            // var userQueues = guard.Secure(dataModels);

            //var viewModels = userQueues
            //    .Select(dataModel => dataModel.ToViewModel<DeliveryQueue, DeliveryQueueModel>())
            //    .ToList();


            List<DeliveryQueueModel> deliveryQueueModel = deliveryqueues.ToViewModel<List<Queue>, List<DeliveryQueueModel>>();
            return deliveryQueueModel;
        }

      
    }
}
