using FluentValidation.Results;
using Nancy;
using Nancy.Security;
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
using VMAiringShortModel = OnDemandTools.API.v1.Models.Airing.Short;
using VMAiringLongModel = OnDemandTools.API.v1.Models.Airing.Short;

namespace OnDemandTools.API.v1.Routes
{

    public class AiringRoutes : NancyModule
    {
        public AiringRoutes(
            IIdDistributor airingIdDistributorSvc,
            IAiringService airingSvc,
            IReportingService reporterSvc,
            IProductService productSvc,                                 
            AiringValidator _validator,
            IQueueService queueSvc,            
            Serilog.ILogger logger
            )
            : base("v1")
        {
            this.RequiresAuthentication();



            #region "GET Operations"
            Get("/airing/{airingId}", _ =>
            {
                this.RequiresClaims(c => c.Type == HttpMethod.Get.Verb());
                var options = GetOptionsFromQueryString();

                var airing = airingSvc.GetBy((string)_.airingId);
                var user = Context.User();
                FilterDestinations(airing, user.Destinations.ToList());
                ValidateRequest(airing, user.Brands);

                var viewModel = airing.ToViewModel<BLAiringModel.Airing, VMAiringLongModel.Airing>();

                //if (options.Contains(Appenders.File.ToString().ToLower()))
                //    fileAppender.Append(viewModel);

                //if (options.Contains(Appenders.Title.ToString().ToLower()))
                //    titleAppender.Append(viewModel);

                //if (options.Contains(Appenders.Series.ToString().ToLower()) && (options.Contains(Appenders.File.ToString().ToLower())))
                //{
                //    seriesAppender.Append(viewModel);
                //}
                //else if (options.Contains(Appenders.Series.ToString().ToLower()))
                //{
                //    seriesAppender.Append(viewModel);
                //    fileAppender.SeriesFileAppend(viewModel);
                //}

                //if (options.Contains(Appenders.Destination.ToString().ToLower()))
                //    destinationAppender.Append(viewModel);

                //if (options.Contains(Appenders.Change.ToString().ToLower()))
                //    changeAppender.Append(viewModel);

                //// Append status information if requested
                //if (options.Contains(Appenders.Status.ToString().ToLower()))
                //    airingStatusAppender.AppendStatus(viewModel);

                //if (options.Contains(Appenders.Package.ToString().ToLower()))
                //    packageAppender.Append(viewModel, Request.Headers.Accept);


                return viewModel;
            });


            Get("/airings/titleId/{titleId}", _ =>
            {
                this.RequiresClaims(c => c.Type == HttpMethod.Get.Verb());
                var airings = airingSvc
                    .GetNonExpiredBy((int)_.titleId, DateTime.UtcNow)
                    .ToList();

                var user = Context.User();
                airings = FilterDestinations(airings, user.Destinations.ToList());
                airings = FilterBrands(airings, user.Brands.ToList());

                var viewModels = airings.Select(a => a.ToViewModel<BLAiringModel.Airing, VMAiringShortModel.Airing>()).ToList();
                return viewModels;
            });

            Get("/airings/seriesId/{seriesId}", _ =>
            {                
                this.RequiresClaims(c => c.Type == HttpMethod.Get.Verb());
                var airings = airingSvc
                    .GetNonExpiredBy((int)_.seriesId, DateTime.UtcNow, true)
                    .ToList();

                var user = Context.User();
                airings = FilterDestinations(airings, user.Destinations.ToList());
                airings = FilterBrands(airings, user.Brands.ToList());
                var viewModels = airings.Select(a => a.ToViewModel<BLAiringModel.Airing, VMAiringShortModel.Airing>()).ToList();
                return viewModels;
            });

            Get("/airings/mediaId/{mediaId}", _ =>
            {
                this.RequiresClaims(c => c.Type == HttpMethod.Get.Verb());
                List<BLAiringModel.Airing> airings;
                DateTime startDate;
                DateTime endDate;
                if ((Request.Query["startDate"] != String.Empty) && (Request.Query["endDate"] != String.Empty))
                {
                    // validate
                    ValidationResult results = new ValidationResult();
                    results = validateRequestDate(Request.Query["startDate"], Request.Query["endDate"]);
                    if (results.Errors.Any())  // Verify if there are any validation errors. If so, return error
                    {
                        return Negotiate.WithModel(results.Errors.Select(c => c.ErrorMessage))
                                    .WithStatusCode(HttpStatusCode.BadRequest);   // Return status
                    }
                    startDate = getDate(Request.Query["startDate"]);
                    endDate = getDate(Request.Query["endDate"]);

                    if (startDate > endDate)
                    {
                        return Negotiate.WithModel("Start date should be smaller than the end date")
                           .WithStatusCode(HttpStatusCode.PreconditionFailed);
                    }

                    airings = airingSvc.GetAiringsByMediaId((string)_.mediaId, startDate, endDate);
                }
                else
                {
                    airings = new List<BLAiringModel.Airing>(airingSvc.GetByMediaId((string)_.mediaId));
                }

                var user = Context.User();
                airings = FilterDestinations(airings, user.Destinations.ToList());
                airings = FilterBrands(airings, user.Brands.ToList());
                var viewModels = airings.Select(a => a.ToViewModel<BLAiringModel.Airing, VMAiringShortModel.Airing>()).ToList();
                return viewModels;
            });

            Get("/airings/destination/{destination}", _ =>
            {
                this.RequiresClaims(c => c.Type == HttpMethod.Get.Verb());
                var airings = airingSvc
                    .GetNonExpiredBy((string)_.destination, DateTime.UtcNow)
                    .ToList();

                var user = Context.User();
                airings = FilterDestinations(airings, user.Destinations.ToList());
                airings = FilterBrands(airings, user.Brands.ToList());
                var viewModels = airings.Select(a => a.ToViewModel<BLAiringModel.Airing, VMAiringShortModel.Airing>()).ToList();
                return viewModels;
            });

            Get("/airings/brand/{brand}/destination/{destination}", _ =>
            {
                this.RequiresClaims(c => c.Type == HttpMethod.Get.Verb());
                List<BLAiringModel.Airing> airings = airingSvc
                    .GetBy((string)_.brand, (string)_.destination, (DateTime)Request.Query["startDate"],
                           (DateTime)Request.Query["endDate"], Request.Query["airingStatus"]);

                var user = Context.User();
                airings = FilterDestinations(airings, user.Destinations.ToList());
                airings = FilterBrands(airings, user.Brands.ToList());
                var viewModels = airings.Select(a => a.ToViewModel<BLAiringModel.Airing, VMAiringShortModel.Airing>()).ToList();
                return viewModels;
            });
            #endregion

            //#region "POST Operations"
            //Post["/airing/task"] = _ =>
            //{
            //    this.RequiresClaims(new[] { "post" });

            //    var request = this.Bind<TaskUpdateRequest>();

            //    airingSvc.UpdateTask(request.AiringIds, request.Tasks);

            //    return new
            //    {
            //        request.AiringIds,
            //        Message = "updated successfully"
            //    }; ;
            //};

            //Post["/airing/send/{airingId}"] = _ =>
            //{
            //    this.RequiresClaims(new[] { "post" });

            //    if (!airingSvc.IsAiringExists((string)_.airingId))
            //    {
            //        return Negotiate.WithModel("airingId not found")
            //                      .WithStatusCode(HttpStatusCode.NotFound);

            //    }

            //    airingSvc.PushToQueues(new List<string> { (string)_.airingId });

            //    return new
            //    {
            //        AiringId = (string)_.airingId,
            //        Message = "AiringId flagged for delivery for valid queues"
            //    };
            //};

            //Post["/airing/deliver/{airingId}"] = _ =>
            //{
            //    this.RequiresClaims(new[] { "post" });

            //    if (!airingSvc.IsAiringExists((string)_.airingId))
            //    {
            //        return Negotiate.WithModel("airingId not found")
            //                      .WithStatusCode(HttpStatusCode.NotFound);

            //    }

            //    airingSvc.PushToQueues(new List<string> { (string)_.airingId });

            //    return new
            //    {
            //        AiringId = (string)_.airingId,
            //        Message = "AiringId flagged for delivery for valid queues"
            //    };
            //};

            //Post["/airing/send"] = _ =>
            //{
            //    this.RequiresClaims(new[] { "post" });

            //    var request = this.Bind<PushRequest>();
            //    var queueName = queueSvc.GetByApiKey(request.QueueName);
            //    if (queueName == null)
            //    {

            //        return Negotiate.WithModel("Queue not found")
            //                    .WithStatusCode(HttpStatusCode.NotFound);
            //    }
            //    if (!queueName.Active)
            //    {
            //        return Negotiate.WithModel("Inactive queue")
            //                                      .WithStatusCode(HttpStatusCode.PreconditionFailed);
            //    }
            //    var invalidAirings = new List<string>();
            //    var validAiringIds = new List<string>();
            //    foreach (string airingId in request.AiringIds)
            //    {
            //        if (!airingSvc.IsAiringExists(airingId))
            //        {
            //            invalidAirings.Add(airingId);

            //        }
            //        else
            //        {
            //            validAiringIds.Add(airingId);
            //        }
            //    }
            //    if (validAiringIds.Any())
            //    {
            //        airingSvc.PushToQueue(request.QueueName, request.AiringIds);
            //    }

            //    return new
            //    {
            //        validAiringIds = validAiringIds,
            //        invalidAiringsIds = invalidAirings,
            //        Message = "validAiringIds are flagged for delivery and delivered based on enabled options of the queue"
            //    };
            //};

            //Post["/airing/deliver"] = _ =>
            //{
            //    this.RequiresClaims(new[] { "post" });

            //    var request = this.Bind<PushRequest>();
            //    var queueName = queueSvc.GetByApiKey(request.QueueName);
            //    if (queueName == null)
            //    {

            //        return Negotiate.WithModel("Queue not found")
            //                    .WithStatusCode(HttpStatusCode.NotFound);
            //    }
            //    if (!queueName.Active)
            //    {
            //        return Negotiate.WithModel("Inactive queue")
            //                                      .WithStatusCode(HttpStatusCode.PreconditionFailed);
            //    }
            //    var invalidAirings = new List<string>();
            //    var validAiringIds = new List<string>();
            //    foreach (string airingId in request.AiringIds)
            //    {
            //        if (!airingSvc.IsAiringExists(airingId))
            //        {
            //            invalidAirings.Add(airingId);

            //        }
            //        else
            //        {
            //            validAiringIds.Add(airingId);
            //        }
            //    }
            //    if (validAiringIds.Any())
            //    {
            //        airingSvc.PushToQueue(request.QueueName, request.AiringIds);
            //    }

            //    return new
            //    {
            //        AiringIds = validAiringIds,
            //        invalidAiringsIds = invalidAirings,
            //        Message = "AiringIds are flagged for delivery and delivered based on enabled options of the queue"
            //    };
            //};

            //Post["/airing/{prefix}"] = _ =>
            //{

            //    // Verify that the user has permission to POST
            //    this.RequiresClaims(new[] { "post" });

            //    LogzIO.LogzIOServiceHelper mmLogger = (dynamic)System.Configuration.ConfigurationManager.GetSection("LogzIO");


            //    // Bind POST request to data contract
            //    var request = this.Bind<AiringRequest>();
            //    try
            //    {
            //        // If the Airing provided in the request doesn't have an
            //        // AiringId, create new one.
            //        if (string.IsNullOrEmpty(request.AiringId))
            //            request.AiringId = airingIdDistributorSvc.Distribute((string)_.prefix).AiringId;

            //        // Translate data contract to airing business model
            //        var airing = Mapper.Map<Airing>(request);

            //        // validate
            //        List<ValidationResult> results = new List<ValidationResult>();
            //        results.Add(_validator.Validate(airing, ruleSet: AiringValidationRuleSet.PostAiring.ToString()));

            //        // Verify if there are any validation errors. If so, return error
            //        if (results.Where(c => (!c.IsValid)).Count() > 0)
            //        {
            //            var message = results.Where(c => (!c.IsValid))
            //                        .Select(c => c.Errors.Select(d => d.ErrorMessage));

            //            mmLogger.Send(GetMontoringProperties(new Dictionary<string, object>()
            //                                {{ "airingid", request.AiringId },{ "mediaid", airing.MediaId }, { "error", message },
            //                {"message","Failure ingesting released asset" }, {"level","Error" } }));

            //            // Return status
            //            return Negotiate.WithModel(message)
            //                        .WithStatusCode(HttpStatusCode.BadRequest);
            //        }

            //        // Now that the validation is succesful, proceed to
            //        // persisting the data. But first, populate remaining
            //        // properties for the airing business model.
            //        airing.ReleaseOn = DateTime.UtcNow;

            //        // If flight information is provided in the airing, create
            //        // the corresponding product to destination mapping as defined
            //        // in ODT
            //        if (airing.Flights.SelectMany(f => f.Products).Any())
            //        {
            //            productSvc.ProductDestinationConverter(ref airing);
            //        }

            //        // If the versions exist, create a mediaid based on the
            //        // provided version informtion and the network to which this
            //        // asset/airing belongs
            //        if (airing.Versions.Any())
            //            airingSvc.AugmentMediaId(airing);

            //        // Finally, persist the airing data
            //        var savedAiring = airingSvc.Save(airing, request.Instructions.DeliverImmediately, true);

            //        // If immediate deliver requested, force push the airing to the
            //        // respective queues (based on: product=>destination=>queue)
            //        if (request.Instructions.DeliverImmediately)
            //        {
            //            airingSvc.PushToQueues(new List<string> { savedAiring.AiringId });
            //        }

            //        // Report status of this airing to monitoring system (digital fulfillment)
            //        reporterSvc.Report(savedAiring);

            //        var logzPayload = GeneratePostAiringpropertiesForLogzIO(savedAiring);
            //        logzPayload.Add("message", "Successfully ingested released asset");
            //        mmLogger.Send(logzPayload);

            //        // Return airing model
            //        return savedAiring.ToViewModel<Airing, Common.Model.Airings.Long.Airing>();
            //    }
            //    catch (Exception e)
            //    {

            //        mmLogger.Send(GetMontoringProperties(new Dictionary<string, object>()
            //                                    { { "airingid", request.AiringId },{ "exceptionMessage", e.Message} ,
            //            { "message", "Failure ingesting released asset"},
            //            {"level", "Error" } }));
            //        throw e;
            //    }

            //};

            //#endregion

            //#region "DELETE Operations"
            //Delete["/airing"] = _ =>
            //{
            //    this.RequiresClaims(new[] { "delete" });
            //    var request = this.Bind<AiringRequest>();
            //    var airing = Mapper.Map<Airing>(request);


            //    // validate
            //    List<ValidationResult> results = new List<ValidationResult>();
            //    results.Add(_validator.Validate(airing, ruleSet: AiringValidationRuleSet.DeleteAiring.ToString()));

            //    // Verify if there are any validation errors. If so, return error
            //    if (results.Where(c => (!c.IsValid)).Count() > 0)
            //    {
            //        // Return status
            //        return Negotiate.WithModel(results.Where(c => (!c.IsValid))
            //                    .Select(c => c.Errors.Select(d => d.ErrorMessage)))
            //                    .WithStatusCode(HttpStatusCode.BadRequest);
            //    }


            //    var existingAiring = airingSvc.GetBy(airing.AssetId);

            //    var user = GetUserFromContext();

            //    ValidateRequest(existingAiring, user.Brands);

            //    existingAiring.ReleaseBy = airing.ReleaseBy;
            //    existingAiring.ReleaseOn = DateTime.UtcNow;
            //    existingAiring.DeliveredTo.Clear();

            //    airingSvc.Delete(existingAiring);

            //    if (request.Instructions.DeliverImmediately)
            //    {
            //        airingSvc.PushToQueues(new List<string> { existingAiring.AiringId });
            //    }

            //    reporterSvc.Report(existingAiring);

            //    return new
            //    {
            //        AiringId = airing.AssetId,
            //        Message = "deleted successfully"
            //    };
            //};
            //#endregion

        }


