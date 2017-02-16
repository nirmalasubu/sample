using OnDemandTools.DAL.Modules.unitTestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.Airing
{
    public class AiringUnitTestService : IAiringUnitTestService
    {
        private readonly IAiringHelper _airingHelper;

        public AiringUnitTestService(IAiringHelper airingHelper)
        {
            _airingHelper = airingHelper;
        }
         public void UpdateAiringRelasedDateAndFlightEndDate(string airingId, DateTime releaseDate)
        {
            _airingHelper.UpdateAiringRelasedDateAndFlightEndDate(airingId, releaseDate);
        }

        public void RemoveMediaIdFromHistory(string mediaId)
        {
            _airingHelper.RemoveMediaIdFromHistory(mediaId);
        }
    }
}
