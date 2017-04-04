using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnDemandTools.Business.Modules.Airing.Builder;
using OnDemandTools.Business.Modules.Airing.Diffing;
using OnDemandTools.Business.Modules.Airing.Model.Alternate.Change;
using OnDemandTools.Business.Modules.Airing.Model.Alternate.Long;
using OnDemandTools.Business.Modules.Airing.Model.Alternate.Title;
using OnDemandTools.Common;
using OnDemandTools.Common.Configuration;
using OnDemandTools.Common.Extensions;
using OnDemandTools.Common.Model;
using OnDemandTools.DAL.Modules.Airings;
using OnDemandTools.DAL.Modules.Airings.Commands;
using OnDemandTools.DAL.Modules.Airings.Queries;
using OnDemandTools.DAL.Modules.Destination.Queries;
using OnDemandTools.DAL.Modules.File.Queries;
using OnDemandTools.DAL.Modules.Package.Commands;
using OnDemandTools.DAL.Modules.Package.Queries;
using OnDemandTools.DAL.Modules.Queue.Queries;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLModel = OnDemandTools.Business.Modules.Airing.Model;
using DLDestinationModel = OnDemandTools.DAL.Modules.Destination.Model;
using DLFileModel = OnDemandTools.DAL.Modules.File.Model;
using DLModel = OnDemandTools.DAL.Modules.Airings.Model;
using DLPackageModel = OnDemandTools.DAL.Modules.Package.Model;

namespace OnDemandTools.Business.Modules.Airing
{
    public class AiringService : IAiringService
    {
        IGetAiringQuery airingQueryHelper;
        IAiringSaveCommand airingSaveCommandHelper;
        IAiringDeleteCommand airingDeleteCommandHelper;
        IAiringMessagePusher airingMessagePusherCommandHelper;
        IQueueQuery queueQueryHelper;
        ITaskUpdater taskUpdaterCommand;
        IFileQuery fileQueryHelper;
        AppSettings appSettings;
        IDestinationQuery destinationQueryHelper;
        IPackageQuery packageQueryHelper;
        IChangeHistoricalAiringQuery changeHistoricalAiringQueryHelper;
        IChangeDeletedAiringQuery changeDeletedAiringQueryHelper;
        IDeportExpiredAiring deportExpiredAiringHelper;
        CurrentAiringsQuery currentAiringsQuery;
        DeletedAiringsQuery deletedAiringsQuery;
        IUpdateDeletedAiringQueueDelivery updateDeletedAiringQueueDelivery;
        IUpdateAiringQueueDelivery updateAiringQueueDelivery;
        IPackageCommand packagePersist;
        IPurgeAiringCommand purgeAiringCommand;
        IChangeNotificaitonCommands changeNotificaitonCommands;
        private IApplicationContext cntx;

