using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OnDemandTools.Job.Tests.Helpers
{
    [CollectionDefinition("API Collection")]
    public class JobTestCollection:ICollectionFixture<JobTestFixture>
    {
    }
}
