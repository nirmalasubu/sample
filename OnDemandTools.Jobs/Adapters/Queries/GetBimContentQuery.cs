using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OnDemandTools.Jobs.JobRegistry.Models.Model;
using FBDService;
using System.Threading.Tasks;

namespace OnDemandTools.Jobs.Adapters.Queries
{
    public class GetBimContentQuery : IGetBimContentQuery
    {

        #region IGetBimContentQuery Members

        public Content Get(string contentId)
        {            
            BIMToolRecord[] response = null;

            var client = new FBDWSSoapClient(new FBDWSSoapClient.EndpointConfiguration());

            Task.Run(async () =>
            {
                response = await client.GetBIMRecordByMaterialIdAsync(contentId + "%");
            }).Wait();


            if (!response.Any())
                return new Content();

            var content = new Content
            {
                ContentId = contentId,
                MaterialIds = response
                                      .Select(r => r.MaterialId).ToList()
            };

            return content;
        }

        #endregion
    }


    public interface IGetBimContentQuery
    {
        Content Get(string contentId);
    }


}