        public AiringService(IGetAiringQuery airingQueryHelper,
            AppSettings appSettings,
            IAiringSaveCommand airingSaveCommandHelper,
            IAiringDeleteCommand airingDeleteCommandHelper, IAiringMessagePusher airingMessagePusherCommandHelper,
            IQueueQuery queueQueryHelper,
            ITaskUpdater taskUpdaterCommand,
            IFileQuery fileQueryHelper,
            IDestinationQuery destinationQueryHelper,
            IPackageQuery packageQueryHelper,
            IChangeHistoricalAiringQuery changeHistoricalAiringQueryHelper,
            IChangeDeletedAiringQuery changeDeletedAiringQueryHelper,
            IDeportExpiredAiring deportExpiredAiringHelper,
            CurrentAiringsQuery currentAiringsQuery,
            DeletedAiringsQuery deletedAiringsQuery,
            IUpdateDeletedAiringQueueDelivery updateDeletedAiringQueueDelivery,
            IUpdateAiringQueueDelivery updateAiringQueueDelivery,
            IPackageCommand packagePersist,
            IPurgeAiringCommand purgeAiringCommand,
            IChangeNotificaitonCommands changeNotificaitonCommands,
            IApplicationContext cntx
           )
        {
            this.airingQueryHelper = airingQueryHelper;
            this.airingSaveCommandHelper = airingSaveCommandHelper;
            this.airingDeleteCommandHelper = airingDeleteCommandHelper;
            this.airingMessagePusherCommandHelper = airingMessagePusherCommandHelper;
            this.queueQueryHelper = queueQueryHelper;
            this.taskUpdaterCommand = taskUpdaterCommand;
            this.fileQueryHelper = fileQueryHelper;
            this.appSettings = appSettings;
            this.destinationQueryHelper = destinationQueryHelper;
            this.packageQueryHelper = packageQueryHelper;
            this.changeHistoricalAiringQueryHelper = changeHistoricalAiringQueryHelper;
            this.changeDeletedAiringQueryHelper = changeDeletedAiringQueryHelper;
            this.deportExpiredAiringHelper = deportExpiredAiringHelper;
            this.currentAiringsQuery = currentAiringsQuery;
            this.deletedAiringsQuery = deletedAiringsQuery;
            this.updateAiringQueueDelivery = updateAiringQueueDelivery;
            this.updateDeletedAiringQueueDelivery = updateDeletedAiringQueueDelivery;
            this.packagePersist = packagePersist;
            this.purgeAiringCommand = purgeAiringCommand;
            this.changeNotificaitonCommands = changeNotificaitonCommands;
            this.cntx = cntx;
        }

        #region "Public method"
        public BLModel.Airing Delete(BLModel.Airing airing)
        {

            return
            airingDeleteCommandHelper.Delete(airing.ToDataModel<BLModel.Airing, DLModel.Airing>())
                .ToBusinessModel<DLModel.Airing, BLModel.Airing>();
        }

        /// <summary>
        /// Deletes the package mapped to airing.
        /// </summary>
        /// <param name="airingId">The package.</param>
        /// <param name="updateHistorical">if set to <c>true</c> [update historical].</param>
        /// <returns></returns>
        public bool DeleteAiringMappedPackages(string airingId, bool updateHistorical = true)
        {
            DLPackageModel.Package existingPkg = new DLPackageModel.Package();

            if (!string.IsNullOrEmpty(airingId))
                existingPkg = packageQueryHelper.GetBy(airingId, "", "");


            if (existingPkg != null)
            {
                var username = cntx.GetUser().Name;
                existingPkg.ModifiedBy = username;
                packagePersist.DeletePackagebyAiringId(airingId, username, updateHistorical);
                return true;
            }

            return false;
        }

        public BLModel.Airing GetBy(string assetId, AiringCollection getFrom = AiringCollection.CurrentOrExpiredCollection)
        {
            return
            airingQueryHelper.GetBy(assetId, getFrom)
                .ToBusinessModel<DLModel.Airing, BLModel.Airing>();
        }

        public IEnumerable<BLModel.Airing> GetDeliverToBy(string queueName, int limit, AiringCollection getFrom = AiringCollection.CurrentCollection)
        {
            IEnumerable<DLModel.Airing> airings = new List<DLModel.Airing>();
            if (getFrom == AiringCollection.CurrentCollection)
                airings = currentAiringsQuery.GetDeliverToBy(queueName, limit);
            else if (getFrom == AiringCollection.DeletedCollection)
                airings = deletedAiringsQuery.GetDeliverToBy(queueName, limit);
            else
                throw new NotImplementedException();

            return airings.ToBusinessModel<IEnumerable<DLModel.Airing>, IEnumerable<BLModel.Airing>>();

        }

