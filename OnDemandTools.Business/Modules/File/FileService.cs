using System;
using System.Collections.Generic;
using OnDemandTools.Business.Modules.User.Model;
using OnDemandTools.DAL.Modules.File.Queries;
using System.Linq;
using BLModel = OnDemandTools.Business.Modules.File.Model;
using DLModel = OnDemandTools.DAL.Modules.File.Model;
using OnDemandTools.Common.Model;

namespace OnDemandTools.Business.Modules.File
{
    public class FileService : IFileService
    {
        IFileQuery fileQuery;

        public FileService(IFileQuery fileQuery)
        {
            this.fileQuery = fileQuery;
        }

        public IList<BLModel.File> GetBy(List<string> contentIds, List<int> titleIds, string airingId, string mediaId)
        {
            throw new NotImplementedException();
        }

        public List<BLModel.File> GetByAiringId(string airingId)
        {
            return 
            fileQuery.Get(airingId)
                .ToList<DLModel.File>()
                .ToBusinessModel<List<DLModel.File>, List<BLModel.File>>();
           
        }

        public List<BLModel.File> GetByTitleId(int titleId)
        {
            return
            fileQuery.Get(titleId)
                .ToList<DLModel.File>()
                .ToBusinessModel<List<DLModel.File>, List<BLModel.File>>();
        }

        public void Save(List<BLModel.File> files, UserIdentity user)
        {
            throw new NotImplementedException();
        }
    }
}
