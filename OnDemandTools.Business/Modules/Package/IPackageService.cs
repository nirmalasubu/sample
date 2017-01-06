using System;
using System.Collections.Generic;
using BLModel = OnDemandTools.Business.Modules.Package.Model;
using OnDemandTools.Common.Configuration;

namespace OnDemandTools.Business.Modules.Package
{
    public interface IPackageService
    {

        /// <summary>
        /// Saves the package.
        /// </summary>
        /// <param name="packageDataModel">The package data model.</param>
        /// <param name="updateHistorical">if set to <c>true</c> [update historical].</param>
        /// <returns></returns>
        BLModel.Package SavePackage(BLModel.Package packageDataModel, bool updateHistorical = true);

        /// <summary>
        /// Gets the package that matches all criteria - titleIds, destinationCode,
        /// type - explicitly. If more than one package is found then the first one
        /// will be returned
        /// </summary>
        /// <param name="titleIds">The title ids.</param>
        /// <param name="destinationCode">The destination code.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        BLModel.Package GetBy(List<int> titleIds, string destinationCode, string type);

        /// <summary>
        /// Deletes the specified package.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <param name="updateHistorical">if set to <c>true</c> [update historical].</param>
        /// <returns></returns>
        Boolean Delete(ref BLModel.Package package, bool updateHistorical = true);
    }
}
