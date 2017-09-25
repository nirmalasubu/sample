using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Business.Adapters.ActiveDirectoryQuery;
using OnDemandTools.Business.Modules.User;
using OnDemandTools.Business.Modules.UserPermissions;
using OnDemandTools.Common.Configuration;
using OnDemandTools.Common.Model;
using OnDemandTools.Web.Models.UserPermissions;
using BLModel = OnDemandTools.Business.Modules.UserPermissions.Model;
using System.Linq;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnDemandTools.Web.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        IUserPermissionService _userSvc;
        IUserHelper _oldUser;
        IActiveDirectoryQuery _adQuery;

        public UserController(IUserPermissionService userSvc, IUserHelper oldUser, IActiveDirectoryQuery adQuery)
        {
            _userSvc = userSvc;
            _oldUser = oldUser;
            _adQuery = adQuery;
        }

        // GET: api/values
        [Authorize]
        [HttpGet]
        public UserPermission Get()
        {
            UserPermission user = _userSvc.GetByUserName(HttpContext.User.Identity.Name)
                .ToViewModel<Business.Modules.UserPermissions.Model.UserPermission, UserPermission>();

            return user;
        }

        [HttpGet("migrate")]
        public string Migrate()
        {
            var users = _oldUser.GetUsers();
            var modules = _userSvc.GetAllPortalModules();

            foreach (var user in users)
            {
                var existingUser = _userSvc.GetByUserName(string.IsNullOrEmpty(user.EmailAddress) ? user.UserName : user.EmailAddress);

                if (existingUser != null) continue;

                BLModel.UserPermission newPermission = new BLModel.UserPermission();

                //If email address exists then it is Portal user otherwise "System/API" user
                if (!string.IsNullOrEmpty(user.EmailAddress) && user.EmailAddress.Contains("@"))
                {
                    var adUser = _adQuery.GetUserByEmailId(user.EmailAddress);

                    FillPortalUser(user, adUser, newPermission, modules);

                    FillApiUser(user, newPermission);
                }
                else //System or API user
                {
                    newPermission.UserType = UserType.Api;
                    newPermission.UserName = user.UserName;
                    newPermission.Notes = user.Description;
                    FillApiUser(user, newPermission);
                }

                newPermission.CreatedBy = user.CreatedBy;
                newPermission.ModifiedBy = user.ModifiedBy;
                newPermission.CreatedDateTime = user.CreatedDateTime;
                newPermission.ModifiedDateTime = user.ModifiedDateTime;

                _userSvc.Save(newPermission);
            }

            return "success";
        }

        private void FillPortalUser(UserIdentity user, AzureAdUser adUser, BLModel.UserPermission newPermission, List<BLModel.PortalModule> modules)
        {
            newPermission.UserType = UserType.Portal;
            newPermission.FirstName = adUser.givenName;
            newPermission.LastName = adUser.surname;
            newPermission.Notes = user.Description;
            newPermission.UserName = user.EmailAddress;

            if (!string.IsNullOrEmpty(adUser.mobilePhone))
            {
                newPermission.PhoneNumber = adUser.mobilePhone;
            }
            else if (adUser.businessPhones.Any())
            {
                newPermission.PhoneNumber = adUser.businessPhones.First();
            }

            newPermission.Portal = new BLModel.Portal();

            newPermission.Portal.IsActive = true;

            newPermission.Portal.IsAdmin = user.Claims.Any(e => e.Value.ToLower() == "admin");

            //Non Admin
            if (newPermission.Portal.IsAdmin == false)
            {
                foreach (var module in modules)
                {
                    if (module.ModuleType == "User")
                        newPermission.Portal.ModulePermissions[module.ModuleName] = module.ModulePermission;
                    else
                        newPermission.Portal.ModulePermissions[module.ModuleName] = new BLModel.Permission(false);
                }
            }
            else
            {
                foreach (var module in modules)
                {
                    newPermission.Portal.ModulePermissions[module.ModuleName] = new BLModel.Permission(true);
                }
            }



        }

        private void FillApiUser(UserIdentity user, BLModel.UserPermission newPermission)
        {
            newPermission.Api = new BLModel.Api();

            newPermission.Api.ApiKey = user.ApiKey;
            newPermission.Api.Destinations = user.Destinations;
            newPermission.Api.IsActive = true;
            newPermission.Api.Brands = user.Brands;

            var isAdmin = user.Claims.Any(e => e.Value.ToLower() == "admin");

            newPermission.Api.BrandPermitAll = isAdmin;
            newPermission.Api.DestinationPermitAll = isAdmin;

            var claims = new List<string>();
            if (user.Claims.Any(e => e.Value.ToLower() == "get"))
            {
                claims.Add("get");
            }

            if (user.Claims.Any(e => e.Value.ToLower() == "post"))
            {
                claims.Add("post");
            }

            if (user.Claims.Any(e => e.Value.ToLower() == "delete"))
            {
                claims.Add("delete");
            }

            newPermission.Api.Claims = claims;


        }


    }
}
