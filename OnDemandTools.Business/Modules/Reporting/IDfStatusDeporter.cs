using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.Reporting
{
    public interface IDfStatusDeporter
    {
        void DeportDfStatus();
    }
}
