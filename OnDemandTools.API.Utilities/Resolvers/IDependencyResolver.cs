using System;

namespace OnDemandTools.API.Utilities.Resolvers
{ 
    public interface IDependencyResolver : IDisposable
    {
        void RegisterImplmentation();
    }
}