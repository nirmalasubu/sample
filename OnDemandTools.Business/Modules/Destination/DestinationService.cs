using System;
using System.Collections.Generic;
using System.Linq;
using BLModel = OnDemandTools.Business.Modules.Destination.Model;
using DLModel = OnDemandTools.DAL.Modules.Destination.Model;
using OnDemandTools.DAL.Modules.Destination.Queries;
using OnDemandTools.Common.Model;
using OnDemandTools.Business.Modules.Airing.Model;
using System.Text.RegularExpressions;
using RestSharp;
using OnDemandTools.Common.Configuration;
using OnDemandTools.Common.Extensions;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.Destination
{
    /// <summary>
    /// Destination service
    /// </summary>
    /// <seealso cref="OnDemandTools.Business.Modules.Destination.IDestinationService" />
    public class DestinationService : IDestinationService
    {
        IDestinationQuery destinationHelper;
        AppSettings appSettings;

        private const string TitleStorylinePattern = @"{TITLE_STORYLINE([\w\W\d ]+)}";

        public DestinationService(IDestinationQuery destinationHelper, AppSettings appSettings)
        {
            this.destinationHelper = destinationHelper;
            this.appSettings = appSettings;
        }


        /// <summary>
        /// Returns first destination that match the given name
        /// </summary>
        /// <param name="destinationName">Name of the destination.</param>
        /// <returns></returns>
        public BLModel.Destination GetByName(string destinationName)
        {
            return
                (destinationHelper.Get().First(d => d.Name == destinationName)               
                .ToBusinessModel<DLModel.Destination, BLModel.Destination>());
        }

        /// <summary>
        /// Gets destinations that matches the given names
        /// </summary>
        /// <param name="names">The names.</param>
        /// <returns></returns>
        public List<BLModel.Destination> GetByDestinationNames(List<string> names)
        {
            return (destinationHelper.GetByDestinationNames(names)
                .ToViewModel<List<DLModel.Destination>, List<BLModel.Destination>>());
        }


        /// <summary>
        /// Gets destinations by mapping identifier.
        /// </summary>
        /// <param name="mappingId">The mapping identifier.</param>
        /// <returns></returns>
        public List<BLModel.Destination> GetByMappingId(int mappingId)
        {
            return (destinationHelper.GetByMappingId(mappingId)
                .ToList<DLModel.Destination>()
                .ToViewModel<List<DLModel.Destination>, List<BLModel.Destination>>());
        }

        /// <summary>
        /// Gets destinations by product (non unique) identifier. 
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns></returns>
        public List<BLModel.Destination> GetByProductId(Guid productId)
        {
            return (destinationHelper.GetByProductId(productId)
               .ToList<DLModel.Destination>()
               .ToViewModel<List<DLModel.Destination>, List<BLModel.Destination>>());
        }

        /// <summary>
        /// Gets products by product ids.
        /// </summary>
        /// <param name="productIds">The product ids.</param>
        /// <returns></returns>
        public List<BLModel.Destination> GetByProductIds(IList<Guid> productIds)
        {
            return (destinationHelper.GetByProductIds(productIds)
               .ToList<DLModel.Destination>()
               .ToViewModel<List<DLModel.Destination>, List<BLModel.Destination>>());
        }

        /// <summary>
        /// Gets all destinations
        /// </summary>
        /// <returns></returns>
        public List<BLModel.Destination> GetAll()
        {
            return
                (destinationHelper.Get().ToList<DLModel.Destination>()
                .ToBusinessModel<List<DLModel.Destination>, List<BLModel.Destination>>());
        }

        public void MapAiringDetinationProperties(ref Airing.Model.Airing airing)
        {

            foreach (var flight in airing.Flights)
            {
                foreach (var destination in flight.Destinations)
                {
                    var propertiesToRemove = new List<BLModel.Property>();
                    BLModel.Destination destinationdetails = GetByName(destination.Name);

                    foreach (var property in destinationdetails.Properties)
                    {
                        if (property.Brands.Any() && !property.Brands.Contains(airing.Network))
                        {
                            propertiesToRemove.Add(property);
                            continue;
                        }

                        if (property.TitleIds.Any() && property.SeriesIds.Any())
                        {
                            if (!IsPropertyTitleIdsAssociatedwithAiringTitleIds(airing, property) && !IsPropertySeriesIdsAssociatedwithAiringSeriesIds(airing, property))
                            {
                                propertiesToRemove.Add(property);
                            }
                            continue;
                        }

                        if (property.TitleIds.Any())
                        {
                            if (!IsPropertyTitleIdsAssociatedwithAiringTitleIds(airing, property))
                            {
                                propertiesToRemove.Add(property);
                            }
                        }

                        if (property.SeriesIds.Any())
                        {
                            if (!IsPropertySeriesIdsAssociatedwithAiringSeriesIds(airing, property))
                            {
                                propertiesToRemove.Add(property);
                            }
                        }
                    }
                    destinationdetails.Properties = destinationdetails.Properties.Where(p => !propertiesToRemove.Contains(p)).ToList();
                    destination.Properties = destinationdetails.Properties.ToBusinessModel<List<BLModel.Property>,List<Airing.Model.Property>>();
                    destination.Deliverables= destinationdetails.Deliverables.ToBusinessModel<List<BLModel.Deliverable>, List<Airing.Model.Deliverable>>();
                    bool value=checkDelvierablesandPropertyrequiresFlowTitle(destination);
                    if(value)
                    {
                        GetFlowValuesforAiring(ref airing);
                    }
                    new Airing.Model.DeliverableFormatter(airing).Format(destination); // destinations passed by reference for formatting
                    new Airing.Model.PropertyFormatter(airing).Format(destination); // destinations passed by reference for formatting
                    destination.Deliverables = destination.Deliverables.Where(x => x.Value != string.Empty).ToList();
                }

            }
        }
           
        private void GetFlowValuesforAiring( ref Airing.Model.Airing airing)
        {
            List<int> titleIds = airing.Title.TitleIds
                .Where(t => t.Authority == "Turner" && t.Value != null)
                .Select(t => int.Parse(t.Value)).ToList();
            titleIds.AddRange(airing.Title.RelatedTitleIds.Where(t => t.Authority == "Turner").Select(t => int.Parse(t.Value)).ToList());

            var titles = GetFlowTitlesFor(titleIds);
            
                 airing.FlowTitleData = titles;
        }


        private List<Airing.Model.Alternate.Title.Title> GetFlowTitlesFor(IEnumerable<int> titleIds)
        {
            // The Distinct() here is neccessary because of a limitation we discovered.
            // The titles API returns only distinct FlowTitle objects per request.
            // If there are more then 5 titles (because of the partition), you can potentially
            // end up with duplicates. (Titles API limits us to 25. Consult titles api wiki.)
            var listsOfTitleIds = titleIds.Distinct().Partition(25).ToList();
            RestClient client = new RestClient(appSettings.GetExternalService("Flow").Url);
            var titles = new List<Airing.Model.Alternate.Title.Title>();

            foreach (var list in listsOfTitleIds)
            {
                var request = new RestRequest("/v2/title/{ids}?api_key={api_key}", Method.GET);
                request.AddUrlSegment("ids", string.Join(",", list));
                request.AddUrlSegment("api_key", appSettings.GetExternalService("Flow").ApiKey);

                System.Threading.Tasks.Task.Run(async () =>
                {
                    var rs = await GetFlowTitleAsync(client, request) as List<Airing.Model.Alternate.Title.Title>;
                    if (!rs.IsNullOrEmpty())
                    {
                        titles.AddRange(rs);
                    }

                }).Wait();
            }

            return titles;
        }

        private Task<List<Airing.Model.Alternate.Title.Title>> GetFlowTitleAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<List<Airing.Model.Alternate.Title.Title>>();
            theClient.ExecuteAsync<List<Airing.Model.Alternate.Title.Title >> (theRequest, response =>
            {
                tcs.SetResult(response.Data);
            });
            return tcs.Task;
        }

        private bool checkDelvierablesandPropertyrequiresFlowTitle(Airing.Model.Destination destination)
        {
            if(destination.Properties.Any(s => s.Value == "{TITLE_EPISODE_NUMBER}")){
                return true;
            }
          
            foreach(Property p in destination.Properties)
            {
                    var match = Regex.Match(p.Value, TitleStorylinePattern);
                if (match.Success)
                     return true;
            }

            if (destination.Deliverables.Any(s => s.Value == "{TITLE_EPISODE_NUMBER}"))
            {
                return true;
            }

            foreach (Deliverable p in destination.Deliverables)
            {
                var match = Regex.Match(p.Value, TitleStorylinePattern);
                if (match.Success)
                    return true;
            }
            return false;
        }

        //public void AppendTitle(Airing airing)
        //{
        //    List<int> titleIds = airing.Title.TitleIds
        //        .Where(t => t.Authority == "Turner" && t.Value != null)
        //        .Select(t => int.Parse(t.Value)).ToList();
        //    titleIds.AddRange(airing.Title.RelatedTitleIds.Where(t => t.Authority == "Turner").Select(t => int.Parse(t.Value)).ToList());

        //    var titles = GetFlowTitlesFor(titleIds);
        //    var primaryTitleId = airing.Title.TitleIds.FirstOrDefault(t => t.Primary);
        //    if (primaryTitleId != null)
        //    {
        //        var primaryTitle = titles.First(t => t.TitleId == int.Parse(primaryTitleId.Value));
        //        UpdateTitleFieldsFor(ref airing, primaryTitle);
        //    }

        //    airing.Options.Titles = titles;
        //}

        private bool IsPropertyTitleIdsAssociatedwithAiringTitleIds(Airing.Model.Airing airing, BLModel.Property property)
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

        private bool IsPropertySeriesIdsAssociatedwithAiringSeriesIds(Airing.Model.Airing airing, BLModel.Property property)
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

    }
}
