﻿using System;
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

    }
}
