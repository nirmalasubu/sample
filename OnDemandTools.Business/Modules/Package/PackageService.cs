using System;
using DLModel = OnDemandTools.DAL.Modules.Package.Model;
using OnDemandTools.DAL.Modules.Package.Queries;
using OnDemandTools.DAL.Modules.Package.Commands;
using BLModel = OnDemandTools.Business.Modules.Package.Model;
using OnDemandTools.Common.Model;
using System.Collections.Generic;
using System.Linq;
using OnDemandTools.Common.Configuration;

namespace OnDemandTools.Business.Modules.Package
{
    public class PackageService : IPackageService
    {
        IPackageCommand packagePersist;
        IPackageQuery packageQuery;
        IApplicationContext cntx;

        public PackageService(IPackageCommand packagePersist, IPackageQuery packageQuery, IApplicationContext cntx)
        {
            this.packagePersist = packagePersist;
            this.packageQuery = packageQuery;
            this.cntx = cntx;
        }

        /// <summary>
        /// Deletes the specified package.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <param name="updateHistorical">if set to <c>true</c> [update historical].</param>
        /// <returns></returns>

        public Boolean Delete(ref BLModel.Package package, bool updateHistorical = true)
        {
            DLModel.Package existingPkg = packageQuery.GetBy(package.TitleIds.ToList(), package.DestinationCode, package.Type);
            var user = cntx.GetUser();

            if(existingPkg != null)
            {
                package = existingPkg.ToBusinessModel<DLModel.Package, BLModel.Package>();
                existingPkg.ModifiedBy = user.UserName;
                existingPkg.ModifiedDateTime = DateTime.UtcNow;
                packagePersist.Delete(existingPkg, user.UserName, updateHistorical);
                return true;
            }

            return false;
         }

        /// <summary>
        /// Gets the package that matches all criteria - titleIds, destinationCode,
        /// type - explicitly. If more than one package is found then the first one
        /// will be returned
        /// </summary>
        /// <param name="titleIds">The title ids.</param>
        /// <param name="destinationCode">The destination code.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public BLModel.Package GetBy(List<int> titleIds, string destinationCode, string type)
        {
            return (
            packageQuery.GetBy(titleIds, destinationCode, type)
                .ToBusinessModel<DLModel.Package, BLModel.Package>());            
        }

                /// <summary>
        /// Gets the package that matches all criteria - titleIds, destinationCode,
        /// type - explicitly. If more than one package is found then the first one
        /// will be returned
        /// </summary>
        /// <param name="contentIds">The content ids.</param>
        /// <param name="destinationCode">The destination code.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public BLModel.Package GetBy(List<string> contentIds, string destinationCode, string type)
        {
            return (
            packageQuery.GetBy(contentIds, destinationCode, type)
                .ToBusinessModel<DLModel.Package, BLModel.Package>());            
        }

        /// <summary>
        /// Saves the package.
        /// </summary>
        /// <param name="package">The package business model.</param>
        /// <param name="updateHistorical">if set to <c>true</c> [update historical].</param>
        /// <returns></returns>
        public BLModel.Package SavePackage(BLModel.Package package, bool updateHistorical)
        {
            var user = cntx.GetUser();
            package.CreatedBy = user.UserName;
            package.CreatedDateTime = DateTime.UtcNow;
            package.ModifiedBy = user.UserName;
            package.ModifiedDateTime = DateTime.UtcNow;

            return 
            (packagePersist.Save(package.ToDataModel<BLModel.Package, DLModel.Package>(), user.UserName, updateHistorical)
                .ToBusinessModel<DLModel.Package, BLModel.Package>());
        }
    }
}
