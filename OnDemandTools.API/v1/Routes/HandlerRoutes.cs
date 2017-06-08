using AutoMapper;
using Nancy;
using Nancy.Extensions;
using Nancy.ModelBinding;
using Nancy.Security;
using OnDemandTools.API.Helpers;
using OnDemandTools.API.v1.Models.Handler;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Business.Modules.Destination;
using OnDemandTools.Business.Modules.File;
using OnDemandTools.Business.Modules.Handler;
using OnDemandTools.Business.Modules.Pathing;
using OnDemandTools.Business.Modules.Queue;
using OnDemandTools.Business.Modules.Reporting;
using OnDemandTools.Common.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using BLFileModel = OnDemandTools.Business.Modules.File.Model;
using BLPathModel = OnDemandTools.Business.Modules.Pathing.Model;
using OnDemandTools.Business.Modules.Airing.Model;
using OnDemandTools.Business.Modules.Queue.Model;
using OnDemandTools.Common.Extensions;

namespace OnDemandTools.API.v1.Routes
{
    /// <summary>
    /// Handler related routes
    /// </summary>
    public class HandlerRoutes : NancyModule
    {
        #region PROPERTIES 

        // Digital fufillment codes
        private readonly int DF_COMPLETED_STATUS_CODE = 2;
        private readonly int DF_ERROR_STATUS_CODE = 4;
        private int DF_ENCOM_DESTINATION_CODE;

        private IAiringService _airingSvc;
        private IDestinationService _destinationSvc;
        private readonly IPathingService _pathingSvc;
        private IFileService _fileSvc;
        private IHandlerHistoryService _handlerHistorySvc;
        private IQueueService _queueSvc;
        private IReportingService _reporterSvc;
        private EncodingFileContentValidator _encodingFileValidator;
        public Serilog.ILogger _logger { get; }

        #endregion

        public HandlerRoutes(
            IAiringService airingSvc,
            IDestinationService destinationSvc,
            IPathingService pathingSvc,
            IFileService fileSvc,
            IHandlerHistoryService handlerHistorySvc,
            IQueueService queueSvc,
            IReportingService reporterSvc,
            EncodingFileContentValidator encodingFileValidator,
            Serilog.ILogger logger
            )
            : base("v1")
        {

            _airingSvc = airingSvc;
            _destinationSvc = destinationSvc;
            _pathingSvc = pathingSvc;
            _fileSvc = fileSvc;
            _handlerHistorySvc = handlerHistorySvc;
            _queueSvc = queueSvc;
            _encodingFileValidator = encodingFileValidator;
            _reporterSvc = reporterSvc;
            _logger = logger;


            /// Persist Encoding related data through POST operation.
            /// The correlation between an existing airing and the provided payload
            /// is performed using the provided odt-media-id data point. As a result,
            /// odt-media-id is a required field in the payload.

            Post("/handler/encoding", _ =>
            {
                BLFileModel.File file = default(BLFileModel.File);

                try
                {
                    // Perform preliminary authorization
                    this.RequiresAuthentication();
                    this.RequiresClaims(c => c.Type == HttpMethod.Post.Verb());

                    // Deserealize JSON request to DataContracts. 
                    EncodingFileContentViewModel encodingPayLoad = this.Bind<EncodingFileContentViewModel>();

                    // Retrieve encoding destination details                  
                    DF_ENCOM_DESTINATION_CODE = _destinationSvc.GetByDestinationNames(new List<String> { "ENCOM" }).First().ExternalId;

                    // Persist encoding raw JSON data before proceeding
                    this.Request.Body.Seek(0, SeekOrigin.Begin);
                    _handlerHistorySvc.Save(this.Request.Body.AsString(), encodingPayLoad.MediaId);

                    // Validate provided data contract. If validation errors are found
                    // then inform user, else continue
                    var validationResult = _encodingFileValidator.Validate(encodingPayLoad);
                    if (!validationResult.IsValid)
                    {
                        // Report error status (for encoding destination) to monitoring system                      
                        ReportStatusToMonitoringSystem(encodingPayLoad.MediaId, "Ingestion of encoding data failed due to validation errors in payload", DF_ERROR_STATUS_CODE, DF_ENCOM_DESTINATION_CODE);

                        // Return status
                        return Negotiate.WithModel(validationResult.Errors.Select(c => c.ErrorMessage))
                                        .WithStatusCode(HttpStatusCode.BadRequest);
                    }



                    // Translate DataContracts to Business Models
                    file = encodingPayLoad.ToBusinessModel<EncodingFileContentViewModel, BLFileModel.File>();

                    // Apply business rules
                    // => perform path translation
                    ApplyPathTranslationInformation(file);

                    // Perform CRUD
                    _fileSvc.PersistVideoFile(file);

                    // Send notification to all subscribed consumers
                    SendNotification(file);

                    // Report completed status (for Encoding destination) to monitoring system 
                    ReportStatusToMonitoringSystem(file.MediaId, "Successfully ingested encoding data", DF_COMPLETED_STATUS_CODE, DF_ENCOM_DESTINATION_CODE);

                    // Return                    
                    return Response.AsJson(new
                    {
                        result = "success"
                    }, HttpStatusCode.OK);

                }
                catch (Exception ex)
                {
                    // Log error
                    logger.Error(ex, ex.Message);
                    throw;
                }

            });
        }

