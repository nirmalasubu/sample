using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.Destination
{
    public interface IDestinationService
    {
        /// <summary>
        /// Gets all destinations
        /// </summary>
        /// <returns></returns>
        List<Model.Destination> GetAll();

        /// <summary>
        /// Returns first destination that match the given name
        /// </summary>
        /// <param name="destinationName">Name of the destination.</param>
        /// <returns></returns>
        Model.Destination GetByName(string destinationName);

        /// <summary>
        /// Gets destinations by mapping identifier.
        /// </summary>
        /// <param name="mappingId">The mapping identifier.</param>
        /// <returns></returns>
        List<Model.Destination> GetByMappingId(int mappingId);

        /// <summary>
        /// Gets destinations by product (non unique) identifier. 
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns></returns>
        List<Model.Destination> GetByProductId(Guid productId);

        /// <summary>
        /// Gets destinations that matches the given names
        /// </summary>
        /// <param name="names">The names.</param>
        /// <returns></returns>
        List<Model.Destination> GetByDestinationNames(List<string> names);

        /// <summary>
        /// Gets products by product ids.
        /// </summary>
        /// <param name="productIds">The product ids.</param>
        /// <returns></returns>
        List<Model.Destination> GetByProductIds(IList<Guid> productIds);

        /// <summary>
        /// Retrieve destination related data - properties, deliverables and categories, and augment it to airing
        /// </summary>
        /// <param name="airing"></param>
        void GetAiringDestinationRelatedData(ref Airing.Model.Airing airing);

        /// <summary>
        /// Fliter and Transform Destination properties and deliverables to airing
        /// </summary>
        /// <param name="airing">airing</param>
        void FilterDestinationPropertiesDeliverablesAndCategoriesAndTransformTokens(ref Airing.Model.Airing airing);

        /// <summary>
        /// Remove destnation from collection using ObjectID
        /// </summary>
        /// <param name="id">Object Id</param>
        void Delete(string id);

        /// <summary>
        /// Save destnation to collection
        /// </summary>
        /// <param name="model">destination model</param>
        Model.Destination Save(Model.Destination model);

        /// <summary>
        /// Get title  names for titleIds from flow
        /// </summary>
        /// <param name="titleIds"> title Ids</param>
        /// <returns>titles</returns>
        List<Airing.Model.Alternate.Title.Title> GetTitlesNameFor(List<int> titleIds);

     }
}
