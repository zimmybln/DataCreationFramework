using NUnit.Framework;

namespace DataCreationFramework.Tests
{
    [TestFixture]
    public class ReplaceMethodTest
    {
        [Test]
        public void Test()
        {
            var dt = Hook(new TestClass());

            

            

        }


        public T Hook<T>(T item) where T : class
        {
            return item;
        }

    }

    public class TestClass
    {
        public int DoSomething() => 1;
    }

}
