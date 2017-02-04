using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OnDemandTools.Jobs.Tests.Helpers
{
    [CollectionDefinition("Job Collection")]
    public class JobTestCollection:ICollectionFixture<JobTestFixture>
    {
    }
}
