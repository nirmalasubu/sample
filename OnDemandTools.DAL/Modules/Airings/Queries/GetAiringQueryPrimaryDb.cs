using OnDemandTools.DAL.Database;

namespace OnDemandTools.DAL.Modules.Airings.Queries
{
    public class GetAiringQueryPrimaryDb : GetAiringQuery, IGetAiringQueryPrimaryDb
    {
        public GetAiringQueryPrimaryDb(ODTPrimaryDatastore connection)
            : base(connection)
        {

        }
    }
}