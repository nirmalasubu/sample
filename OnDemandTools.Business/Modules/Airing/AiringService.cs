﻿using System;
using System.Collections.Generic;
using OnDemandTools.DAL.Modules.Airings;
using BLModel = OnDemandTools.Business.Modules.Airing.Model;
using DLModel = OnDemandTools.DAL.Modules.Airings.Model;
using DLDestinationModel = OnDemandTools.DAL.Modules.Destination.Model;
using DLPackageModel = OnDemandTools.DAL.Modules.Package.Model;
using System.Linq;
using OnDemandTools.Common.Model;
using OnDemandTools.DAL.Modules.Queue.Queries;
using System.Text;
using OnDemandTools.Common;
using OnDemandTools.DAL.Modules.File.Queries;
using AutoMapper;
using DLFileModel = OnDemandTools.DAL.Modules.File.Model;
using OnDemandTools.Business.Modules.Airing.Model.Alternate.Title;
using RestSharp;
using Microsoft.Extensions.Configuration;
using OnDemandTools.Common.Configuration;
using OnDemandTools.Common.Extensions;
using System.Threading.Tasks;
using OnDemandTools.Business.Modules.Airing.Model.Alternate.Long;
using OnDemandTools.DAL.Modules.Destination.Queries;
using OnDemandTools.DAL.Modules.Package.Queries;

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
        IConfiguration configuration;
        IDestinationQuery destinationQueryHelper;
        IPackageQuery packageQueryHelper;

        public AiringService(IGetAiringQuery airingQueryHelper,
            IConfiguration configuration,
            IAiringSaveCommand airingSaveCommandHelper,
            IAiringDeleteCommand airingDeleteCommandHelper, IAiringMessagePusher airingMessagePusherCommandHelper,
            IQueueQuery queueQueryHelper,
            ITaskUpdater taskUpdaterCommand,
            IFileQuery fileQueryHelper,
            IDestinationQuery destinationQueryHelper,
            IPackageQuery packageQueryHelper)
        {
            this.airingQueryHelper = airingQueryHelper;
            this.airingSaveCommandHelper = airingSaveCommandHelper;
            this.airingDeleteCommandHelper = airingDeleteCommandHelper;
            this.airingMessagePusherCommandHelper = airingMessagePusherCommandHelper;
            this.queueQueryHelper = queueQueryHelper;
            this.taskUpdaterCommand = taskUpdaterCommand;
            this.fileQueryHelper = fileQueryHelper;
            this.configuration = configuration;
            this.destinationQueryHelper = destinationQueryHelper;
            this.packageQueryHelper = packageQueryHelper;
        }

        #region "Public method"
        public BLModel.Airing Delete(BLModel.Airing airing)
        {
            return
            airingDeleteCommandHelper.Delete(airing.ToDataModel<BLModel.Airing, DLModel.Airing>())
                .ToBusinessModel<DLModel.Airing, BLModel.Airing>();
        }

        public BLModel.Airing GetBy(string assetId, AiringCollection getFrom = AiringCollection.CurrentOrExpiredCollection)
        {
            return
            airingQueryHelper.GetBy(assetId, getFrom)
                .ToBusinessModel<DLModel.Airing, BLModel.Airing>();
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
            var titleIds = airing.Title.TitleIds
                .Where(t => t.Authority == "Turner" && t.Value != null)
                .Select(t => int.Parse(t.Value));

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



        public void AppendStatus(ref BLModel.Alternate.Long.Airing airing)
        {
            // initialize status property
            airing.Options.Status = new Status();

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
                    airing.Options.Status.Video = !files.Where(c => c.Video == true).IsNullOrEmpty();
                }
            }
            else
            {
                airing.Options.Status.Video = !airing.Options.Files.Where(c => c.Video == true).IsNullOrEmpty();
            }          
        }


        public void AppendPackage(ref BLModel.Alternate.Long.Airing airing, IEnumerable<Tuple<string, decimal>> acceptHeaders)
        {
            var titleIds = airing.Title.TitleIds.Where(title => (title.Authority.Equals("Turner") && isNumeric(title.Value)))
                            .Select(thisTitle => (getTitleId(thisTitle.Value)));

            //double check no bad titleIds are in the array.
            titleIds = titleIds.Where(title => (title != -1)).ToList();
            var destinations = airing.Flights.SelectMany(flight =>
                                                         flight.Destinations.Select(destination => destination.Name));
            var packages = packageQueryHelper.GetBy(titleIds.ToList(), destinations.ToList()).ToList();

            airing.Options.Packages = packages.ToViewModel<List<DLPackageModel.Package>, List<BLModel.Alternate.Package.Package>>();

            // verify the kind of serialization
            bool isXMLSerialization = (acceptHeaders.Count(c => c.Item1 == "application/xml") > 0) ? true : false;
            foreach (var package in airing.Options.Packages)
            {
                package.Data = isXMLSerialization ? package.Data : null;
                package.PackageData = !isXMLSerialization ? package.PackageData : null;

            }

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
                    }

                    var titleIds = airing.Title.TitleIds.Where(t => t.Authority == "Turner").Select(t => int.Parse(t.Value)).ToList();

                    if (property.TitleIds.Any())
                    {
                        if (titleIds.Any())
                        {
                            if (!property.TitleIds.Any(titleIds.Contains))
                            {
                                propertiesToRemove.Add(property);
                            }
                        }
                        else
                        {
                            propertiesToRemove.Add(property);
                        }
                    }

                    if (property.SeriesIds.Any())
                    {
                        if (airing.Title.Series.Id.HasValue)
                        {
                            if (!property.SeriesIds.Contains(airing.Title.Series.Id.Value))
                            {
                                propertiesToRemove.Add(property);
                            }
                        }
                        else
                        {
                            propertiesToRemove.Add(property);
                        }
                    }
                }

                destination.Properties = destination.Properties.Where(p => !propertiesToRemove.Contains(p)).ToList();
            }
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
            RestClient client = new RestClient(configuration.Get("flowUrl"));
            var titles = new List<BLModel.Alternate.Title.Title>();

            foreach (var list in listsOfTitleIds)
            {              
                var request = new RestRequest("/v2/title/{ids}?api_key={api_key}", Method.GET);
                request.AddUrlSegment("ids", string.Join(",", list));
                request.AddUrlSegment("api_key", configuration.Get("flowApiKey"));

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
            theClient.ExecuteAsync<List<BLModel.Alternate.Title.Title>>(theRequest, response => {
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

        #endregion

    }
}