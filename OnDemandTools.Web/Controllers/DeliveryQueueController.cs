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
using System.Text.RegularExpressions;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Business.Modules.Airing.Model;
using OnDemandTools.Business.Adapters.Hangfire;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnDemandTools.Web.Controllers
{
    [Route("api/[controller]")]
    public class DeliveryQueueController : Controller
    {
        IQueueService _queueSvc;
        AppSettings appsettings;
        IRemoteQueueHandler remoteQueueHandler;
        IAiringService _airingService;
        IHangfireRecurringJobCommand hangfireCommand;

        public DeliveryQueueController(IQueueService queueSvc,
            AppSettings appsettings,
            IRemoteQueueHandler remoteQueueHandler,
            IAiringService airingSvc,
            IHangfireRecurringJobCommand hangfireCommand
            )
        {
            this.appsettings = appsettings;
            _queueSvc = queueSvc;
            _airingService = airingSvc;
            this.remoteQueueHandler = remoteQueueHandler;
            this.hangfireCommand = hangfireCommand;
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

        [Authorize]
        [HttpGet("newqueue")]
        public DeliveryQueueModel GetEmptyModel()
        {
            return new DeliveryQueueModel
            {
                MessageCount = "0",
                PendingDeliveryCount = "0",
                ContactEmailAddress = string.Empty,
                Query = string.Empty,
                Name = string.Empty,
                RoutingKey= string.Empty,
                FriendlyName = string.Empty
            };
        }

        [Authorize]
        [HttpPost]
        public DeliveryQueueModel Post([FromBody]DeliveryQueueModel viewModel)
        {
            Queue blModel = viewModel.ToBusinessModel<DeliveryQueueModel, Queue>();

            if (string.IsNullOrEmpty(blModel.Id))
            {
                blModel.CreatedDateTime = DateTime.UtcNow;
                blModel.CreatedBy = HttpContext.User.Identity.Name;
            }
            else
            {
                blModel.ModifiedDateTime = DateTime.UtcNow;
                blModel.ModifiedBy = HttpContext.User.Identity.Name;
            }

            blModel = _queueSvc.SaveQueue(blModel);

            return blModel.ToViewModel<Queue, DeliveryQueueModel>();
        }

        [Authorize]
        [HttpGet("notificationhistory/{name}")]
        public IEnumerable<HistoricalMessage> GetNotificationHistory(string name)
        {
            List<HistoricalMessage> messages = _queueSvc.GetTop50MessagesDeliveredForQueue(name);
            return messages;
        }

        [Authorize]
        [HttpGet("notificationhistory/{name}/{airingids}")]
        public IEnumerable<HistoricalMessage> GetNotificationHistory(string name, string airingids)
        {
            //clears the empty spaces
            airingids = airingids.Replace(" ", string.Empty);

            List<string> airingIdsRequest = Regex.Split(airingids, ",").ToList();

            List<HistoricalMessage> messages = _queueSvc.GetAllMessagesDeliveredForAiringId(airingIdsRequest, name);
            return messages;
        }

        // POST api/values
        [Authorize]
        [HttpPost("messages/deliver")]
        public bool ResendQueueByCriteria([FromBody]DeliverCriteriaModel criteria)
        {
            return _queueSvc.FlagForRedeliveryByCriteria(criteria.ToBusinessModel<DeliverCriteriaModel, DeliverCriteria>());
        }

        [Authorize]
        [HttpPost("syntaxChecker")]
        public Airing QuerySyntaxChecker([FromBody]DeliverCriteriaModel criteria)
        {
            return _airingService.GetExampleBy(criteria.Query);
        }

        [Authorize]
        [HttpPost("reset/{name}")]
        public string ResendQueue(string name)
        {
            _queueSvc.FlagForRedelivery(name);
            return "Success";
        }

        [Authorize]
        [HttpPost("reset/{name}/{airingId}")]
        public string ResendQueue(string name, string airingId)
        {
            _queueSvc.FlagForRedelivery(name, airingId);
            return "Success";
        }

        [Authorize]
        [HttpDelete("purge/{name}")]
        public void PurgeQueue(string name)
        {
            var queues = _queueSvc.GetQueues();

            var singleQueueAttached = queues.Count(q => q.Name == name) == 1;

            if (singleQueueAttached)
                remoteQueueHandler.Purge(name);
        }

        [Authorize]
        [HttpDelete("clear/{name}")]
        public void ClearQueue(string name)
        {
            _queueSvc.ClearPendingDeliveries(name);
        }

        [Authorize]
        [HttpDelete("{id}/{name}")]
        public bool Delete(string id, string name)
        {
            var queues = _queueSvc.GetQueues();

            var singleQueueAttached = queues.Count(q => q.Name == name) == 1;

            hangfireCommand.DeletePublisherJob(name);

            if (singleQueueAttached)
                remoteQueueHandler.Delete(name);

            _queueSvc.DeleteQueue(id);

            return true;
        }

    }
}
