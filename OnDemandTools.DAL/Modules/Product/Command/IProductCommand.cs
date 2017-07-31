using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using DLModel = OnDemandTools.DAL.Modules.Product.Model;

namespace OnDemandTools.DAL.Modules.Product.Command
{
    public interface IProductCommand
    {
        /// <summary>
        /// Save the product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        DLModel.Product Save(DLModel.Product product);

        /// <summary>
        /// Delete products by Id
        /// </summary>
        /// <param name="id"></param>
        void Delete(string id);

        /// <summary>
        /// Delete Content Tier by name
        /// </summary>
        /// <param name="name"></param>
        void DeleteContentTierByName(string name);
    }
}
