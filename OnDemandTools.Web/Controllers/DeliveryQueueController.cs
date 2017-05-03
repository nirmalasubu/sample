using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public DeliveryQueueController(IQueueService queueSvc)
        {
            _queueSvc = queueSvc;
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
        public void Post([FromBody]string value)
        {
        }

        // POST api/values
        [HttpPost("/reset/{name}")]
        public bool ResendQueue(string name)
        {
            _queueSvc.FlagForRedelivery(name);

            return true;
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
