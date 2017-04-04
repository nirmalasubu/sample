using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using OnDemandTools.API.Helpers;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Business.Modules.Queue;
using OnDemandTools.Business.Modules.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using OnDemandTools.Business.Modules.Airing.Model;
using VMAiringRequestModel = OnDemandTools.API.v1.Models.Airing.Update;
using OnDemandTools.Business.Modules.Queue.Model;

namespace OnDemandTools.API.v1.Routes
{
    public sealed class AiringStatusRoutes : NancyModule
    {
        private readonly IStatusSerivce _statusService;

        public AiringStatusRoutes(
            IAiringService airingSvc,
            IStatusSerivce statusService,
            IQueueService queueService,
            Serilog.ILogger logger
            )
            : base("v1")
        {
            this.RequiresAuthentication();

            _statusService = statusService;

            #region "POST Operations"


            Post("/airingstatus/{airingid}", _ =>
            {
                // Verify that the user has permission to POST
                this.RequiresClaims(c => c.Type == HttpMethod.Post.Verb());

                var airingId = (string)_.airingid;

                try
                {
                    if (!airingSvc.IsAiringExists(airingId))
                    {
                        var airingErrorMessage = string.IsNullOrWhiteSpace(airingId) ?
                            "AiringId is required." : "Provided AiringId does not exists or expired.";

                        // Return's NOT found status if airing not exists in current collection.
                        return Negotiate.WithModel(airingErrorMessage)
                                    .WithStatusCode(!string.IsNullOrWhiteSpace(airingId) ? HttpStatusCode.NotFound : HttpStatusCode.BadRequest);
                    }

                    // Bind POST request to data contract
                    var request = this.Bind<VMAiringRequestModel.AiringStatusRequest>();

                    //Validates the airing status request
                    var validationResults = ValidateRequest(request);

                    if (validationResults.Any())
                    {
                        //returns validation results if there is any
                        return Negotiate.WithModel(validationResults)
                               .WithStatusCode(HttpStatusCode.BadRequest);
                    }

                    var airing = airingSvc.GetBy(airingId, AiringCollection.CurrentCollection);


                    //Updates the airing status with the POST request
                    foreach (var status in request.Status)
                    {
                        airing.Status[status.Key] = status.Value;
                    }

                    // get all active queues
                    var statusQueues = queueService.GetByStatus(true);

                    // Finally, persist the airing data
                    airingSvc.Save(airing, false, true);

                    //update the status notification
                    airingSvc.CreateNotificationForStatusChange(airing.AssetId, ResetDeliveryQueue(statusQueues, airing));

                    return new { Result = "Successfully updated the airing status." };
                }
                catch (Exception e)
                {
                    logger.Error(e, "Failure ingesting status to airing. Airingid:{@airingId}", airingId);
                    throw;
                }

            });


            #endregion


        }



        /// <summary>
        ///  Reset's the delivery queue for the airing
        /// </summary>
        /// <param name="statusQueues"></param>
        /// <param name="airing"></param>
        /// <returns>list of status change notifications</returns>
        private List<ChangeNotification> ResetDeliveryQueue(List<Queue> statusQueues, Airing airing)
        {
            IList<string> queuesTobeNotified = new List<string>();
            List<string> airingStatusNames = airing.Status.Keys.ToList();

            List<ChangeNotification> ChangeNotifcations = new List<ChangeNotification>();
            foreach (string statusname in airingStatusNames)
            {
                var subscribedQueues = statusQueues.Where(x => x.StatusNames.Contains(statusname));
                foreach (var deliveryQueue in subscribedQueues.Select(e => e.Name))
                {
                    if (airing.DeliveredTo.Contains(deliveryQueue) || airing.IgnoredQueues.Contains(deliveryQueue))
                    {
                        if (ChangeNotifcations.Select(x => x.QueueName).Contains(deliveryQueue))
                        {
                            var existingChangeNotification = ChangeNotifcations.Where(x => x.QueueName == deliveryQueue).FirstOrDefault();
                            existingChangeNotification.ChangedProperties.Add(statusname);
                        }
                        else
                        {
                            ChangeNotification newChangeNotification = new ChangeNotification();
                            newChangeNotification.QueueName = deliveryQueue;
                            newChangeNotification.ChangeNotificationType = ChangeNotificationType.Status;
                            newChangeNotification.ChangedProperties.Add(statusname);
                            ChangeNotifcations.Add(newChangeNotification);
                        }
                    }
                }
            }
            return ChangeNotifcations;
        }




        /// <summary>
        /// Validates the request
        /// </summary>
        /// <param name="request">the airing status request</param>
        /// <returns>Validation error's in list</returns>
        private IEnumerable<string> ValidateRequest(VMAiringRequestModel.AiringStatusRequest request)
        {
            if (request.Status == null || request.Status.Count == 0)
                return new List<string>() { "Airing status is required." };

            var validStatus = _statusService.GetAllStatus();

            return from status in request.Status
                   where validStatus.All(e => e.Name != status.Key)
                   select string.Format("Provided Status key '{0}' not exists.", status.Key);
        }
    }

}
