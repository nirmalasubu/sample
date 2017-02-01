using System;

namespace OnDemandTools.Common.DIResolver
{ 
    public interface IDependencyResolver : IDisposable
    {
        void RegisterImplmentation();
    }
}