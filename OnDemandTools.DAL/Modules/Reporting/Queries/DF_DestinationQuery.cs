using Microsoft.Extensions.Configuration;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Reporting.Model;
using System.Collections.Generic;
using System.Linq;

namespace OnDemandTools.DAL.Modules.Reporting.Queries
{

    public class DF_DestinationQuery
    {
        IConfiguration configuration;

        public DF_DestinationQuery(IConfiguration configuration)
        {
            this.configuration = configuration;
            //  _client = new RestClient(String.Format("http://{0}", ConfigurationManager.AppSettings.Get("OnDemandToolsUrl")));
        }


        public IList<DF_Destination> Get()
        {
            ODTDatastore db = new ODTDatastore(configuration);
            return db.GetDatabase().GetCollection<DF_Destination>("Destination").FindAll().ToList();

        }

    }
}
