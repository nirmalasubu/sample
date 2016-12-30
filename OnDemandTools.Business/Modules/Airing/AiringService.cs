using System;
using System.Collections.Generic;
using OnDemandTools.DAL.Modules.Airings;
using BLModel = OnDemandTools.Business.Modules.Airing.Model;
using DLModel = OnDemandTools.DAL.Modules.Airings.Model;
using System.Linq;
using OnDemandTools.Common.Model;

namespace OnDemandTools.Business.Modules.Airing
{
    public class AiringService : IAiringService
    {
        IGetAiringQuery airingQueryHelper;

        public AiringService(IGetAiringQuery airingQueryHelper)
        {
            this.airingQueryHelper = airingQueryHelper;
        }

        public List<BLModel.Airing> GetByMediaId(string mediaId)
        {
            return
            (airingQueryHelper.GetByMediaId(mediaId).ToList<DLModel.Airing>()
                .ToBusinessModel<List<DLModel.Airing>, List<BLModel.Airing>>());

        }

        public List<Model.Airing> GetNonExpiredBy(int titleId, DateTime cutOffDateTime, bool isSeries = false)
        {
            return
            (airingQueryHelper.GetNonExpiredBy(titleId, cutOffDateTime, isSeries).ToList<DLModel.Airing>()
                .ToBusinessModel<List<DLModel.Airing>, List<BLModel.Airing>>());
        }
    }
}
