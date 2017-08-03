using System.Collections.Generic;
using BLModel = OnDemandTools.Business.Modules.AiringId.Model;

namespace OnDemandTools.Business.Modules.AiringId
{
    public interface IAiringIdService
    {
        BLModel.CurrentAiringId Distribute(string prefix);

        BLModel.CurrentAiringId Create(string prefix);

        BLModel.CurrentAiringId Save(BLModel.CurrentAiringId airingId);

        void Delete(string prefix);

        void DeleteById(string id);

        List<BLModel.CurrentAiringId> GetAllCurrentAiringIds();
    }
}
