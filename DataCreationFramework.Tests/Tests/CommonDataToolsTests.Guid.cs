using DataCreationFramework.Tests.Types;
using DataCreationFramework.Data;
using NUnit.Framework;
using System;
using System.Linq;

namespace DataCreationFramework.Tests
{

    [TestFixture]
    public class CommonDataToolsTestsGuid
    {
        [Test]
        public void CreateItemWithGuidStrategyNotEmpty()
        {
            var personStrategy = new DataCreationStrategy<Person>();

            personStrategy.Add(p => p.Id, new GuidStrategy()).CreateNew();

            var items = Common.CreateItems(100, personStrategy);

            Assert.IsTrue(items.All(i => !i.Id.Equals(Guid.Empty)));
        }

        [Test]
        public void CreateItemWithGuidStrategyEmpty()
        {
            var personStrategy = new DataCreationStrategy<Person>();

            var items = Common.CreateItems(100, personStrategy);

            Assert.IsTrue(items.All(i => i.Id.Equals(Guid.Empty)));
        }

        [Test]
        public void CreateItemWithSpecificValue()
        {
            var personStrategy = new DataCreationStrategy<Person>();

            Guid value = Guid.NewGuid();

            personStrategy.Add(p => p.Id, new GuidStrategy()).Value(value);

            var items = Common.CreateItems(100, personStrategy);

            Assert.IsTrue(items.All(p => p.Id.Equals(value)));
        }

    }

}
