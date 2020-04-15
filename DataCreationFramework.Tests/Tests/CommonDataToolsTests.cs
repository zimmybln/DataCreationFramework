using DataCreationFramework.Tests.Types;
using DataCreationFramework.Data;
using NUnit.Framework;
using System.Linq;

namespace DataCreationFramework.Tests
{

    [TestFixture]
    public class CommonDataToolsTests
    {
        [Test]
        public void CreateTextCloseToEachOther()
        {
            var text1 = Common.CreateUniqueText("AB", 50, CreateTextOptions.Randomized);
            var text2 = Common.CreateUniqueText("AB", 50, CreateTextOptions.Randomized);

            Assert.AreNotEqual(text1, text2);
        }

        [Test]
        public void CreateTextWithLength()
        {
            var text = Common.CreateUniqueText("AB", 50, CreateTextOptions.Randomized);

            Assert.AreEqual(50, text.Length);
        }

        [Test]
        public void CreateItemsWithTestData()
        {
            var items = Common.CreateItems<Person>(200).ToList();

            Assert.AreEqual(200, items.Count());
            Assert.IsTrue(items.All(i => string.IsNullOrEmpty(i.FirstName)));
            Assert.IsTrue(items.All(i => string.IsNullOrEmpty(i.LastName)));
            Assert.IsTrue(items.All(i => i.Age == 0));
        }

        [Test]
        public void CreateItemsWithCustomStrategy()
        {
            var emailStrategy = new EmailStrategy();

            var strategy = new DataCreationStrategy<Person>();
            strategy.Add(p => p.EMail, emailStrategy);

            var items = Common.CreateItems(100, strategy);

            Assert.IsTrue(items.All(i => i.EMail.Equals("test@beispiel.com")));

        }



        public class EmailStrategy : DataCreationStrategy<string>
        {
            public override string GetValue()
            {
                return "test@beispiel.com";
            }
        }
    }

}
