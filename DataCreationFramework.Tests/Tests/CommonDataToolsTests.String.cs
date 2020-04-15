using DataCreationFramework.Tests.Types;
using DataCreationFramework.Data;
using DataCreationFramework.Extensions;
using NUnit.Framework;
using System;
using System.Linq;

namespace DataCreationFramework.Tests
{

    [TestFixture]
    public class CommonDataToolsTestsString
    {
        [Test]
        public void CreateItemsWithStringStrategyMaxLength()
        {
            var strategy = new DataCreationStrategy<Person>();

            strategy.Add(p => p.FirstName, new StringStrategy()).Length(153);

            var items = Common.CreateItems(10, strategy).ToList();

            Assert.IsTrue(items.Any(i => i.FirstName.Length == 153));
        }

        [Test]
        public void CreateItemsWithStringStrategyOneOfTheseValues()
        {
            var strategy = new DataCreationStrategy<Person>();

            strategy.Add(p => p.FirstName, new StringStrategy()).OneOfTheseValues("John", "Paul", "George", "Ringo");

            var items = Common.CreateItems(100, strategy).ToList();

            var template = new[] { "John", "Paul", "George", "Ringo" };

            Assert.IsTrue(items.All(i => template.Contains(i.FirstName)));

            template.ToList().ForEach(name => Assert.IsTrue(items.Any(i => i.FirstName.Equals(name))));
        }


        [Test]
        public void CreateItemsWithStringStrategyOneOfTheseValuesInTwoFields()
        {

            var firstNameTemplates = new[] { "John", "Paul", "George", "Ringo" };
            var lastNameTemplates = new[] { "Lennon", "McCartney", "Harrison", "Starr" };

            var strategy = new DataCreationStrategy<Person>();

            strategy.Add(p => p.FirstName, new StringStrategy()).OneOfTheseValues(firstNameTemplates);
            strategy.Add(p => p.LastName, new StringStrategy()).OneOfTheseValues(lastNameTemplates);

            var items = Common.CreateItems(100, strategy).ToList();


            Assert.IsTrue(items.All(i => firstNameTemplates.Contains(i.FirstName)));
            Assert.IsTrue(items.All(i => i.FirstName.IsOneOfTheseValues(firstNameTemplates)));

            Assert.IsTrue(items.All(i => lastNameTemplates.Contains(i.LastName)));
            Assert.IsTrue(items.All(i => i.LastName.IsOneOfTheseValues(lastNameTemplates)));
        }

        [Test]
        public void CreateItemsWithStringStrategyOneOfTheseValuesIgnoringMaxLength()
        {
            var strategy = new DataCreationStrategy<Person>();

            strategy.Add(p => p.FirstName, new StringStrategy()).Length(2).OneOfTheseValues("John", "Paul", "George", "Ringo");

            var items = Common.CreateItems(10, strategy).ToList();

            Assert.IsFalse(items.Any(i => i.FirstName.Length <= 2));
        }

        [Test]
        public void CreateItemsWithStringStrategyPrefix()
        {
            var strategy = new DataCreationStrategy<Person>();

            strategy.Add(p => p.FirstName, new StringStrategy()).Prefix("ABCD").Length(20);

            var items = Common.CreateItems(10, strategy).ToList();

            Assert.IsTrue(items.All(i => i.FirstName.StartsWith("ABCD")));
            Assert.IsTrue(items.All(i => i.FirstName.Length == 20));
        }

        [Test]
        public void CreateItemsWithStringStrategySuffix()
        {
            var strategy = new DataCreationStrategy<Person>();

            strategy.Add(p => p.FirstName, new StringStrategy()).Suffix("WXYZ").Length(20);
            strategy.Add(p => p.Age, new IntegerStrategy()).Min(10).Max(90);

            var items = Common.CreateItems(10, strategy).ToList();

            Assert.IsTrue(items.All(i => i.FirstName.EndsWith("WXYZ")));
            Assert.IsTrue(items.All(i => i.FirstName.Length == 20));
            Assert.IsTrue(items.All(i => i.Age >= 10 && i.Age <= 90));
        }


        [Test]
        public void CreateItemsWithCombinedStringStrategyPrefixMaxLength()
        {
            var strategy = new DataCreationStrategy<Person>();

            strategy.Add(p => p.FirstName, new StringStrategy()).Prefix("ABCD");
            strategy.Add(p => p.LastName, new StringStrategy()).Length(20);

            var items = Common.CreateItems(10, strategy).ToList();

            Assert.IsTrue(items.All(i => i.FirstName.StartsWith("ABCD")));
            Assert.IsTrue(items.All(i => i.FirstName.Length != 20));
            Assert.IsTrue(items.All(i => i.LastName.Length == 20));
        }

        [Test]
        public void CreateItemsWithValue()
        {
            var personStrategy = new DataCreationStrategy<Person>();

            personStrategy.Add(p => p.FirstName, new StringStrategy()).Value("Hallo");

            var items = Common.CreateItems(200, personStrategy);

            Assert.IsTrue(items.All(p => p.FirstName.Equals("Hallo")));
        }

        [Test]
        public void CreateItemsWithComposedStrategy()
        {
            var cities = new[] { "Berlin", "Köln", "Dresden" };

            // define the strategy for Person
            var personStrategy = new DataCreationStrategy<Person>();

            personStrategy.Add(p => p.Age, new IntegerStrategy()).Min(10).Max(100);
            personStrategy.Add(p => p.FirstName, new StringStrategy()).Length(35);
            personStrategy.Add(p => p.LastName, new StringStrategy()).Length(20).Prefix("CC_");

            // define the strategy for Address
            var addressStrategy = new DataCreationStrategy<Address>();

            addressStrategy.Add(a => a.City, new StringStrategy()).OneOfTheseValues(cities);

            // add the strategy for adress to the strategy for person
            personStrategy.Add<Address, DataCreationStrategy<Address>>(p => p.HomeAddress, addressStrategy);

            var items = Common.CreateItems(100, personStrategy);

            Assert.IsTrue(items.All(i => i.HomeAddress != null));
            Assert.IsTrue(items.All(i => i.FirstName.Length == 35));
            Assert.IsTrue(items.All(i => i.LastName.Length == 20));
            Assert.IsTrue(items.All(i => i.LastName.StartsWith("CC_")));
            Assert.IsTrue(items.All(i => cities.Contains(i.HomeAddress.City)));

        }


    }



}
