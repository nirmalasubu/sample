using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OnDemandTools.API.Tests.Helpers
{
    [CollectionDefinition("API Collection")]
    public class APITestCollection:ICollectionFixture<APITestFixture>
    {
    }
}
