﻿using OnDemandTools.Business.Modules.AiringId.Model;

namespace OnDemandTools.Business.Modules.AiringId
{
    public interface IAiringIdCreator
    {
        CurrentAiringId Create(string prefix);
        CurrentAiringId Create(string prefix, int previousFiveDigitNumber);

        CurrentAiringId Save(CurrentAiringId currentAiringId);
    }
}
