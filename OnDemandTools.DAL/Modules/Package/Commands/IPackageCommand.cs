using System.Collections.Generic;

namespace OnDemandTools.DAL.Modules.Package.Commands
{
    public interface IPackageCommand
    {
        Model.Package Save(Model.Package pkg, string userName, bool updateHistorical);


        Model.Package Delete(Model.Package pkg, string userName, bool updateHistorical);

        bool DeletePackagebyAiringId(string airingId, string userName, bool updateHistorical);
    }

}