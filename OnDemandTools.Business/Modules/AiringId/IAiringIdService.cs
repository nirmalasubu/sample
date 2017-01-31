using BLModel = OnDemandTools.Business.Modules.AiringId.Model;

namespace OnDemandTools.Business.Modules.AiringId
{
    public interface IAiringIdService
    {
        BLModel.CurrentAiringId Distribute(string prefix);

        BLModel.CurrentAiringId Create(string prefix);

        void Delete(string prefix);
    }
}
