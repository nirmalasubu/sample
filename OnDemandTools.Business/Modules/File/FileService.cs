using System;
using System.Collections.Generic;
using OnDemandTools.DAL.Modules.File.Queries;
using System.Linq;
using BLModel = OnDemandTools.Business.Modules.File.Model;
using DLModel = OnDemandTools.DAL.Modules.File.Model;
using OnDemandTools.Common.Model;
using OnDemandTools.DAL.Modules.File.Command;
using OnDemandTools.Common.Configuration;

namespace OnDemandTools.Business.Modules.File
{
    public class FileService : IFileService
    {
        IFileQuery fileQuery;
        IFileUpsertCommand fileCommand;
        IApplicationContext cntx;

        public FileService(IFileQuery fileQuery, 
            IFileUpsertCommand fileCommand,
            IApplicationContext cntx)
        {
            this.fileQuery = fileQuery;
            this.fileCommand = fileCommand;
            this.cntx = cntx;
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

        public void Save(List<BLModel.File> files)
        {           
            fileCommand.Save(files.ToDataModel<List<BLModel.File>, List<DLModel.File>>(), cntx.GetUser().UserName);
        }

        public void PersistNonVideoFiles(List<BLModel.File> files)
        {
            throw new NotImplementedException();
        }

        public void PersistVideoFile(BLModel.File file)
        {
            fileCommand.PersistVideoFile(file.ToDataModel<BLModel.File, DLModel.File>(), cntx.GetUser().UserName);           
        }

        public void PersistVideoFiles(List<BLModel.File> files)
        {
            throw new NotImplementedException();
        }
    }
}
