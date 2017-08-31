using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Web.Models.UserPermissions;
using OnDemandTools.Business.Modules.UserPermissions;
using OnDemandTools.Common.Model;
using BLModel = OnDemandTools.Business.Modules.UserPermissions.Model;
using Microsoft.AspNetCore.Authorization;
using OnDemandTools.Business.Modules.Queue;
using System.Collections;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnDemandTools.Web.Controllers
{
    [Route("api/[controller]")]
    public class UserPermissionController : Controller
    {

        IUserPermissionService _service;
        IQueueService _queueSvc;

        public UserPermissionController(IUserPermissionService service, IQueueService queueSvc)
        {
            _service = service;
            _queueSvc = queueSvc;
        }


        [Authorize]
        [HttpGet("{type}")]
        public IEnumerable<UserPermission> Get(string type)
        {
            var permissionLists = _service.GetAll(type == "system" ? UserType.Api : UserType.Portal).OrderBy(e => e.UserName).ToList()
            .ToViewModel<List<BLModel.UserPermission>, List<UserPermission>>();



            if (type == "system")
            {
                foreach (var permission in permissionLists)
                {
                    if (!string.IsNullOrEmpty(permission.Api.TechnicalContactId))
                        permission.Api.TechnicalContactUser = _service.GetById(permission.Api.TechnicalContactId).ToViewModel<BLModel.UserPermission, UserPermission>();
                    if (!string.IsNullOrEmpty(permission.Api.FunctionalContactId))
                        permission.Api.FunctionalContactUser = _service.GetById(permission.Api.FunctionalContactId).ToViewModel<BLModel.UserPermission, UserPermission>();
                }
            }
            else
            {
                var modules = _service.GetAllPortalModules();

                foreach (var module in modules)
                {
                    foreach (var permission in permissionLists)
                    {
                        if (!permission.Portal.ModulePermissions.ContainsKey(module.ModuleName))
                        {
                            permission.Portal.ModulePermissions.Add(module.ModuleName, new Permission(permission.Portal.IsAdmin));
                        }
                    }
                }

                var queues = _queueSvc.GetQueues();

                foreach (var queue in queues)
                {
                    foreach (var permission in permissionLists)
                    {
                        if (!permission.Portal.DeliveryQueuePermissions.ContainsKey(queue.Name))
                        {
                            permission.Portal.DeliveryQueuePermissions.Add(queue.Name, new Permission(permission.Portal.IsAdmin));
                        }
                    }
                }

            }

            return permissionLists;
        }

        [Authorize]
        [HttpGet("getcontactforbyuserid/{id}")]
        public UserContactFor GetContactForByUserId(string id)
        {
            IList <BLModel.UserPermission> lstUser = _service.GetContactForByUserId(id);
            UserContactFor contactfor = new UserContactFor();
            contactfor.TechnicalContactFor = lstUser.Where(s => s.Api.TechnicalContactId == id).Select(p => p.UserName).ToList();
            contactfor.FunctionalContactFor = lstUser.Where(s => s.Api.FunctionalContactId == id).Select(p => p.UserName).ToList();
            return contactfor;
        }

        [Authorize]
        [HttpPost]
        public UserPermission Post([FromBody]UserPermission viewModel)
        {


            if (string.IsNullOrEmpty(viewModel.Id))
            {
                viewModel.Api.ApiKey = Guid.NewGuid().ToString();
                viewModel.CreatedDateTime = DateTime.UtcNow;
                viewModel.CreatedBy = HttpContext.User.Identity.Name;
                if (viewModel.Portal.IsActive)
                {
                    viewModel.ActiveDateTime = DateTime.UtcNow;
                }
            }
            else
            {
                viewModel.ModifiedDateTime = DateTime.UtcNow;
                viewModel.ModifiedBy = HttpContext.User.Identity.Name;
            }
            BLModel.UserPermission model = _service.Save(viewModel.ToBusinessModel<UserPermission, BLModel.UserPermission>());


            return model.ToViewModel<BLModel.UserPermission, UserPermission>();
        }

        [Authorize]
        [HttpGet("newuserpermission")]
        public UserPermission GetEmptyModel()
        {
            var model = new UserPermission
            {
                UserName = string.Empty,
                FirstName = string.Empty,
                LastName = string.Empty,
                PhoneNumber = string.Empty,
                Extension = string.Empty,
                Notes = string.Empty,
                Portal = new Portal
                {
                    IsActive = true,
                },
                Api = new Api { ApiKey = "" }
            };

            var modules = _service.GetAllPortalModules();

            foreach (var module in modules)
            {
                model.Portal.ModulePermissions[module.ModuleName] = new Permission();
            }

            var queues = _queueSvc.GetQueues();

            foreach (var queue in queues)
            {
                model.Portal.DeliveryQueuePermissions[queue.Name] = new Permission();
            }

            return model;
        }
    }
}
