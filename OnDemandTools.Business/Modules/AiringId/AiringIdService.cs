using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnDemandTools.Business.Modules.AiringId.Model;
using OnDemandTools.DAL.Modules.AiringId.Commands;


namespace OnDemandTools.Business.Modules.AiringId
{
    public class AiringIdService : IAiringIdService
    {
        IAiringIdCreator creator;
        IIdDistributor distributor;
        AiringIdDeleteCommand deleteCommand;

        public AiringIdService(IAiringIdCreator creator, IIdDistributor distributor, AiringIdDeleteCommand deleteCommand)
        {
            this.creator = creator;
            this.distributor = distributor;
            this.deleteCommand = deleteCommand;
        }

        public CurrentAiringId Create(string prefix)
        {
            var newAiringId = creator.Create(prefix);

            return creator.Save(newAiringId);
        }

        public void Delete(string prefix)
        {
            deleteCommand.Delete(prefix);
        }

        public CurrentAiringId Distribute(string prefix)
        {
            return distributor.Distribute(prefix);
        }
    }
}
