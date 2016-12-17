using System;
using System.Collections.Generic;
using OnDemandTools.Common.Model;
using System.Security.Principal;
using System.Security.Claims;

namespace OnDemandTools.Business.Modules.User.Model
{
    public class UserIdentity : GenericIdentity, IModel
    {
        public UserIdentity() : base("guest")
        {
            UserName = "guest";
            Claims = new List<string>();
            Destinations = new List<string>();
            Brands = new List<string>();
        }

        public String Id { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public Guid ApiKey { get; set; }
        public IEnumerable<string> Claims { get; set; }
        public IEnumerable<string> Destinations { get; set; }
        public IEnumerable<string> Brands { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }

        public string AuthenticationType
        {
            get
            {
                return "stateless";
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return UserName == "guest" ? false : true;
            }
        }

        public string Name
        {
            get
            {
                return UserName;
            }
        }
    }

}