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
using OnDemandTools.DAL.Modules.Destination.Comparer;
using MongoDB.Bson;
using OnDemandTools.DAL.Modules.Destination.Command;

namespace OnDemandTools.Business.Modules.Destination
{
    /// <summary>
    /// Destination service
    /// </summary>
    /// <seealso cref="OnDemandTools.Business.Modules.Destination.IDestinationService" />
    public class DestinationService : IDestinationService
    {
        IDestinationQuery destinationHelper;
        IDestinationCommand destinationCommand;
        AppSettings appSettings;


        private const string Episode = "{TITLE_EPISODE_NUMBER}";

        private const string AiringStorylineLong = "{AIRING_STORYLINE_LONG}";
        private const string AiringStorylineShort = "{AIRING_STORYLINE_SHORT}";
        private const string TitleStorylinePattern = @"{TITLE_STORYLINE([\w\W\d ]+)}";

        public DestinationService(IDestinationQuery destinationHelper, IDestinationCommand destinationCommand, AppSettings appSettings)
        {
            this.destinationHelper = destinationHelper;
            this.destinationCommand = destinationCommand;
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

        /// <summary>
        /// Get Destination properties and deliverables to airing
        /// </summary>
        /// <param name="airing">airing</param>    

        public void GetAiringDestinationRelatedData(ref Airing.Model.Airing airing)
        {
            foreach (var flight in airing.Flights)
            {
                var destinationNames = flight.Destinations.Select(d => d.Name).Distinct().ToList();

                var dbDestinations = (destinationHelper.GetByDestinationNames(destinationNames)
                                                         .Distinct(new DestinationDataModelComparer()).ToList().
                                                          ToViewModel<List<DLModel.Destination>, List<BLModel.Destination>>());

                foreach (Airing.Model.Destination des in flight.Destinations)
                {
                    foreach (BLModel.Destination dbDes in dbDestinations)
                    {
                        if (dbDes.Name == des.Name)
                        {
                            des.Categories = dbDes.Categories.ToBusinessModel<List<BLModel.Category>, List<Category>>();
                            des.Properties = dbDes.Properties.ToBusinessModel<List<BLModel.Property>, List<Property>>();
                            des.Deliverables = dbDes.Deliverables.ToBusinessModel<List<BLModel.Deliverable>, List<Deliverable>>();
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Fliter Deestination properties, deliverables and categories.Then transform the tokens.
        ///  Finally, append results to the provided airing
        /// </summary>
        /// <param name="airing">airing</param>
        public void FilterDestinationPropertiesDeliverablesAndCategoriesAndTransformTokens(ref Airing.Model.Airing airing)
        {
            foreach (var flight in airing.Flights)
            {
                foreach (var destination in flight.Destinations)
                {
                    // Filter out the properties based on selected brand(s) and title(s)
                    var propertiesToRemove = new List<Property>();

                    foreach (var property in destination.Properties)
                    {
                        if (property.Brands.Any() && !property.Brands.Contains(airing.Network)) //airing and property brand should be same
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

                    destination.Properties = destination.Properties.Where(p => !propertiesToRemove.Contains(p)).ToList(); //remove  unmatched  properties


                    // Next filter out categories based on selected brand(s) and title(s)
                    List<Category> categoriesToRemove = new List<Category>();

                    foreach (Category cat in destination.Categories)
                    {
                        // Verify if category filter (brand) and airing brand match. If not, add it to list to be removed later
                        if (cat.Brands.Any() && !cat.Brands.Contains(airing.Network))
                        {
                            categoriesToRemove.Add(cat);
                            continue;
                        }

                        // Next, verify if category filter has any title or series associated with it. If so, verify that it matches that of the airing.
                        // If it doesn't match, add it to list to be removed later
                        if (cat.TitleIds.Any() && cat.SeriesIds.Any())
                        {
                            if (!IsCategoryTitleIdsAssociatedwithAiringTitleIds(airing, cat) && !IsCategorySeriesIdsAssociatedwithAiringSeriesIds(airing, cat))
                            {
                                categoriesToRemove.Add(cat);
                            }
                        }

                        if (cat.TitleIds.Any())
                        {
                            if (!IsCategoryTitleIdsAssociatedwithAiringTitleIds(airing, cat))
                            {
                                categoriesToRemove.Add(cat);
                            }
                        }

                         if (cat.SeriesIds.Any())
                        {
                            if (!IsCategorySeriesIdsAssociatedwithAiringSeriesIds(airing, cat))
                            {
                                categoriesToRemove.Add(cat);
                            }
                        }
                    }

                    // Finally remove categories whose filter criteria (series & title) doesn't match that of the airing
                    destination.Categories = destination.Categories.Where(p => !categoriesToRemove.Contains(p)).ToList();

                    // Check if the tokens identified in deliverables or properties require flow data
                    bool isFlowDataRequired = CheckDelvierablesandPropertiesRequiresFlowTitle(destination); 
                    if (isFlowDataRequired)
                    {
                        GetFlowValuesforAiring(ref airing);
                    }

                    // Now transform/subsitute tokens identified in properties & deliverables with actual values
                    new DeliverableFormatter(airing).Format(destination); // destinations passed by reference for formatting to transform Deliverable tokens
                    new PropertyFormatter(airing).Format(destination); // destinations passed by reference for formatting to transform property tokens
                    destination.Deliverables = destination.Deliverables.Where(x => x.Value != string.Empty).ToList(); // remove the empty tokens
                }
            }
        }

        /// <summary>
        /// Save destnation to collection
        /// </summary>
        /// <param name="model">destination model</param>
        public BLModel.Destination Save(BLModel.Destination model)
        {
            DLModel.Destination dataModel = model.ToDataModel<BLModel.Destination, DLModel.Destination>();

            dataModel = destinationCommand.Save(dataModel);

            return dataModel.ToBusinessModel<DLModel.Destination, BLModel.Destination>();
        }

        /// <summary>
        /// Update the categories to the destination collection
        /// </summary>
        /// <param name="destination">The destiination model.</param>
        /// <returns></returns>
        public BLModel.Destination SaveDestinationCategory(BLModel.Destination destination)
        {
            foreach (var category in destination.Categories)
            {
                DLModel.Category dataModel = category.ToDataModel<BLModel.Category, DLModel.Category>();

                destinationCommand.UpdateDestinationCategory(destination.Name, dataModel);
            }

            return GetByName(destination.Name);
        }

        /// <summary>
        /// Remove destnation from collection using ObjectID
        /// </summary>
        /// <param name="id">Object Id</param>
        public void Delete(string id)
        {
            destinationCommand.Delete(id);
        }

        /// <summary>
        /// Get title  names for titleIds from flow
        /// </summary>
        /// <param name="titleIds"> title Ids</param>
        /// <returns>titles</returns>
        public List<Airing.Model.Alternate.Title.Title> GetTitlesNameFor(List<int> titleIds)
        {
            var titles = GetFlowTitlesFor(titleIds);
            return titles;
        }

        /// <summary>
        /// delete destination category by name
        /// </summary>
        /// <param name="name">category name</param>
        public void DeleteCategoryByName(string name)
        {
             destinationCommand.DeleteCategoryByName(name);
        }

        #region PRIVATE METHODS
        private void GetFlowValuesforAiring(ref Airing.Model.Airing airing)
        {
            List<int> titleIds = airing.Title.TitleIds
                .Where(t => t.Authority == "Turner" && t.Value != null)
                .Select(t => int.Parse(t.Value)).ToList();
            titleIds.AddRange(airing.Title.RelatedTitleIds.Where(t => t.Authority == "Turner").Select(t => int.Parse(t.Value)).ToList());
            List<Airing.Model.Alternate.Title.Title> titles = new List<Airing.Model.Alternate.Title.Title>();

            if (!airing.FlowTitleData.Any()) // Fetch when flowData is not available
            {
                titles = GetFlowTitlesFor(titleIds);
                airing.FlowTitleData = titles;
            }

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
            theClient.ExecuteAsync<List<Airing.Model.Alternate.Title.Title>>(theRequest, response =>
          {
              tcs.SetResult(response.Data);
          });
            return tcs.Task;
        }

        private bool CheckDelvierablesandPropertiesRequiresFlowTitle(Airing.Model.Destination destination)
        {
            foreach (Property p in destination.Properties)
            {
                if (p.Value == Episode || p.Value == AiringStorylineLong || p.Value == AiringStorylineShort)
                {
                    return true;
                }

                var match = Regex.Match(p.Value, TitleStorylinePattern);
                if (match.Success)
                    return true;
            }

            foreach (Deliverable p in destination.Deliverables)
            {
                if (p.Value == Episode || p.Value == AiringStorylineLong || p.Value == AiringStorylineShort)
                {
                    return true;
                }

                var match = Regex.Match(p.Value, TitleStorylinePattern);
                if (match.Success)
                    return true;
            }
            return false;
        }

        /// Verify if the title filter associated with the category matches that of the airing.
        /// If yes, return true; else, false
        private bool IsCategoryTitleIdsAssociatedwithAiringTitleIds(Airing.Model.Airing airing, Category category)
        {
            List<int> titleIds = airing.Title.TitleIds.Where(t => t.Authority == "Turner").Select(t => int.Parse(t.Value)).ToList();

            if (titleIds.Any())
            {
                return (category.TitleIds.Any(titleIds.Contains));
            }
            else
            {
                return false;
            }
        }

        /// Verify if the series filter associated with the category matches that of the airing.
        /// If yes, return true; else, false
        private bool IsCategorySeriesIdsAssociatedwithAiringSeriesIds(Airing.Model.Airing airing, Category category)
        {
            if (airing.Title.Series != null && airing.Title.Series.Id.HasValue)
            {
                return (category.SeriesIds.Contains(airing.Title.Series.Id.Value));

            }
            else
            {
                return false;
            }
        }


        private bool IsPropertyTitleIdsAssociatedwithAiringTitleIds(Airing.Model.Airing airing, Property property)
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

        private bool IsPropertySeriesIdsAssociatedwithAiringSeriesIds(Airing.Model.Airing airing, Property property)
        {
            if (airing.Title.Series != null && airing.Title.Series.Id.HasValue)
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

        #endregion
    }
}