        #region PRIVATE METHODS        

        /// <summary>
        // Send notifications to all subscribed consumers
        /// </summary>
        private void SendNotification(BLFileModel.File file)
        {
            List<string> statusList = file.Contents.FirstOrDefault().MediaCollection.Select(c => c.Type.Replace("_", "").Replace("-", "").ToUpper()).ToList();
            List<Airing> airingIds = _airingSvc.GetByMediaId(file.MediaId);

            // For each airing associated with the media id, do the following
            foreach (Airing airing in airingIds)
            {
                // Persist statuses
                foreach (string status in statusList)
                {
                    airing.Status[status] = true;
                }

                // Finally, persist the airing data
                _airingSvc.Save(airing, false, true);

                // Retrieve full list of notification changes
                List<ChangeNotification> notifications = GetNotificationList(airing, statusList);

                // Update the status notification
                _airingSvc.CreateNotificationForStatusChange(airing.AssetId, notifications);
            }
        }


        /// <summary>
        /// Retrieve status notifications for both file/video and filetype/status. Both
        /// are based on queue settings
        /// </summary>
        /// <param name="statusQueues"></param>
        /// <param name="airing"></param>
        /// <returns>list of status change notifications</returns>
        private List<ChangeNotification> GetNotificationList(Airing airing, List<string> statuses)
        {
            List<Queue> activeQueues = _queueSvc.GetByStatus(true);
            IList<string> queuesTobeNotified = new List<string>();
            List<string> notifiableStatuses = statuses;
            List<ChangeNotification> changeNotifications = new List<ChangeNotification>();

            // Prepare for file/video notification
            // Find all queues that are subscribed for video change notification
            // and prepare for notification
            List<string> videoQueueNames = activeQueues
                 .Where(q => q.DetectVideoChanges)
                 .Select(q => q.Name).ToList();

            foreach (string queue in videoQueueNames)
            {
                if (airing.DeliveredTo.Contains(queue) || airing.IgnoredQueues.Contains(queue) ||
                         airing.ChangeNotifications.Select(x => x.QueueName).Contains(queue))
                {
                    ChangeNotification newChangeNotification = new ChangeNotification();
                    newChangeNotification.QueueName = queue;
                    newChangeNotification.ChangeNotificationType = ChangeNotificationType.File.ToString();
                    newChangeNotification.ChangedDateTime = DateTime.UtcNow;
                    changeNotifications.Add(newChangeNotification);
                }

            }


            // Prepare for filetype/status notifications
            foreach (string statusname in notifiableStatuses)
            {
                List<Queue> subscribedQueues = activeQueues.Where(x => x.StatusNames.Contains(statusname)).ToList();

                foreach (string deliveryQueue in subscribedQueues.Select(e => e.Name))
                {
                    if (airing.DeliveredTo.Contains(deliveryQueue) || airing.IgnoredQueues.Contains(deliveryQueue) || airing.ChangeNotifications.Select(x => x.QueueName).Contains(deliveryQueue))
                    {
                        if (!changeNotifications
                                .Where(c =>
                                {
                                    return c.QueueName.Contains(deliveryQueue) &&
                                    c.ChangeNotificationType == ChangeNotificationType.Status.ToString();
                                }).IsNullOrEmpty())
                        {
                            ChangeNotification existingChangeNotification = changeNotifications.Where(c =>
                            {
                                return c.QueueName.Contains(deliveryQueue) &&
                                c.ChangeNotificationType == ChangeNotificationType.Status.ToString();
                            }).FirstOrDefault();
                            existingChangeNotification.ChangedProperties.Add(statusname);
                        }
                        else
                        {
                            ChangeNotification newChangeNotification = new ChangeNotification();
                            newChangeNotification.QueueName = deliveryQueue;
                            newChangeNotification.ChangeNotificationType = ChangeNotificationType.Status.ToString();
                            newChangeNotification.ChangedProperties.Add(statusname);
                            newChangeNotification.ChangedDateTime = DateTime.UtcNow;
                            changeNotifications.Add(newChangeNotification);
                        }
                    }
                }
            }

            return changeNotifications;
        }




