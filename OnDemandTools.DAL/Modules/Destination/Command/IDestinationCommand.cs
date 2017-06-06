using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnDemandTools.DAL.Modules.Destination.Command
{
    public interface IDestinationCommand
    {
        void Delete(string id);
    }
}
