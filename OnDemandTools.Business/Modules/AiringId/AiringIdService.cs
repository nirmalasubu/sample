using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnDemandTools.Business.Modules.AiringId.Model;
using OnDemandTools.DAL.Modules.AiringId.Commands;
using OnDemandTools.DAL.Modules.AiringId;
using DLModel = OnDemandTools.DAL.Modules.AiringId.Model;
using OnDemandTools.Common.Model;


namespace OnDemandTools.Business.Modules.AiringId
{
    public class AiringIdService : IAiringIdService
    {
        IAiringIdCreator creator;
        IIdDistributor distributor;
        IAiringIdDeleteCommand deleteCommand;
        IGetAiringIdsQuery query;

        public AiringIdService(IAiringIdCreator creator, IIdDistributor distributor, IAiringIdDeleteCommand deleteCommand, IGetAiringIdsQuery query)
        {
            this.creator = creator;
            this.distributor = distributor;
            this.deleteCommand = deleteCommand;
            this.query = query;
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

        public void DeleteById(string id)
        {
            deleteCommand.Delete(id);
        }

        public CurrentAiringId Distribute(string prefix)
        {
            return distributor.Distribute(prefix);
        }

        public List<CurrentAiringId> GetAllCurrentAiringIds()
        {
            return (query.Get().ToList()
                .ToBusinessModel<List<DLModel.CurrentAiringId>, List<CurrentAiringId>>());
        }
    }
}
