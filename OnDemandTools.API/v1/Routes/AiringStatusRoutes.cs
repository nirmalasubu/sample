using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using OnDemandTools.API.Helpers;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Business.Modules.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using VMAiringRequestModel = OnDemandTools.API.v1.Models.Airing.Update;

namespace OnDemandTools.API.v1.Routes
{
    public sealed class AiringStatusRoutes : NancyModule
    {
        private readonly IStatusSerivce _statusService;

        public AiringStatusRoutes(
            IAiringService airingSvc,
            IStatusSerivce statusService,
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
                    if (airingSvc.IsAiringExists(airingId))
                    {
                        // Bind POST request to data contract
                        var request = this.Bind<VMAiringRequestModel.AiringStatusRequest>();

                        var validationResults = ValidateRequest(request);

                        if (validationResults.Any())
                        {
                            return Negotiate.WithModel(validationResults)
                                   .WithStatusCode(HttpStatusCode.BadRequest);
                        }

                        var airing = airingSvc.GetBy(airingId, AiringCollection.CurrentCollection);

                        foreach (var status in request.Status)
                        {
                            airing.Status[status.Key] = status.Value;
                        }

                        //Clears the existing delivery details
                        //TODO Queue reset logic goes here

                        // Finally, persist the airing data
                        airingSvc.Save(airing, false, true);

                        return "Successfully updated the airing status.";
                    }

                    var airingErrorMessage = string.IsNullOrWhiteSpace(airingId) ?
                                                "AiringId is required." : "Provided AiringId does not exists.";

                    // Return status
                    return Negotiate.WithModel(airingErrorMessage)
                                .WithStatusCode(!string.IsNullOrWhiteSpace(airingId) ? HttpStatusCode.NotFound : HttpStatusCode.BadRequest);

                }
                catch (Exception e)
                {
                    logger.Error(e, "Failure ingesting status to airing. Airingid:{@airingId}", airingId);
                    throw;
                }

            });


            #endregion


        }

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
