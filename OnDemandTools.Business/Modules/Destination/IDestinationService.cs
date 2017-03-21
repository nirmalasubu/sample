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
        /// 
        /// </summary>
        /// <param name="airing"></param>
        void MapAiringDetinationProperties(ref Airing.Model.Airing airing);

     }
}
