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

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public string Post()
        {
            return "Success";
        }

        // POST api/values
        [HttpPost("reset/{name}")]
        public string ResendQueue(string name)
        {
            _queueSvc.FlagForRedelivery(name);
            return "Success";
        }

        // POST api/values
        [HttpPost("purge/{name}")]
        public void PurgeQueue(string name)
        {
            var queues = _queueSvc.GetQueues();

            var singleQueueAttached = queues.Count(q => q.Name == name) == 1;

            if (singleQueueAttached)
                remoteQueueHandler.Purge(name);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
