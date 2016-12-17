namespace OnDemandTools.API
{
    using Nancy;

    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get("/", _ => "Hello World");


            Get("/home", args =>
            {
                throw new System.Exception("error");
            });
        }
    }
}
