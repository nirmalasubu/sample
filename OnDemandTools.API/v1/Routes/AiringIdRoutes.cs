using Nancy;
using Nancy.Security;
using OnDemandTools.Business.Modules.AiringId.Model;
using OnDemandTools.Common.Model;
using OnDemandTools.API.v1.Models;
using OnDemandTools.API.Helpers;
using System.Net.Http;
using OnDemandTools.Business.Modules.AiringId;

namespace OnDemandTools.API.v1.Routes
{
    public class AiringIdRoutes : NancyModule
    {
        public AiringIdRoutes(IAiringIdService service)
           : base("v1")
        {
            this.RequiresAuthentication();

            Get("/airingId/generate/{prefix}", _ =>
            {
                this.RequiresClaims(c => c.Type == HttpMethod.Get.Verb());

                return service.Distribute((string)_.prefix)
                        .ToViewModel<CurrentAiringId, CurrentAiringIdViewModel>();
            });

            Post("/airingId/{prefix}", _ =>
           {
               this.RequiresClaims(c => c.Type == HttpMethod.Post.Verb());
               CurrentAiringId airingId = service.Create((string)_.prefix);
               return airingId.ToViewModel<CurrentAiringId, CurrentAiringIdViewModel>();
           });

            Delete("/airingId/{prefix}", _ =>
            {
                this.RequiresClaims(c => c.Type == HttpMethod.Delete.Verb());
                service.Delete((string)_.prefix);
                return new { Message = "deleted successfully" };
            });


        }
    }
}
