using FluentValidation.Results;
using FluentValidation;
using Nancy;
using Nancy.Security;
using Nancy.ModelBinding;
using OnDemandTools.API.Helpers;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Business.Modules.AiringId;
using OnDemandTools.Business.Modules.CustomExceptions;
using OnDemandTools.Business.Modules.Product;
using OnDemandTools.Business.Modules.Queue;
using OnDemandTools.Business.Modules.Reporting;
using OnDemandTools.Common.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using BLAiringModel = OnDemandTools.Business.Modules.Airing.Model;
using BLAiringLongModel = OnDemandTools.Business.Modules.Airing.Model.Alternate.Long;
using VMAiringShortModel = OnDemandTools.API.v1.Models.Airing.Short;
using VMAiringLongModel = OnDemandTools.API.v1.Models.Airing.Long;
using OnDemandTools.API.v1.Models.Airing.Task;
using OnDemandTools.API.v1.Models.Airing.Queue;
using VMAiringRequestModel = OnDemandTools.API.v1.Models.Airing.Update;
using AutoMapper;
using OnDemandTools.Common.Configuration;

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

                    // validate
                    List<ValidationResult> results = new List<ValidationResult>();
                    results.Add(_validator.Validate(airing, ruleSet: AiringValidationRuleSet.PostPlaylist.ToString()));

                    // Verify if there are any validation errors. If so, return error
                    if (results.Where(c => (!c.IsValid)).Count() > 0)
                    {
                        var message = results.Where(c => (!c.IsValid))
                                    .Select(c => c.Errors.Select(d => d.ErrorMessage));


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
