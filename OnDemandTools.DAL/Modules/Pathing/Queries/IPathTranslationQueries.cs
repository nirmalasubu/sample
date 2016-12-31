using OnDemandTools.DAL.Modules.Pathing.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTools.DAL.Modules.Pathing.Queries
{
    public interface IPathTranslationQueries
    {
        List<PathTranslation> GetBySourceBaseUrlAndBrand(String sourceBaseUrl, String sourceBrand);

        List<PathTranslation> GetBySourceBaseUrl(String sourceBaseUrl);

        List<PathTranslation> GetAll();
    }
}
