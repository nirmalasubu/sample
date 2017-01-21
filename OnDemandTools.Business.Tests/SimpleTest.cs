using OnDemandTools.Business.Tests.Helpers;
using Xunit;

namespace OnDemandTools.Business.Tests
{
    [TestCaseOrderer("OnDemandTools.Business.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.Business.Tests")]
    [Collection("Business Collection")]
    public class SimpleTest
    {
        [Fact, Order(1)]
        public void PassingTest()
        {
            Assert.Equal(4, Add(2, 2));
        }

       

        int Add(int x, int y)
        {
            return x + y;
        }
    }
}
