using OnDemandTools.Common.Model;
using OnDemandTools.DAL.Modules.Product.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using BLModel = OnDemandTools.Business.Modules.Product.Model;
using DLModel = OnDemandTools.DAL.Modules.Product.Model;

namespace OnDemandTools.Business.Modules.Product
{
    public class ProductService : IProductService
    {
        IProductQuery productHelper;

        public ProductService(IProductQuery productHelper)
        {
            this.productHelper = productHelper;
        }


        /// <summary>
        /// Gets all products
        /// </summary>
        /// <returns></returns>
        public List<BLModel.Product> GetAll()
        {
            return (productHelper.Get().ToList<DLModel.Product>()
                .ToBusinessModel<List<DLModel.Product>, List<BLModel.Product>>());
        }

        /// <summary>
        /// Gets products by matching product ids
        /// </summary>
        /// <param name="productIds">The product ids.</param>
        /// <returns></returns>
        public List<Model.Product> GetByProductIds(List<Guid> productIds)
        {
            return (productHelper.GetByProductIds(productIds).ToList<DLModel.Product>()
                .ToBusinessModel<List<DLModel.Product>, List<BLModel.Product>>()) ;
        }

        /// <summary>
        /// Gets products by matching tags
        /// </summary>
        /// <param name="tags">The tags.</param>
        /// <returns></returns>
        public List<Model.Product> GetByTags(List<string> tags)
        {
            var k = productHelper.GetByTags(tags).ToList<DLModel.Product>()
                .ToBusinessModel<List<DLModel.Product>, List<BLModel.Product>>();

            return (productHelper.GetByTags(tags).ToList<DLModel.Product>()
                .ToBusinessModel<List<DLModel.Product>,List<BLModel.Product>>());
        }
    }
}
