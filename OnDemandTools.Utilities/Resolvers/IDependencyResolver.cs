using System;

namespace OnDemandTools.Utilities.Resolvers
{ 
    public interface IDependencyResolver : IDisposable
    {
        void RegisterImplmentation();
    }
}