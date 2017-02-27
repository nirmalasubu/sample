using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using OnDemandTools.API.Helpers;
using OnDemandTools.Business.Modules.Airing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using BLAiringModel = OnDemandTools.Business.Modules.Airing.Model;
using VMAiringRequestModel = OnDemandTools.API.v1.Models.Airing.Update;

namespace OnDemandTools.API.v1.Routes
{

    public class PlaylistRoutes : NancyModule
    {
        public PlaylistRoutes(
            IAiringService airingSvc,
            AiringValidator _validator,
            Serilog.ILogger logger
            )
            : base("v1")
        {
            this.RequiresAuthentication();



            #region "POST Operations"


            Post("/playlist/{airingid}", _ =>
            {
                // Verify that the user has permission to POST
                this.RequiresClaims(c => c.Type == HttpMethod.Post.Verb());

                var airingId = (string)_.airingid;

                // Bind POST request to data contract
                var request = this.Bind<VMAiringRequestModel.PlayListRequest>();
                try
                {
                    var airing = new BLAiringModel.Airing();

                    if (airingSvc.IsAiringExists(airingId))
                    {
                        airing = airingSvc.GetBy(airingId, AiringCollection.CurrentCollection);

                        //Updates the airing with the given Playlist payload
                        airing.PlayList = Mapper.Map<List<BLAiringModel.PlayItem>>(request.PlayList);
                        airing.ReleaseBy = request.ReleasedBy;

                        //Clears the existing delivery details
                        airing.IgnoredQueues = new List<string>();
                        airing.DeliveredTo = new List<string>();

                        //Sets the date with the current date time
                        airing.ReleaseOn = DateTime.UtcNow;

                    }
                    else
                    {
                        var airingErrorMessage = string.IsNullOrWhiteSpace(airingId) ?
                                                    "AiringId is required." : "Provided AiringId does not exists.";

                        // Return status
                        return Negotiate.WithModel(airingErrorMessage)
                                    .WithStatusCode(HttpStatusCode.BadRequest);
                    }

                    // validate
                    List<ValidationResult> results = new List<ValidationResult>();
                    results.Add(_validator.Validate(airing, ruleSet: AiringValidationRuleSet.PostPlaylist.ToString()));

                    // Verify if there are any validation errors. If so, return error
                    if (results.Where(c => (!c.IsValid)).Count() > 0)
                    {
                        var message = results.Where(c => (!c.IsValid))
                                    .Select(c => c.Errors.Select(d => d.ErrorMessage)).ToList();


                        logger.Error("Failure ingesting playlist released asset: {AssetId}", new Dictionary<string, object>()
                                            {{ "airingid", airingId},{ "mediaid", airing.MediaId }, { "error", message }   });

                        // Return status
                        return Negotiate.WithModel(message)
                                    .WithStatusCode(HttpStatusCode.BadRequest);
                    }

                    // Finally, persist the airing data
                    var savedAiring = airingSvc.Save(airing, false, true);

                    return "Successfully updated the playlist.";
                }
                catch (Exception e)
                {
                    logger.Error(e, "Failure ingesting playlist to airing. Airingid:{@airingId}", airingId);
                    throw e;
                }

            });


            #endregion


        }
    }
}
