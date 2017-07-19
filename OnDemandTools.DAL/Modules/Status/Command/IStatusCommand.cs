using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.DAL.Modules.Status.Command
{
   public interface IStatusCommand
    {
        void Delete(string id);
    }
}
