using System;
using System.Collections.Generic;
using BLAiringModel = OnDemandTools.Business.Modules.Airing.Model;

namespace OnDemandTools.Business.Modules.Product
{
    public interface IProductService
    {
        /// <summary>
        /// Gets all products
        /// </summary>
        /// <returns></returns>
        List<Model.Product> GetAll();

        /// <summary>
        /// Gets products by matching tags
        /// </summary>
        /// <param name="tags">The tags.</param>
        /// <returns></returns>
        List<Model.Product> GetByTags(List<string> tags);

        /// <summary>
        /// Gets products by matching product ids
        /// </summary>
        /// <param name="productIds">The product ids.</param>
        /// <returns></returns>
        List<Model.Product> GetByProductIds(List<Guid> productIds);

        /// <summary>
        /// Converts products specified in the given 
        /// airing to its corresponding destinations
        /// </summary>
        /// <param name="airing">The airing.</param>
        void ProductDestinationConverter(ref BLAiringModel.Airing airing);

        /// <summary>
        /// Save product to collection
        /// </summary>
        /// <param name="model">product model</param>
        Model.Product Save(Model.Product model);

        /// <summary>
        /// Remove product from collection using ObjectID
        /// </summary>
        /// <param name="id">Object Id</param>
        void Delete(string id);

        /// <summary>
        /// Gets the product by id
        /// </summary>
        /// <param name="externalId">product id</param>
        /// <returns></returns>
        Model.Product GetById(string externalId);
    }
}
