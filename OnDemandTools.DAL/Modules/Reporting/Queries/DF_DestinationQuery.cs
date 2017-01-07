using Microsoft.Extensions.Configuration;
using OnDemandTools.Common.Configuration;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Reporting.Model;
using System.Collections.Generic;
using System.Linq;

namespace OnDemandTools.DAL.Modules.Reporting.Queries
{

    public class DF_DestinationQuery
    {
        AppSettings appSettings;

        public DF_DestinationQuery(AppSettings appSettings)
        {
            this.appSettings = appSettings;
        }


        public IList<DF_Destination> Get()
        {
            ODTDatastore db = new ODTDatastore(appSettings);
            return db.GetDatabase().GetCollection<DF_Destination>("Destination").FindAll().ToList();

        }

    }
}
