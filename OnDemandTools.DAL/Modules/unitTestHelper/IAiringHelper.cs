using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.DAL.Modules.unitTestHelper
{
     public interface IAiringHelper
    {
        void UpdateAiringRelasedDateAndFlightEndDate(string airingId, DateTime releasedon);

        void RemoveMediaIdFromHistory(string mediaId);
    }
}
