using Nancy;
using Nancy.Security;
using OnDemandTools.API.Helpers;
using OnDemandTools.API.v1.Models.File;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Business.Modules.File;
using OnDemandTools.Business.Modules.File.Model;
using OnDemandTools.Business.Modules.Queue;
using OnDemandTools.Common.Model;
using OnDemandTools.DAL.Modules.Reporting.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace OnDemandTools.API.v1.Routes
{
    public class FileRoutes : NancyModule
    {
        private IQueueService queueSvc;
        private IFileService fileSvc;
        private FileValidator fileValidator;
        private IReportStatusCommand reporter;
        private IAiringService  airingSvc;


        public FileRoutes(
            IQueueService queueSvc,
            IFileService fileSvc,           
            FileValidator fileValidator,
            IReportStatusCommand reporter,
            IAiringService airingSvc)

            : base("v1")
        {
            this.RequiresAuthentication();

            this.queueSvc = queueSvc;
            this.fileSvc = fileSvc;
            this.fileValidator = fileValidator;
            this.reporter = reporter;
            this.airingSvc = airingSvc;
            

            Get("/files/title/{titleId}", _ =>
            {
                this.RequiresClaims(c => c.Type == HttpMethod.Get.Verb());
                var files = fileSvc.GetByTitleId((int)_.titleId);

                return (files.ToViewModel<List<File>, List<FileViewModel>>());                
            });

            Get("/files/airing/{airingId}", _ =>
            {
                this.RequiresClaims(c => c.Type == HttpMethod.Get.Verb());
                var files = fileSvc.GetByAiringId((string)_.airingId);

                return (files.ToViewModel<List<File>, List<FileViewModel>>());
            });


            //Post["/files"] = _ =>
            //{
            //    // Verify if the user has post permission  
            //    this.RequiresClaims(new[] { "post" });

            //    // Bind POST request to data contract
            //    var newFiles = this.Bind<List<DC.File>>();

            //    // Validate files
            //    List<ValidationResult> results = ValidateFiles(newFiles);

            //    // Verify if there are any validation errors. If so, return error
            //    if (results.Where(c => (!c.IsValid)).Count() > 0)
            //    {
            //        // Report status to monitoring system
            //        ReportStatusToMonitoringSystem(newFiles, "Ingestion of file data failed due to validation errors in payload");

            //        // Return status
            //        return Negotiate.WithModel(results.Where(c => (!c.IsValid)).Select(c => c.Errors.Select(d => d.ErrorMessage)))
            //                        .WithStatusCode(HttpStatusCode.BadRequest);
            //    }

            //    // Translate data contract to file business model
            //    var files = Mapper.Map<List<DC.File>, List<File>>(newFiles);

            //    // Persist the files
            //    command.Save(files, this.UserIdentity().UserName);


            //    // Inform subscriber queues that are listening
            //    // for file notification changes
            //    ResetFileQueues(files);

            //    // Report status to monitoring system 
            //    ReportStatusToMonitoringSystem(newFiles, "Successfully ingested file data");

            //    // Return Status
            //    return Response.AsJson(new
            //    {
            //        result = "success"
            //    }, HttpStatusCode.OK);
            //};

        }


        ///// <summary>
        ///// Validates the files.
        ///// </summary>
        ///// <param name="newFiles">The new files.</param>
        //private List<ValidationResult> ValidateFiles(IEnumerable<DC.File> newFiles)
        //{
        //    // Validate provided data contract. If validation errors are found
        //    // then inform user, else continue
        //    List<ValidationResult> results = new List<ValidationResult>();

        //    foreach (var newFile in newFiles)
        //    {
        //        if (newFile.Video == true)
        //        {
        //            results.Add(_validator.Validate(newFile, ruleSet: "default,Video"));
        //        }
        //        else
        //        {
        //            results.Add(_validator.Validate(newFile, ruleSet: "NonVideo"));
        //        }
        //    }

        //    return results;
        //}



        ///// <summary>
        ///// Reports the status to digital fulfillment system. For each file, if
        /////  => If [titleid!=empty && airingid==* && mediaid==*]
        /////         do not report anything
        /////  => If [titleid==empty && airingid!=empty && mediaid!=empty]
        /////         report by AiringId
        /////  => If [titleid==empty && airingid==empty && mediaid!=empty]
        /////         For each airing/asset for the mediaid, report its status
        /////  => If [titleid==empty && airingid==empty && mediaid==empty]
        /////         do not report anything
        ///// </summary>
        ///// <param name="files">The files.</param>
        ///// <param name="statusMessage">The status message.</param>
        //private void ReportStatusToMonitoringSystem(List<DC.File> files, String statusMessage)
        //{
        //    foreach (var file in files)
        //    {

        //        if (!String.IsNullOrEmpty(file.AiringId))
        //            _handlerReporter.Report(file.AiringId, statusMessage);
        //        else if (!String.IsNullOrWhiteSpace(file.MediaId))
        //        {
        //            // Report to all assets under mediaid
        //            List<Airing> airings = _airingQuery.GetByMediaId(file.MediaId).ToList();
        //            foreach (var airing in airings)
        //                _handlerReporter.Report(airing.AssetId, statusMessage);
        //        }
        //        else if (file.TitleId.HasValue)
        //        {
        //            // Report to all assets under titleid
        //            List<Airing> airings = _airingQuery.GetNonExpiredBy(file.TitleId.Value, DateTime.MinValue).ToList();
        //            foreach (var airing in airings)
        //                _handlerReporter.Report(airing.AssetId, statusMessage);
        //        }
        //    }
        //}


        //private void ResetFileQueues(List<File> files)
        //{
        //    var imageFileTitleIds = GetTitleIdsFrom(files, false);
        //    var videoFileTitleIds = GetTitleIdsFrom(files, true);

        //    var imageFileAiringIds = GetAiringIdsFrom(files, false);
        //    var videoFileAiringIds = GetAiringIdsFrom(files, true);

        //    var videoQueueNames = _getQueuesQuery
        //        .Get(true)
        //        .Where(q => q.DetectVideoChanges)
        //        .Select(q => q.Name);

        //    var imageQueueNames = _getQueuesQuery
        //        .Get(true)
        //        .Where(q => q.DetectImageChanges)
        //        .Select(q => q.Name);

        //    _queueResetCommand.ResetFor(videoQueueNames, videoFileTitleIds);
        //    _queueResetCommand.ResetFor(imageQueueNames, imageFileTitleIds);

        //    _queueResetCommand.ResetFor(videoQueueNames, videoFileAiringIds);
        //    _queueResetCommand.ResetFor(imageQueueNames, imageFileAiringIds);
        //}

        //private List<int> GetTitleIdsFrom(IEnumerable<File> files, bool video)
        //{
        //    return files
        //        .Where(f => f.TitleId.HasValue)
        //        .Where(f => f.Video == video)
        //        .Select(f => f.TitleId.Value)
        //        .ToList();
        //}

        ///// <summary>
        ///// Gets the airing ids from.
        ///// </summary>
        ///// <param name="files">The files.</param>
        ///// <param name="video">if set to <c>true</c> [video].</param>
        ///// <returns></returns>
        //private List<string> GetAiringIdsFrom(IEnumerable<File> files, bool video)
        //{
        //    List<String> airings = new List<string>();

        //    foreach (var file in files)
        //    {
        //        if (!String.IsNullOrWhiteSpace(file.MediaId) && (file.Video == video))
        //        {
        //            airings.AddRange(_airingQuery.GetByMediaId(file.MediaId).Select(c => c.AssetId).ToList<String>());
        //        }
        //        else
        //        {
        //            if (!String.IsNullOrWhiteSpace(file.AiringId) && (file.Video == video))
        //            {
        //                airings.Add(file.AiringId);
        //            }
        //        }
        //        if (!String.IsNullOrWhiteSpace(file.MediaId) && (file.Video == video))
        //        {
        //            airings.AddRange(_airingQuery.GetByMediaId(file.MediaId).Select(c => c.AssetId).ToList<String>());
        //        }
        //        else
        //        {
        //            if (!String.IsNullOrWhiteSpace(file.AiringId) && (file.Video == video))
        //            {
        //                airings.Add(file.AiringId);
        //            }
        //        }
        //    }
        //    return airings;
        //}
    }
}
