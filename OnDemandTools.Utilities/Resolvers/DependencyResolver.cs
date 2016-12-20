namespace OnDemandTools.Utilities.Resolvers
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