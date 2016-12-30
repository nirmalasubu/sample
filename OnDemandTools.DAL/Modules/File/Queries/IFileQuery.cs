using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueueModel = OnDemandTools.DAL.Modules.Queue.Model;
using FileModel = OnDemandTools.DAL.Modules.File.Model;

namespace OnDemandTools.DAL.Modules.File.Queries
{
    public interface IFileQuery
    {
        List<FileModel.File> Get(int titleId);
        List<FileModel.File> Get(string airingId);

        IList<FileModel.File> GetBy(List<string> contentIds, List<int> titleIds, string airingId, string mediaId);
    }
}