        public IEnumerable<BLModel.Airing> GetBy(string jsonQuery, int hoursOut, IList<string> queueNames, bool includeEndDate = false, AiringCollection getFrom = AiringCollection.CurrentCollection)
        {
            IEnumerable<DLModel.Airing> airings = new List<DLModel.Airing>();
            if (getFrom == AiringCollection.CurrentCollection)
                airings = currentAiringsQuery.GetBy(jsonQuery, hoursOut, queueNames, includeEndDate);
            else if (getFrom == AiringCollection.DeletedCollection)
                airings = deletedAiringsQuery.GetBy(jsonQuery, hoursOut, queueNames, includeEndDate);
            else
                throw new NotImplementedException();

            return airings.ToBusinessModel<IEnumerable<DLModel.Airing>, IEnumerable<BLModel.Airing>>();
        }

        public bool IsAiringDistributed(string airingId, string queueName, AiringCollection getFrom = AiringCollection.CurrentCollection)
        {
            if (getFrom == AiringCollection.CurrentCollection)
                return currentAiringsQuery.IsAiringDistributed(airingId, queueName);
            else if (getFrom == AiringCollection.DeletedCollection)
                return deletedAiringsQuery.IsAiringDistributed(airingId, queueName);
            else
                throw new NotImplementedException();
        }

        public List<BLModel.Airing> GetByMediaId(string mediaId)
        {
            return
            (airingQueryHelper.GetByMediaId(mediaId).ToList<DLModel.Airing>()
                .ToBusinessModel<List<DLModel.Airing>, List<BLModel.Airing>>());

        }

        public List<Model.Airing> GetNonExpiredBy(int titleId, DateTime cutOffDateTime, bool isSeries = false)
        {
            return
            (airingQueryHelper.GetNonExpiredBy(titleId, cutOffDateTime, isSeries).ToList<DLModel.Airing>()
                .ToBusinessModel<List<DLModel.Airing>, List<BLModel.Airing>>());
        }

        public bool IsAiringExists(string assetId)
        {
            return airingQueryHelper.IsAiringExists(assetId);
        }

        public BLModel.Airing Save(BLModel.Airing airing, bool hasImmediateDelivery, bool updateHistorical)
        {
            return
            (airingSaveCommandHelper.Save(airing.ToDataModel<BLModel.Airing, DLModel.Airing>(), hasImmediateDelivery, updateHistorical)
                .ToBusinessModel<DLModel.Airing, BLModel.Airing>());
        }

        public void PushToQueue(string queueName, IList<string> airingIds)
        {
            airingMessagePusherCommandHelper.PushBy(queueName, airingIds);
        }

        public void PushToQueues(IList<string> airingIds)
        {
            var queues = queueQueryHelper
                .Get()
                .Where(q => q.Active);

            foreach (var deliveryQueue in queues)
            {
                airingMessagePusherCommandHelper.PushBy(deliveryQueue.Name, deliveryQueue.Query, deliveryQueue.HoursOut, airingIds);
            }

        }

