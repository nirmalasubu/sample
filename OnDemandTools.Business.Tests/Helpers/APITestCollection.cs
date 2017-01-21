using Xunit;

namespace OnDemandTools.Business.Tests.Helpers
{
    [CollectionDefinition("Business Collection")]
    public class APITestCollection:ICollectionFixture<BusinessTestFixture>
    {
    }
}
