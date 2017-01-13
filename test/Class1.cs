using Xunit;

namespace test
{
    // see example explanation on xUnit.net website:
    // https://xunit.github.io/docs/getting-started-dotnet-core.html
    public class Class1
    {
       [Fact]
        public void Test1() 
        {
            Assert.True(true);
        }

         [Fact]
        public void Test2() 
        {
            Assert.True(false);
        }
    }
}
