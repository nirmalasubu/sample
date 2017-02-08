using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OnDemandTools.Jobs.JobRegistry.Models.Model;
using FBDService;

namespace OnDemandTools.Jobs.Adapters.Queries
{
    public class GetBimContentQuery : IGetBimContentQuery
    {

        #region IGetBimContentQuery Members

        public Content Get(string contentId)
        {
            List<BIMToolRecord> response;

            try
            {
                var client = new FBDWSSoapClient(new FBDWSSoapClient.EndpointConfiguration());

                response = client.GetBIMRecordByMaterialId(contentId + "%").ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (!response.Any())
                return new Content();

            var content = new Content
            {
                ContentId = contentId,
                MaterialIds = response
                                      .Select(r => r.MaterialId).ToList()
                                      .ConvertAll(i => i.ToString(CultureInfo.InvariantCulture))
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