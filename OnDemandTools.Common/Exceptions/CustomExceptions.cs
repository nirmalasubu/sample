using System;


namespace OnDemandTools.Common.Exceptions
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
