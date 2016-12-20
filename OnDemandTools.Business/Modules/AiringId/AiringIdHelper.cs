

using OnDemandTools.Business.Modules.AiringId.Model;

namespace OnDemandTools.Business.Modules.AiringId
{
    public interface IIdDistributor
    {
        CurrentAiringId Distribute(string prefix);
    }
}
