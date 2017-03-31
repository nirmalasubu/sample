using OnDemandTools.Business.Modules.Airing;
using System.Linq;

namespace OnDemandTools.Jobs.JobRegistry.Airings
{
    public class PurgeUnitTestAirings
    {
        IAiringService _service;

        public PurgeUnitTestAirings(IAiringService service)
        {
            _service = service;
        }
        public void Execute(string[] airingids)
        {
            _service.PurgeUnitTestAirings(airingids.ToList());
        }
    }
}
