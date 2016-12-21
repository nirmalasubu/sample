using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.CustomExceptions
{
    public class AiringNotFoundException : Exception
    {
        public AiringNotFoundException(string message)
            : base(message)
        {
        }
    }


    public class SecurityAccessDeniedException : Exception
    {
        public SecurityAccessDeniedException(string message)
            : base(message)
        {
        }
    }
}
