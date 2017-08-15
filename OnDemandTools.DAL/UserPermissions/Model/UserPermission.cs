using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.DAL.UserPermissions.Model
{
    public class UserPermission
    {
        public UserPermission()
        {
            UserName = "";
        }

        public ObjectId Id { get; set; }
        public string UserName { get; set; }
        public string FirtName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Notes { get; set; }

        public UserType UserType { get; set; }

        public Api Api { get; set; }
        public Portal Portal { get; set; }

        public DateTime ActivateDateTime { get; set; }
       
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }
    }
}
