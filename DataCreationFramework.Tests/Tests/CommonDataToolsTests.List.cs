using DataCreationFramework.Tests.Types;
using DataCreationFramework.Data;
using NUnit.Framework;
using System.Linq;

namespace DataCreationFramework.Tests
{
    [TestFixture]
    public class CommonDataToolsTestsList
    {
        [Test]
        public void TestListOfNumbers()
        {
            var strategy = new DataCreationStrategy<Person>();

            var intstrategy = new IntegerStrategy();
            intstrategy.Value(42);

            var liststrategy = new ListStrategy<int>(intstrategy);

            strategy.Add(p => p.Changes, liststrategy).Amount(249);

            var items = Common.CreateItems(10, strategy).ToList();

            Assert.IsTrue(items.All(p => p.Changes.Count == 249));
            Assert.IsTrue(items.All(p => p.Changes.All(c => c == 42)));
        }

        [Test]
        public void TestListOfNumbersWithVariableItems()
        {
            var strategy = new DataCreationStrategy<Person>();

            var intstrategy = new IntegerStrategy();
            intstrategy.Value(42);

            var liststrategy = new ListStrategy<int>(intstrategy);

            strategy.Add(p => p.Changes, liststrategy).Min(10).Max(20);

            var items = Common.CreateItems(100, strategy).ToList();

            Assert.IsTrue(items.Select(p => p.Changes.Count).Distinct().ToList().Count == 10);
            Assert.IsTrue(items.All(p => p.Changes.All(c => c == 42)));

        }

        [Test]
        public void TestListOfItemsWithReset()
        {
            var subtypeStrategy = new DataCreationStrategy<SubType>();

            subtypeStrategy.Add(s => s.Value, new IntegerStrategy()).Increment(100);

            var primaryTypeStrategy = new DataCreationStrategy<PrimaryType>();

            primaryTypeStrategy.Add(p => p.SubTypes, new ListStrategy<SubType>(subtypeStrategy)).Amount(10);

            var items = Common.CreateItems(100, primaryTypeStrategy).ToList();

            Assert.IsTrue(items.All(p => p.SubTypes.First().Value == 100));
        }
    }
}