        public void PushDeliveredTo(string airingId, string queueName, AiringCollection getFrom = AiringCollection.CurrentCollection)
        {
            if (getFrom == AiringCollection.CurrentCollection)
            {
                updateAiringQueueDelivery.PushDeliveredTo(airingId, queueName);
            }
            else if (getFrom == AiringCollection.DeletedCollection)
            {
                updateDeletedAiringQueueDelivery.PushDeliveredTo(airingId, queueName);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void PushIgnoredQueueTo(string airingId, string queueName, AiringCollection getFrom = AiringCollection.CurrentCollection)
        {
            if (getFrom == AiringCollection.CurrentCollection)
            {
                updateAiringQueueDelivery.PushIgnoredQueueTo(airingId, queueName);
            }
            else if (getFrom == AiringCollection.DeletedCollection)
            {
                updateDeletedAiringQueueDelivery.PushIgnoredQueueTo(airingId, queueName);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void AugmentMediaId(ref BLModel.Airing airing)
        {
            var contentIds = new StringBuilder();

            foreach (var version in airing.Versions)
            {
                contentIds.Append(version.ContentId);
            }

            airing.MediaId = IdGenerator.Generate(string.Concat(contentIds.ToString(), airing.Network));
        }

        public void UpdateTask(List<string> airingIds, List<string> tasks)
        {
            taskUpdaterCommand.UpdateFor(airingIds, tasks);
        }

        public List<BLModel.Airing> GetAiringsByMediaId(string mediaId, DateTime startDate, DateTime endDate)
        {
            return
                (airingQueryHelper.GetAiringsByMediaId(mediaId, startDate, endDate)
                .ToList<DLModel.Airing>()
                .ToBusinessModel<List<DLModel.Airing>, List<BLModel.Airing>>());
        }

        public List<BLModel.Airing> GetNonExpiredBy(string destination, DateTime cutOffDateTime)
        {
            return
                (airingQueryHelper.GetNonExpiredBy(destination, cutOffDateTime)
                .ToList<DLModel.Airing>()
                .ToBusinessModel<List<DLModel.Airing>, List<BLModel.Airing>>());
        }

        public List<BLModel.Airing> GetBy(string brand, string destination, DateTime startDate, DateTime endDate, string airingStatus = "")
        {
            return
                (airingQueryHelper.GetBy(brand, destination, startDate, endDate, airingStatus)
                .ToList<DLModel.Airing>()
                .ToBusinessModel<List<DLModel.Airing>, List<BLModel.Airing>>());
        }

        public void AppendFile(ref BLModel.Alternate.Long.Airing airing)
        {
            if (String.IsNullOrWhiteSpace(airing.MediaId))
            {
                airing.MediaId = String.Empty;
            }

            var titleIds = ExtractTitleAndSeriesIdsFrom(airing);
            var versionIds = ExtractVersionIdsFrom(airing);
            var files = fileQueryHelper.GetBy(versionIds, titleIds, airing.AiringId, airing.MediaId).ToList();

            airing.Options.Files.AddRange(Mapper.Map<List<DLFileModel.File>, List<BLModel.Alternate.Long.File>>(files)
                                            .ToList());
        }

        public void AppendTitle(ref BLModel.Alternate.Long.Airing airing)
        {
            List<int> titleIds = airing.Title.TitleIds
                .Where(t => t.Authority == "Turner" && t.Value != null)
                .Select(t => int.Parse(t.Value)).ToList();
            titleIds.AddRange(airing.Title.RelatedTitleIds.Where(t => t.Authority == "Turner").Select(t => int.Parse(t.Value)).ToList());

            var titles = GetFlowTitlesFor(titleIds);
            var primaryTitleId = airing.Title.TitleIds.FirstOrDefault(t => t.Primary);
            if (primaryTitleId != null)
            {
                var primaryTitle = titles.First(t => t.TitleId == int.Parse(primaryTitleId.Value));
                UpdateTitleFieldsFor(ref airing, primaryTitle);
            }

            airing.Options.Titles = titles;
        }

        public void AppendSeries(ref BLModel.Alternate.Long.Airing airing)
        {
            if (airing.Title.Series == null || airing.Title.Series.Id == null)
                return;

            var series = GetTitleFor(airing.Title.Series.Id.Value);
            airing.Options.Series.Add(series);
        }

        public void AppendFileBySeriesId(ref BLModel.Alternate.Long.Airing airing)
        {
            if (airing.Title.Series == null || !airing.Title.Series.Id.HasValue)
                return;

            var files = fileQueryHelper.Get(airing.Title.Series.Id.Value).ToList();
            airing.Options.Files.AddRange(Mapper.Map<List<DLFileModel.File>, List<BLModel.Alternate.Long.File>>(files)
                                            .ToList());
        }

        public void AppendDestinations(ref BLModel.Alternate.Long.Airing airing)
        {
            var destinationNames = airing.Flights
               .SelectMany(f => f.Destinations)
               .Select(d => d.Name)
               .Distinct()
               .ToList();

            var destinations = destinationQueryHelper.GetByDestinationNames(destinationNames)
                                .ToBusinessModel<List<DLDestinationModel.Destination>, List<BLModel.Alternate.Destination.Destination>>();

            FilterPropertiesByBrand(destinations, ref airing);
            new BLModel.Alternate.Destination.DeliverableFormatter(airing).Format(destinations); // destinations passed by reference for formatting
            new BLModel.Alternate.Destination.PropertyFormatter(airing).Format(destinations); // destinations passed by reference for formatting

            airing.Options.Destinations = destinations;
        }

        public void AppendChanges(ref BLModel.Alternate.Long.Airing airing)
        {
            var changes = Find(ref airing);

            airing.Options.Changes = changes.ToList();

        }

        public void AppendStatus(ref BLModel.Alternate.Long.Airing airing)
        {
            airing.Options.Status = airing.Status;  // Retrieve other status from airing collection
            // Check if file information is already included. If so, do not
            // retrieve again; else, retrieve it
            if (airing.Options.Files.IsNullOrEmpty())
            {
                if (String.IsNullOrWhiteSpace(airing.MediaId))
                {
                    airing.MediaId = String.Empty;
                }
                var files = fileQueryHelper.Get(airing.AiringId).ToList();

                if (!files.IsNullOrEmpty())
                {
                    airing.Options.Status["video"] = !files.Where(c => c.Video == true).IsNullOrEmpty();
                }
                else
                {
                    airing.Options.Status["video"] = false;
                }
            }
            else
            {
                airing.Options.Status["video"] = !airing.Options.Files.Where(c => c.Video == true).IsNullOrEmpty();
            }

        }

        public void AppendPackage(ref BLModel.Alternate.Long.Airing airing, IEnumerable<Tuple<string, decimal>> acceptHeaders)
        {
            var titleIds = airing.Title.TitleIds.Where(title => (title.Authority.Equals("Turner") && isNumeric(title.Value)))
                            .Select(thisTitle => (getTitleId(thisTitle.Value)));

            var contentIds = airing.Versions.Select(version => version.ContentId);

            //double check no bad titleIds are in the array.
            titleIds = titleIds.Where(title => (title != -1)).ToList();
            var destinations = airing.Flights.SelectMany(flight =>
                                                         flight.Destinations.Select(destination => destination.Name));

            List<DLPackageModel.Package> matchingPackages = new List<DLPackageModel.Package>();
            List<DLPackageModel.Package> titleAndAiringIdPackages = packageQueryHelper.GetBy(airing.AiringId, titleIds.ToList(), destinations.ToList()).ToList();
            List<DLPackageModel.Package> cidPackages = packageQueryHelper.GetBy(contentIds.ToList(), destinations.ToList()).ToList();

            matchingPackages.AddRange(titleAndAiringIdPackages);
            matchingPackages.AddRange(cidPackages);
            airing.Options.Packages = matchingPackages.ToViewModel<List<DLPackageModel.Package>, List<BLModel.Alternate.Package.Package>>();

            // verify the kind of serialization
            bool isXMLSerialization = (acceptHeaders.Count(c => c.Item1 == "application/xml") > 0) ? true : false;
            foreach (var package in airing.Options.Packages)
            {
                package.Data = isXMLSerialization ? package.Data : null;
                package.PackageData = !isXMLSerialization ? package.PackageData : null;

            }

        }

        public void Deport(int airingDeportGraceDays)
        {
            deportExpiredAiringHelper.Deport(airingDeportGraceDays);
        }


        /// <summary>
        /// Create's the change notification
        /// </summary>
        /// <param name="airingId">the airing Id to notify</param>
        /// <param name="changeNotificaitonType">change notificaiton type</param>
        /// <param name="queuesToBeNotified">queues to be notified</param>
        /// <param name="changedValues">changed values</param>
   

        /// <summary>
        /// update the status change notifcation
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="list"></param>
        public void CreateNotificationForStatusChange(string assetId, List<BLModel.ChangeNotification> list)
        {
            changeNotificaitonCommands.Save(assetId, list.ToDataModel<List<BLModel.ChangeNotification>, List<DLModel.ChangeNotification>>());
              
        }

        #endregion

        #region "Private methods"
        private bool isNumeric(string titleId)
        {
            int intOut;
            return int.TryParse(titleId, out intOut);
        }

        private int getTitleId(string titleIdValue)
        {
            int intOut;
            if (int.TryParse(titleIdValue, out intOut))
                return intOut;
            else return -1;
        }

        private void FilterPropertiesByBrand(IEnumerable<BLModel.Alternate.Destination.Destination> dataModels, ref BLModel.Alternate.Long.Airing airing)
        {
            var propertiesToRemove = new List<BLModel.Alternate.Destination.Property>();

            foreach (var destination in dataModels)
            {
                foreach (var property in destination.Properties)
                {
                    if (property.Brands.Any() && !property.Brands.Contains(airing.Brand))
                    {
                        propertiesToRemove.Add(property);
                        continue;
                    }

                    if (property.TitleIds.Any() && property.SeriesIds.Any()) // Property has both title and series id . any one of the title/series Id should match
                    {
                        if (!IsPropertyTitleIdsAssociatedwithAiringTitleIds(airing, property) && !IsPropertySeriesIdsAssociatedwithAiringSeriesIds(airing, property))
                        {
                            propertiesToRemove.Add(property);
                        }
                        continue;
                    }

                    if (property.TitleIds.Any())
                    {
                        if (!IsPropertyTitleIdsAssociatedwithAiringTitleIds(airing, property)) // Any one of the title Id should match
                        {
                            propertiesToRemove.Add(property);
                        }
                    }

                    if (property.SeriesIds.Any())
                    {
                        if (!IsPropertySeriesIdsAssociatedwithAiringSeriesIds(airing, property)) // Any one of the series Id should match
                        {
                            propertiesToRemove.Add(property);
                        }
                    }
                }

                destination.Properties = destination.Properties.Where(p => !propertiesToRemove.Contains(p)).ToList();
            }
        }

        private bool IsPropertySeriesIdsAssociatedwithAiringSeriesIds(BLModel.Alternate.Long.Airing airing, BLModel.Alternate.Destination.Property property)
        {
            if (airing.Title.Series.Id.HasValue)
            {
                if (!property.SeriesIds.Contains(airing.Title.Series.Id.Value))
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        private bool IsPropertyTitleIdsAssociatedwithAiringTitleIds(BLModel.Alternate.Long.Airing airing, BLModel.Alternate.Destination.Property property)
        {
            var titleIds = airing.Title.TitleIds.Where(t => t.Authority == "Turner").Select(t => int.Parse(t.Value)).ToList();
            if (titleIds.Any())
            {
                if (!property.TitleIds.Any(titleIds.Contains))
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;

        }

        private void UpdateTitleFieldsFor(ref BLModel.Alternate.Long.Airing airing, BLModel.Alternate.Title.Title primaryTitle)
        {
            if (primaryTitle.TitleType != "Feature Film")
            {
                if (string.IsNullOrEmpty(airing.Title.Episode.Number))
                    airing.Title.Episode.Number = primaryTitle.EpisodeNumber;

                if (string.IsNullOrEmpty(airing.Title.Episode.Name))
                    airing.Title.Episode.Name = primaryTitle.TitleName;
            }

            if (airing.Title.ReleaseYear == 0)
                airing.Title.ReleaseYear = primaryTitle.ReleaseYear;

            if (airing.Title.Season.Number == 0)
                airing.Title.Season.Number = primaryTitle.SeasonNumber;

            if (string.IsNullOrEmpty(airing.Title.StoryLine.Long))
                airing.Title.StoryLine.Long = GetStoryline(primaryTitle.Storylines, "Turner External");

            if (string.IsNullOrEmpty(airing.Title.StoryLine.Short))
                airing.Title.StoryLine.Short = GetStoryline(primaryTitle.Storylines, "Short (245 Characters)");
        }

        private string GetStoryline(List<Storyline> storylines, string type)
        {
            var storyline = storylines.FirstOrDefault(s => s.Type == type);

            return (storyline == null) ? string.Empty : storyline.Description;
        }

        private BLModel.Alternate.Title.Title GetTitleFor(int titleId)
        {
            return GetFlowTitlesFor(new List<int> { titleId }).First();
        }

        private List<BLModel.Alternate.Title.Title> GetFlowTitlesFor(IEnumerable<int> titleIds)
        {
            // The Distinct() here is neccessary because of a limitation we discovered.
            // The titles API returns only distinct FlowTitle objects per request.
            // If there are more then 5 titles (because of the partition), you can potentially
            // end up with duplicates. (Titles API limits us to 25. Consult titles api wiki.)
            var listsOfTitleIds = titleIds.Distinct().Partition(25).ToList();
            RestClient client = new RestClient(appSettings.GetExternalService("Flow").Url);
            var titles = new List<BLModel.Alternate.Title.Title>();

            foreach (var list in listsOfTitleIds)
            {
                var request = new RestRequest("/v2/title/{ids}?api_key={api_key}", Method.GET);
                request.AddUrlSegment("ids", string.Join(",", list));
                request.AddUrlSegment("api_key", appSettings.GetExternalService("Flow").ApiKey);

                Task.Run(async () =>
                {
                    var rs = await GetFlowTitleAsync(client, request) as List<BLModel.Alternate.Title.Title>;
                    if (!rs.IsNullOrEmpty())
                    {
                        titles.AddRange(rs);
                    }

                }).Wait();
            }

            return titles;
        }

        private Task<List<BLModel.Alternate.Title.Title>> GetFlowTitleAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<List<BLModel.Alternate.Title.Title>>();
            theClient.ExecuteAsync<List<BLModel.Alternate.Title.Title>>(theRequest, response =>
            {
                tcs.SetResult(response.Data);
            });
            return tcs.Task;
        }

        private List<string> ExtractVersionIdsFrom(BLModel.Alternate.Long.Airing airing)
        {
            return airing.Versions
                .Select(t => t.ContentId)
               .ToList();
        }

        private static List<int> ExtractTitleAndSeriesIdsFrom(BLModel.Alternate.Long.Airing airing)
        {
            var titleIds = airing.Title.TitleIds
                .Where(t => t.Authority == "Turner" && t.Value != null)
                .Select(t => int.Parse(t.Value))
                .ToList();

            if (airing.Title.Series.Id.HasValue)
            {
                titleIds.Add(airing.Title.Series.Id.Value);
            }

            return titleIds;
        }

        #region "Private methods for options=change"
        private IEnumerable<Change> Find(ref BLModel.Alternate.Long.Airing airing)
        {
            var groupedAirings = GetAiringsToDiffOn(new[] { airing });

            var changes = new List<BLModel.Alternate.Change.Change>();

            foreach (var groupedAiring in groupedAirings)
            {
                try
                {
                    if (AiringIsNew(groupedAiring))
                    {
                        var builder = new ChangeBuilder();
                        changes.Add(builder.BuildNewChange(groupedAiring.First()));
                    }
                    else if (ComparingTwoAirings(groupedAiring))
                    {
                        var currentAsset = groupedAiring.FirstOrDefault();
                        var originalAsset = groupedAiring.LastOrDefault();
                        changes.AddRange(Find(currentAsset, originalAsset));
                    }
                    else if (ComparingThreeAirings(groupedAiring))
                    {
                        var currentAsset = groupedAiring.FirstOrDefault();
                        var previousAsset = groupedAiring.Skip(1).FirstOrDefault();
                        var originalAsset = groupedAiring.LastOrDefault();
                        changes.AddRange(Find(currentAsset, previousAsset, originalAsset));
                    }
                    else if (AiringIsBeingDeleted(groupedAiring.Key))
                    {
                        var builder = new ChangeBuilder();
                        changes.Add(builder.BuildDeletion(groupedAiring.First()));
                    }
                }
                catch (Exception ex)
                {
                    var current = groupedAiring.FirstOrDefault();
                    if (null != current)
                    {
                        var toThrow = new Exception(String.Format("Exception occurred while detecting change for Airing Id '{0}'. ", current.AiringId), ex);
                        throw toThrow;
                    }

                    throw ex;
                }
            }

            return changes;
        }

        private IEnumerable<IGrouping<string, BLModel.Alternate.Long.Airing>> GetAiringsToDiffOn(IEnumerable<BLModel.Alternate.Long.Airing> airings)
        {
            var airingIds = airings.Select(x => x.AiringId).ToList();

            var historicalAirings = changeHistoricalAiringQueryHelper.Find(airingIds).ToList();

            var viewModels = historicalAirings.
                ToBusinessModel<List<DLModel.Airing>, List<BLModel.Alternate.Long.Airing>>();

            var groupedAirings = GroupAiringsAndOrderSoEarliestAiringsAreFirst(viewModels);

            return groupedAirings;
        }

        private IEnumerable<IGrouping<string, BLModel.Alternate.Long.Airing>> GroupAiringsAndOrderSoEarliestAiringsAreFirst(IEnumerable<BLModel.Alternate.Long.Airing> historicalAirings)
        {
            var groupedAirings = from h in historicalAirings
                                 orderby h.ReleasedOn descending
                                 group h by h.AiringId
                                    into g
                                 select g;
            return groupedAirings;
        }

        private bool AiringIsNew(IEnumerable<BLModel.Alternate.Long.Airing> groupedAiring)
        {
            return groupedAiring.Count() == 1;
        }


        private bool ComparingThreeAirings(IEnumerable<BLModel.Alternate.Long.Airing> groupedAiring)
        {
            return groupedAiring.Count() > 2;
        }

        private bool ComparingTwoAirings(IEnumerable<BLModel.Alternate.Long.Airing> groupedAiring)
        {
            return groupedAiring.Count() == 2;
        }

        private bool AiringIsBeingDeleted(string airingId)
        {
            return changeDeletedAiringQueryHelper.Query().Any(x => x.AssetId == airingId);
        }

        private IEnumerable<FieldChange> Find(BLModel.Alternate.Long.Airing currentAsset, BLModel.Alternate.Long.Airing originalAiring)
        {
            var changeBuilder = new ChangeBuilder();

            var currentJson = JsonConvert.SerializeObject(currentAsset);
            var originalJson = JsonConvert.SerializeObject(originalAiring);

            var currentAssetAsJson = JObject.Parse(currentJson);
            var originalAssetAsJson = JObject.Parse(originalJson);

            var differ = new JsonDiffer();

            var results = differ.FindDifferences(currentAssetAsJson, originalAssetAsJson);

            foreach (var change in results)
            {
                changeBuilder.SetCommonValues(change, currentAsset, originalAiring);
            }

            return results;
        }

        private IEnumerable<FieldChange> Find(BLModel.Alternate.Long.Airing currentAsset, BLModel.Alternate.Long.Airing previousAsset, BLModel.Alternate.Long.Airing originalAsset)
        {
            var changeBuilder = new ChangeBuilder();

            var currentJson = JsonConvert.SerializeObject(currentAsset);
            var previousJson = JsonConvert.SerializeObject(previousAsset);
            var originalJson = JsonConvert.SerializeObject(originalAsset);

            var currentAssetAsJson = JObject.Parse(currentJson);
            var previousAssetAsJson = JObject.Parse(previousJson);
            var originalAssetAsJson = JObject.Parse(originalJson);

            var differ = new JsonDiffer();

            var results = differ.FindDifferences(currentAssetAsJson, previousAssetAsJson, originalAssetAsJson);

            foreach (var change in results)
            {
                changeBuilder.SetCommonValues(change, currentAsset, previousAsset, originalAsset);
            }

            return results;
        }

        public void PurgeUnitTestAirings(List<string> airingIds)
        {
            purgeAiringCommand.PurgeAirings(airingIds);
        }

       
        #endregion
        #endregion

    }
}
