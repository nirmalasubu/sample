using FBDService;
using OnDemandTools.Business.Modules.AiringPublisher.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Jobs.Adapters.Queries
{
    public class GetBimContentQuery : IGetBimContentQuery
    {

        #region IGetBimContentQuery Members

        public Content Get(string contentId)
        {

            GetBIMRecordByMaterialIdResponse result = null;

            var client = new FBDWSSoapClient(new FBDWSSoapClient.EndpointConfiguration());

            Task.Run(async () =>
            {
                result = await client.GetBIMRecordByMaterialIdAsync(contentId + "%");
            }).Wait();

            var response = result.Body.GetBIMRecordByMaterialIdResult.ToList();

            if (!response.Any())
                return new Content();

            var content = new Content
            {
                ContentId = contentId,
                MaterialIds = response.Select(r => r.MaterialId).ToList()
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