        /// <summary>
        /// Reports the status to monitoring system. For each airing/asset
        /// within the given mediaid and has ENCOM destination,
        /// report its status to digital fulfillment.
        /// 
        /// DF completed status code => 2
        /// DF destination code => 30 (ENCOM)      
        /// DF error status code => 4        
        /// 
        /// </summary>
        /// <param name="mediaId">The media identifier.</param>
        /// <param name="statusMessage">The status message.</param>
        private void ReportStatusToMonitoringSystem(String mediaId, String statusMessage, int dfStatusCode, int dfDestinationCode)
        {
            if (!String.IsNullOrWhiteSpace(mediaId))
            {

                var airingQuery = (from a in _airingSvc.GetByMediaId(mediaId)
                                   from f in a.Flights
                                   from d in f.Destinations
                                   where d.ExternalId == dfDestinationCode
                                   select a.AssetId).Distinct();

                var airings = airingQuery.ToList();

                foreach (var airing in airings)
                {
                    _reporterSvc.Report(airing, true, dfStatusCode, dfDestinationCode, statusMessage);
                }
            }
        }


        /// <summary>
        /// Logic to translate/create a corresponding URL for the
        /// bucketUrl entry provided from Encoding
        /// </summary>
        /// <param name="file"></param>
        /// <param name="pathTranslationQuery"></param>
        private void ApplyPathTranslationInformation(BLFileModel.File file)
        {
            // Get an asset (the first one in the list) with the given media id
            var airing = _airingSvc.GetByMediaId(file.MediaId).FirstOrDefault();

            foreach (BLFileModel.Content c in file.Contents)
            {
                foreach (BLFileModel.Media bm in c.MediaCollection)
                {
                    foreach (BLFileModel.PlayList pl in bm.Playlists)
                    {
                        // If bucketUrl exists, translate it to corresponding target as defined in the path translation                     
                        var urlEntry = pl.Urls.Where(d => d.ContainsKey("bucketUrl")).FirstOrDefault();
                        if (urlEntry != null)
                        {
                            // Retrieve details of bucketUrl
                            BLFileModel.Url bucketUrl = urlEntry.Values.FirstOrDefault();

                            // Find all translations that match:
                            // a combination of bucketUrl.Host and asset brand/network, or
                            // bucketUrl.Host
                            // If any match exists, do the translation; else, do nothing
                            List<BLPathModel.PathTranslation> pathings = new List<BLPathModel.PathTranslation>();
                            pathings.AddRange(_pathingSvc.GetBySourceBaseUrlAndBrand(bucketUrl.Host, airing.Network));
                            pathings.AddRange(_pathingSvc.GetBySourceBaseUrl(bucketUrl.Host));
                            List<BLPathModel.PathTranslation> distinctPathings = pathings.Distinct<BLPathModel.PathTranslation>(new BLPathModel.PathTranslationEqualityComparer()).ToList();

                            foreach (BLPathModel.PathTranslation pt in pathings.Distinct<BLPathModel.PathTranslation>(new BLPathModel.PathTranslationEqualityComparer()))
                            {
                                // Construct and add akamai url information
                                String host = pt.Target.BaseUrl;
                                String path = bucketUrl.Path;
                                String fileName = bucketUrl.FileName;
                                BLFileModel.Url url = new BLFileModel.Url() { Host = host, Path = path, FileName = fileName };
                                Dictionary<String, BLFileModel.Url> u = new Dictionary<String, BLFileModel.Url>();
                                u.Add("akamaiUrl", url);
                                pl.Urls.Add(u);

                                // Add protectionType to properties if it doesn't already exist
                                if (!pl.Properties.ContainsKey("protectionType"))
                                    pl.Properties.Add("protectionType", pt.Target.ProtectionType);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Publish notification to all queues that are
        /// subscribed to recieve notification in the event
        /// any video file is created or updated.
        /// </summary>
        /// <param name="file"></param>
        private void PublishVideoNotification(BLFileModel.File file)
        {
            //Airings to be notified about.
            //Our assumption is that the provided file doesn't contain
            //AiringId information, but does contain MediaId. If that's the case
            //retrieve the corresponding AiringId before proceeding
            List<String> airingIds = _airingSvc.GetByMediaId(file.MediaId)
                                        .Select(c => c.AssetId).ToList();

            // Queues that needs to be notified
            var videoQueueNames = _queueSvc
                 .GetByStatus(true)
                 .Where(q => q.DetectVideoChanges)
                 .Select(q => q.Name).ToList();

            // Publish notification
            _queueSvc.FlagForRedelivery(videoQueueNames, airingIds, ChangeNotificationType.File);
        }


        #endregion

    }
}
