using System;
using System.Collections.Generic;
using BLModel = OnDemandTools.Business.Modules.Pathing.Model;
//using DLModel = OnDemandTools.DAL.Modules.Product.Model;

namespace OnDemandTools.Business.Modules.Pathing
{
    public interface IPathingService
    {
        List<BLModel.PathTranslation> GetAll();

        /// <summary>
        /// Gets path translations by source base URL and brand.
        /// </summary>
        /// <param name="sourceBaseUrl">The source base URL.</param>
        /// <param name="sourceBrand">The source brand.</param>
        /// <returns></returns>
        List<BLModel.PathTranslation> GetBySourceBaseUrlAndBrand(String sourceBaseUrl, String sourceBrand);

        /// <summary>
        /// Gets path translations by source base URL.
        /// </summary>
        /// <param name="sourceBaseUrl">The source base URL.</param>
        /// <returns></returns>
        List<BLModel.PathTranslation> GetBySourceBaseUrl(String sourceBaseUrl);

    }
}
