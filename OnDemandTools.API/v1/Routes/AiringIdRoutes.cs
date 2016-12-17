using Nancy;
using Nancy.Security;
using OnDemandTools.Business.Modules.AiringId;
using OnDemandTools.Business.Modules.AiringId.Model;
using OnDemandTools.Common.Model;
using OnDemandTools.API.v1.Models;

namespace OnDemandTools.API.v1.Routes
{
    public class AiringIdRoutes : NancyModule
    {
        public AiringIdRoutes(IAiringIdCreator creator)
           : base("v1")
        {
            //TODO activate this later
            this.RequiresAuthentication();

            //Get("/airingId/generate/{prefix}", _ =>
            //{
            //    this.RequiresClaims(c => c.Type == "get");

            //    return distributor.Distribute((string)_.prefix)
            //            .ToViewModel<CurrentAiringId, CurrentAiringIdViewModel>();
            //});

            Post("/airingId/{prefix}", _ =>
           {
               this.RequiresClaims(c => c.Type == "get");
               //CurrentAiringId airingId = creator.Create((string)_.prefix);
               //creator.Save(airingId);
               //return airingId.ToViewModel<CurrentAiringId, CurrentAiringIdViewModel>();
               return null;
           });
        }
    }
}
