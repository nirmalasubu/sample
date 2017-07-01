using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnDemandTools.DAL.Modules.Destination.Command
{
    public interface IDestinationCommand
    {
        DAL.Modules.Destination.Model.Destination Save(DAL.Modules.Destination.Model.Destination destination);

        void UpdateDestinationCategory(string name, DAL.Modules.Destination.Model.Category category);

        void Delete(string id);

        void DeleteCategoryByName(string name);
    }
}
