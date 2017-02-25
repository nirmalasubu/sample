using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using OnDemandTools.Business.Modules.Package;
using System;
using System.Collections.Generic;
using System.Net.Http;
using OnDemandTools.API.v1.Models.Package;
using OnDemandTools.API.Helpers;
using AutoMapper;
using FluentValidation.Results;
using FluentValidation;
using OnDemandTools.Business.Modules.Package.Model;
using System.Linq;
using OnDemandTools.Business.Modules.Queue;

namespace OnDemandTools.API.v1.Routes
{
    public class PackageRoutes : NancyModule
    {
        private readonly IPackageService packageSvc;
        private readonly IQueueService queueSvc;
       

        public PackageRoutes(
            IPackageService packageSvc,
            PackageValidator validator,
            IQueueService queueSvc)
            : base("v1")
        {
            this.RequiresAuthentication();
            this.packageSvc = packageSvc;
            this.queueSvc = queueSvc;          

            #region POST
            Post("/package", _ =>
            {
                try
                {
                    this.RequiresClaims(c => c.Type == HttpMethod.Post.Verb());

                    // Bind POST request to data contract
                    var request = this.Bind<PackageRequest>();

                    // Translate data contract to business model
                    var pkgBusiness = Mapper.Map<Package>(request);

                    // Verify if there are any validation errors. If so, return error                    
                    List<ValidationResult> results = new List<ValidationResult>();
                    results.Add(validator.Validate(pkgBusiness, ruleSet: PackageValidatorRuleSet.PostPackage.ToString()));
                    if (results.Where(c => (!c.IsValid)).Count() > 0)
                    {
                        // Return status
                        return Negotiate.WithModel(results.Where(c => (!c.IsValid))

                                    .Select(c => c.Errors.Select(d => d.ErrorMessage)))
                                    .WithStatusCode(HttpStatusCode.BadRequest);
                    }

                    // Save package
                    var savedPkg = packageSvc.SavePackage(pkgBusiness);

                    // Send notification to subscribed queues
                    DeterminePackageReset(pkgBusiness);

                    // Return response
                    return Mapper.Map<PackageRequest>(savedPkg);                   
                }
                catch (Exception e)
                {
                    throw e;
                }
            });
            #endregion

            #region Delete
            Delete("/package", _ =>
            {
                this.RequiresClaims(c => c.Type == HttpMethod.Delete.Verb());
                var request = this.Bind<PackageRequest>();

                // Translate data contract to package business model
                var pkg = Mapper.Map<Package>(request);

                // validate
                List<ValidationResult> results = new List<ValidationResult>();
                results.Add(validator.Validate(pkg, ruleSet: PackageValidatorRuleSet.DeletePackage.ToString()));

                // Verify if there are any validation errors. If so, return error
                if (results.Where(c => (!c.IsValid)).Count() > 0)
                {
                    // Return status
                    return Negotiate.WithModel(results.Where(c => (!c.IsValid))
                                .Select(c => c.Errors.Select(d => d.ErrorMessage)))
                                .WithStatusCode(HttpStatusCode.BadRequest);
                }
                               
                if (packageSvc.Delete(ref pkg))
                {  
                    DeterminePackageReset(pkg);
                    return new
                    {
                        Message = "Package deleted successfully"
                    };
                }
                else return Negotiate.WithModel("Package with provided data was not found.")
                                    .WithStatusCode(HttpStatusCode.BadRequest);

            });
            #endregion
        }

        private void DeterminePackageReset(Package pkg)
        {
            if(pkg.TitleIds != null && pkg.TitleIds.Count > 0) {
                ResetPackageQueues(pkg.TitleIds, pkg.DestinationCode);
                return;
            }
            if(pkg.ContentIds != null && pkg.ContentIds.Count > 0) {
                ResetPackageQueues(pkg.ContentIds, pkg.DestinationCode);
                return;
            }            
        }

        private void ResetPackageQueues(IList<string> contentIds, string destinationCode)
        {
            var packageQueues = queueSvc.GetPackageNotificationSubscribers();

            if (!packageQueues.Any()) return;

            var queueNames = packageQueues.Select(p => p.Name).ToList();

            queueSvc.FlagForRedelivery(queueNames, contentIds, destinationCode);
        }

        private void ResetPackageQueues(IList<int> titleIds, string destinationCode)
        {
            var packageQueues = queueSvc.GetPackageNotificationSubscribers();

            if (!packageQueues.Any()) return;

            var queueNames = packageQueues.Select(p => p.Name).ToList();

            queueSvc.FlagForRedelivery(queueNames, titleIds, destinationCode);
        }

    }
}
