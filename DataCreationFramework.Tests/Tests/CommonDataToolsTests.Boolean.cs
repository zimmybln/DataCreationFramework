using DataCreationFramework.Tests.Types;
using DataCreationFramework.Data;
using NUnit.Framework;
using System.Linq;

namespace DataCreationFramework.Tests
{
    [TestFixture]
    public class CommonDataToolsTestsBoolean
    {
        [Test]
        public void TestStaticValue()
        {
            var strategy = new DataCreationStrategy<Person>();

            strategy.Add(p => p.IsMember, new BooleanStrategy()).Value(true);

            var items = Common.CreateItems(100, strategy);

            Assert.IsTrue(items.All(p => p.IsMember == true));
        }

        [Test]
        public void TestToggleValue()
        {
            var strategy = new DataCreationStrategy<Person>();

            strategy.Add(p => p.IsMember, new BooleanStrategy()).Toggle();

            var items = Common.CreateItems(100, strategy).ToList();

            Assert.IsTrue(items.Count(p => p.IsMember == true) == items.Count(p => p.IsMember == false));

        }

        [Test]
        public void TestRandomValue()
        {
            var strategy = new DataCreationStrategy<Person>();

            strategy.Add(p => p.IsMember, new BooleanStrategy()).Random();

            var items = Common.CreateItems(100, strategy);

            Assert.IsFalse(items.All(p => p.IsMember == true));
            Assert.IsFalse(items.All(p => p.IsMember == false));
        }
    }
}
