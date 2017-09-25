using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Adapters.ActiveDirectoryQuery
{
    public class AzureAdUser
    {
        public AzureAdUser()
        {
            businessPhones = new List<string>();
        }
        public string givenName { get; set; }

        public string surname { get; set; }

        public string userPrincipalName { get; set; }

        public string mobilePhone { get; set; }

        public List<string> businessPhones { get; set; }
    }
}