        // TODO: add logzio monitoring logix
        //private Dictionary<string, object> GetMontoringProperties(Dictionary<string, object> properties)
        //{
        //    properties.Add("step", "Scheduling");
        //    properties.Add("env", ConfigurationManager.AppSettings.Get("logzIOEnvironment"));
        //    return properties;
        //}

        //private Dictionary<string, object> GeneratePostAiringpropertiesForLogzIO(Airing savedAiring)
        //{
        //    Dictionary<string, object> Airingdetails = new Dictionary<string, object>();

        //    Airingdetails.Add("airingid", savedAiring.AssetId);
        //    Airingdetails.Add("mediaid", savedAiring.MediaId);
        //    Airingdetails.Add("level", "Success");
        //    List<string> lstDestination = new List<string>();
        //    int i = 0;
        //    string flightWindow = null;
        //    foreach (var flight in savedAiring.Flights)
        //    {
        //        flightWindow = string.Concat(flightWindow, string.Format("flightWindow[{0}] : [startDate : {1} , endDate : {2}] \n", i, flight.Start, flight.End));
        //        List<string> tempDestination = flight.Destinations.Select(x => x.Name).ToList<string>();
        //        lstDestination.AddRange(tempDestination);
        //        i++;
        //    }
        //    Airingdetails.Add("flightwindow", flightWindow);
        //    Airingdetails.Add("destination", lstDestination.Distinct());
        //    Airingdetails = GetMontoringProperties(Airingdetails);
        //    return Airingdetails;
        //}

