using System;
using System.Linq;
using OnDemandTools.Jobs.JobRegistry.Models.Model;
using OrionService;

namespace OnDemandTools.Jobs.Adapters.Queries
{
    public class GetOrionContentQuery : IGetOrionContentQuery
    {        
        public Content Get(string contentId)
        {
            try
            {
                var client = new InventoryClient();
                var request = new BasicVersionByCID
                {
                    ContentID = new string[] { contentId }
                };

                var response = client.GetBasicVersionInformationByCID(request);

                if (response == null || response.Length < 1)
                    return new Content();

                if (response.Length > 1)
                    throw new Exception(string.Format("To many versions were returned from Orion. ContentId: {0}", contentId));

                return new Content
                {
                    ContentId = contentId,
                    MaterialIds = response[0].Segments.Select(s => s.MaterialID).ToList()
                };
            }
            catch (Exception ex)
            {                
                throw;
            }
        }
    }

    public interface IGetOrionContentQuery
    {
        Content Get(string contentId);
    }
}