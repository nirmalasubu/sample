using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnDemandTools.DAL.Modules.Status.Model;

namespace OnDemandTools.DAL.Modules.Status.Command
{
   public interface IStatusCommand
    {
        void Delete(string id);

        Model.Status Save(Model.Status status);
    }
}
