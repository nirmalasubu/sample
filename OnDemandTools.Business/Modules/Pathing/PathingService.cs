using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLModel = OnDemandTools.Business.Modules.Pathing.Model;
using DLModel = OnDemandTools.DAL.Modules.Pathing.Model;
using OnDemandTools.DAL.Modules.Pathing.Queries;
using OnDemandTools.Common.Model;
using OnDemandTools.Common.Configuration;
using OnDemandTools.DAL.Modules.Pathing.Command;

namespace OnDemandTools.Business.Modules.Pathing
{
    public class PathingService : IPathingService
    {
        IPathTranslationQueries translationQueryHelper;
        IPathTranslationCommand translationCommandHelper;

        IApplicationContext cntx;

        public PathingService(IPathTranslationQueries translationQueryHelper,
                IApplicationContext cntx, IPathTranslationCommand translationCommandHelper)
        {
            this.translationQueryHelper = translationQueryHelper;
            this.translationCommandHelper = translationCommandHelper;
            this.cntx = cntx;
        }


        /// <summary>
        /// Delete path translation that matches the given object id
        /// </summary>
        /// <param name="id">Path translation object id</param>   
        public void Delete(string id)
        {          
            translationCommandHelper.Delete(id);
        }

        public List<BLModel.PathTranslation> GetAll()
        {
            return
            translationQueryHelper.GetAll()
                .ToList<DLModel.PathTranslation>()
                .ToBusinessModel<List<DLModel.PathTranslation>, List<BLModel.PathTranslation>>();
            
        }

        public List<BLModel.PathTranslation> GetBySourceBaseUrl(string sourceBaseUrl)
        {
            return
            translationQueryHelper.GetBySourceBaseUrl(sourceBaseUrl)
                .ToList<DLModel.PathTranslation>()
                .ToBusinessModel<List<DLModel.PathTranslation>, List<BLModel.PathTranslation>>();
        }

        public List<BLModel.PathTranslation> GetBySourceBaseUrlAndBrand(string sourceBaseUrl, string sourceBrand)
        {
            return
            translationQueryHelper.GetBySourceBaseUrlAndBrand(sourceBaseUrl, sourceBrand)
                .ToList<DLModel.PathTranslation>()
                .ToBusinessModel<List<DLModel.PathTranslation>, List<BLModel.PathTranslation>>();
        }


        /// <summary>
        /// Save the given path translation model. If it already exist,
        /// update it; else, create a new one.
        /// </summary>
        /// <param name="model">Path translation model</param>
        public BLModel.PathTranslation Save(BLModel.PathTranslation model)
        {

            // If the model Id is empty then the assumption is that 
            // this is a new model. Hence provide create user and timestamp
            if (string.IsNullOrEmpty(model.Id))
            {
                model.CreatedBy = cntx.GetHttpIdentity().Name;
                model.CreatedDateTime = DateTime.UtcNow;
            }
            
            // For auditing
            model.ModifiedBy = cntx.GetHttpIdentity().Name;
            model.ModifiedDateTime = DateTime.UtcNow;

            return
            translationCommandHelper.Save(model.ToDataModel<BLModel.PathTranslation, DLModel.PathTranslation>())
                                    .ToBusinessModel<DLModel.PathTranslation, BLModel.PathTranslation>();

        }
    }
}
