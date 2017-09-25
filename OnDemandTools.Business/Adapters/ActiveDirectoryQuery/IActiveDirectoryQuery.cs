using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Adapters.ActiveDirectoryQuery
{
    public interface IActiveDirectoryQuery
    {
        AzureAdUser GetUserByEmailId(string email);
    }
}
