using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLModel = OnDemandTools.Business.Modules.Pathing.Model;
using DLModel = OnDemandTools.DAL.Modules.Pathing.Model;
using OnDemandTools.DAL.Modules.Pathing.Queries;
using OnDemandTools.Common.Model;

namespace OnDemandTools.Business.Modules.Pathing
{
    public class PathingService : IPathingService
    {
        IPathTranslationQueries translationQueryHelper;
        public PathingService(IPathTranslationQueries translationQueryHelper)
        {
            this.translationQueryHelper = translationQueryHelper;
        }

        public List<BLModel.PathTranslation> GetAll()
        {
            return 
            translationQueryHelper.GetAll()
                .ToList<DLModel.PathTranslation>()
                .ToBusinessModel<List<DLModel.PathTranslation>,List<BLModel.PathTranslation>>();            
        }

        public List<BLModel.PathTranslation> GetBySourceBaseUrl(string sourceBaseUrl)
        {
            return 
            translationQueryHelper.GetBySourceBaseUrl(sourceBaseUrl)
                .ToList<DLModel.PathTranslation>()
                .ToBusinessModel<List<DLModel.PathTranslation>,List<BLModel.PathTranslation>>();            
        }

        public List<BLModel.PathTranslation> GetBySourceBaseUrlAndBrand(string sourceBaseUrl, string sourceBrand)
        {
            return 
            translationQueryHelper.GetBySourceBaseUrlAndBrand(sourceBaseUrl, sourceBrand)
                .ToList<DLModel.PathTranslation>()
                .ToBusinessModel<List<DLModel.PathTranslation>,List<BLModel.PathTranslation>>();
        }
    }
}