        private List<BLAiringModel.Airing> FilterBrands(IEnumerable<BLAiringModel.Airing> airings, List<string> permissibleBrands)
        {
            return airings.Where(a => permissibleBrands.Contains(a.Network)).ToList();
        }


        private void ValidateRequest(BLAiringModel.Airing airing, IEnumerable<string> brands)
        {
            if (!brands.Contains(airing.Network))
                throw new SecurityAccessDeniedException(string.Format("Request denied for {0} brand.", airing.Network));

            if (!airing.Flights.Any())
                throw new SecurityAccessDeniedException(string.Format("Request denied for {0} airing.", airing.AssetId));
        }

        private List<BLAiringModel.Airing> FilterDestinations(List<BLAiringModel.Airing> airings, List<string> permissableDestinations)
        {
            foreach (var airing in airings)
            {
                FilterDestinations(airing, permissableDestinations);
            }

            return airings.Where(a => a.Flights.Any()).ToList();
        }

        private void FilterDestinations(BLAiringModel.Airing airing, List<string> permissableDestinations)
        {
            foreach (var flight in airing.Flights)
            {
                var destinations = flight.Destinations.ToList();

                destinations.RemoveAll(item => !permissableDestinations.Contains(item.Name));

                flight.Destinations = destinations;
            }

            for (var index = airing.Flights.Count - 1; index >= 0; index--)
            {
                if (!airing.Flights[index].Destinations.Any())
                    airing.Flights.RemoveAt(index);
            }
        }

        private List<string> GetOptionsFromQueryString()
        {
            return Request.Query["options"].HasValue
                ? ((string)Request.Query["options"].ToString().ToLower()).Split('|').ToList()
                : new List<string>();
        }

        private DateTime getDate(string dateString)
        {
            DateTime dateValue;

            if (DateTime.TryParseExact(dateString, "M/d/yyyy",
                              new CultureInfo("en-US"),
                              DateTimeStyles.None,
                              out dateValue))
            {
                return dateValue;
            }
            return dateValue;
        }

        private ValidationResult validateRequestDate(string startDate1, string endDate)
        {
            ValidationResult result = new ValidationResult();
            if (getDate(Request.Query["startDate"]) == DateTime.MinValue)
            {

                result.Errors.Add(new ValidationFailure("Startdate", "Invalid start date"));
            }

            if (getDate(Request.Query["endDate"]) == DateTime.MinValue)
            {
                result.Errors.Add(new ValidationFailure("endDate", "Invalid end date"));
            }

            return result;
        }
    }
}
