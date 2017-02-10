using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTools.DAL.Modules.ModifiedTitles
{

    public interface ITitleIdsCommand
    {
        void Save(IList<int> titleIds);

        void Delete(IList<int> titleIds);
    }

    public interface ITitleIdsQuery
    {
        IEnumerable<int> Get(int limit);
    }
}
