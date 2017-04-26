using OnDemandTools.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Web.Models.User
{
    public class UserIdentity : IModel
    {
        public UserIdentity()
        {
            UserName = "guest";
            Destinations = new List<string>();
            Brands = new List<string>();
        }

        public String Id { get; set; }

        public string Name
        {
            get
            {
                return this.UserName;
            }
        }

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

    }
}
