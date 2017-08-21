using System;

namespace OnDemandTools.Web.Models.UserPermissions
{
    public class UserPermission
    {
        public UserPermission()
        {
            UserName = "";
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Notes { get; set; }

        public UserType UserType { get; set; }
        public Api Api { get; set; }
        public Portal Portal { get; set; }

        public DateTime ActiveDateTime { get; set; }
       
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }
    }
}
