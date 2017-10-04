using System;
using System.Collections.Generic;
using OnDemandTools.Common.Model;
using System.Security.Principal;
using System.Security.Claims;

namespace OnDemandTools.Common.Configuration
{

    public class UserIdentity : GenericIdentity
    {
        public UserIdentity() : base("", "stateless")
        {
            UserName = "";
            Destinations = new List<string>();
            Brands = new List<string>();
        }

        public String Id { get; set; }

        public override string Name
        {
            get
            {
                return this.UserName;
            }
        }

        public override IEnumerable<Claim> Claims
        {
            get
            {
                return base.Claims;
            }
        }

        public override bool IsAuthenticated
        {
            get
            {
                return (this.Name != "" ? true : false);
            }
        }

        public bool DestinationPermitAll { get; set; }
        public bool BrandPermitAll { get; set; }
        public string UserName { get; set; }
        public UserType UserType { get; set; }
        public Guid ApiKey { get; set; }
        public IEnumerable<string> Destinations { get; set; }
        public IEnumerable<string> Brands { get; set; }
    }

}
