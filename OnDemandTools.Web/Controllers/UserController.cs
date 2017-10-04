using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Business.Modules.UserPermissions;
using OnDemandTools.Common.Model;
using OnDemandTools.Web.Models.UserPermissions;
using System.Collections.Generic;
using OnDemandTools.Web.Models.User;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnDemandTools.Web.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        IUserPermissionService _userSvc;

        IMigrateUsersAndSystem _migrateUserAndSystem;
        public UserController(IUserPermissionService userSvc, IMigrateUsersAndSystem migrateUserAndSystem)
        {
            _userSvc = userSvc;
            _migrateUserAndSystem = migrateUserAndSystem;
        }

        // GET: api/values
        [Authorize]
        [HttpGet]
        public UserPermission Get()
        {
            UserPermission userPermission = _userSvc.GetByUserName(HttpContext.User.Identity.Name)
                .ToViewModel<Business.Modules.UserPermissions.Model.UserPermission, UserPermission>();
           
            if (!user.Portal.IsActive)
            {
                user.Portal.ModulePermissions = new Dictionary<string, Permission>();
                user.Portal.DeliveryQueuePermissions = new Dictionary<string, Permission>();
            }

            return userPermission;
        }

        [Authorize]
        [HttpGet("getcontactforapidetailsbyuserid/{id}")]
        public UserContactForAPI GetContactForApiByUserId(string id)
        {
            IList<BLModel.UserPermission> lstUser = _userSvc.GetContactForByUserId(id);
            UserContactForAPI contactfor = new UserContactForAPI();
            contactfor.TechnicalContactFor = AddContactForDetails(lstUser.Where(s => s.Api.TechnicalContactId == id).ToList());
            contactfor.FunctionalContactFor = AddContactForDetails(lstUser.Where(s => s.Api.FunctionalContactId == id).ToList());
            return contactfor;
        }

        [HttpGet("migrate")]
        public List<string> Migrate()
        {
            return _migrateUserAndSystem.Migrate();
        }

 


        private List<UserContactForAPIDetail> AddContactForDetails(IList<BLModel.UserPermission> lstUser)
        {
            List<UserContactForAPIDetail> lstUserContactForDetail = new List<UserContactForAPIDetail>();

            foreach (BLModel.UserPermission user in lstUser)
            {
                UserContactForAPIDetail userContactForDetail = new UserContactForAPIDetail();
                userContactForDetail.ApiKey = user.Api.ApiKey.ToString();
                userContactForDetail.IsActive = user.Api.IsActive;
                userContactForDetail.UserName = user.UserName;
                lstUserContactForDetail.Add(userContactForDetail);
            }

            return lstUserContactForDetail;
        }
    }
}
