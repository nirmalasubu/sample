using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.Airing
{
     public interface IAiringUnitTestService
    {

         void UpdateAiringRelasedDate(string airingId, DateTime releaseDate);
    }
}
