using DataCreationFramework.Tests.Types;
using DataCreationFramework.Data;
using DataCreationFramework.Extensions;
using NUnit.Framework;
using System.Linq;
using System.Reflection;

namespace DataCreationFramework.Tests
{
    [TestFixture]
    public class ExtensionsTests
    {
        [Test]
        public void UseAscendingArray()
        {
            var array = new int[] { 2, 4, 6, 7, 10 };

            Assert.IsTrue(array.IsAscending());
            Assert.IsFalse(array.IsDescending());
        }

        [Test]
        public void UseDescrendingArray()
        {
            var array = new int[] { 20, 19, 15, 8, 3, 2 };

            Assert.IsFalse(array.IsAscending());
            Assert.IsTrue(array.IsDescending());

        }

        [Test]
        public void UseCustomStrategyWithInitialize()
        {
            var strategy = new DataCreationStrategy<Person>();

            strategy.Add(p => p.FirstName, new CustomStrategyWithInitialize());

            var items = Common.CreateItems(200, strategy);

            Assert.IsTrue(items.All(p => p.FirstName.Equals(nameof(Person.FirstName))));



        }
    }

    public class CustomStrategyWithInitialize : DataCreationStrategy<string>
    {
        private string name = null;

        public override void Initialize(PropertyInfo property)
        {
            name = property.Name;
        }

        public override string GetValue()
        {
            return name;
        }
    }
}
