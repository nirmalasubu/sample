using Nancy;
using Nancy.Security;
using OnDemandTools.Business.Modules.AiringId;
using OnDemandTools.Business.Modules.AiringId.Model;
using OnDemandTools.Common.Model;
using OnDemandTools.API.v1.Models;
using OnDemandTools.API.Helpers;
using System.Net.Http;

namespace OnDemandTools.API.v1.Routes
{
    public class AiringIdRoutes : BaseModule
    {
        Serilog.ILogger _logger;
        public AiringIdRoutes(IAiringIdCreator creator, IIdDistributor distributor)
           : base("v1")
        {         
            this.RequiresAuthentication();
            

            Get("/airingId/generate/{prefix}", _ =>
            {
                var k = this.Logger;
                this.RequiresClaims(c => c.Type == HttpMethod.Get.Verb());

                return distributor.Distribute((string)_.prefix)
                        .ToViewModel<CurrentAiringId, CurrentAiringIdViewModel>();
            });

            Post("/airingId/{prefix}", _ =>
           {
               this.RequiresClaims(c => c.Type == HttpMethod.Post.Verb());
               CurrentAiringId airingId = creator.Create((string)_.prefix);               
               return creator.Save(airingId, Context.User()).ToViewModel<CurrentAiringId, CurrentAiringIdViewModel>();              
           });
        }
    }
}
