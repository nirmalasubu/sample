namespace OnDemandTools.Common.DIResolver
{
    public static class DependencyResolver
    {
        private static IDependencyResolver resolver;

        public static IDependencyResolver RegisterResolver(IDependencyResolver resolver)
        {
            DependencyResolver.resolver = resolver;
            return resolver;
        }

        public static void LoadResolver()
        {
            resolver.RegisterImplmentation();
        }
    }
}