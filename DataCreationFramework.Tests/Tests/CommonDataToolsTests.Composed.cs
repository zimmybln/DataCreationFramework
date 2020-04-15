using DataCreationFramework.Tests.Types;
using DataCreationFramework.Data;
using DataCreationFramework.Extensions;
using DataCreationFramework.TestStructure;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DataCreationFramework.Tests
{

    [TestFixture]
    public class CommonDataToolsTestsComposed : MutableTest<TestStrategy, TestResult>
    {
        [Test]
        public void TestOneOfTheseValues()
        {
            var citiesForHome = new[] { "Berlin", "Köln", "Dresden" };
            var citiesForBusiness = new[] { "München", "Hamburg", "Rostock" };

            When(CreateItemsWithComposedStrategy)
            .RunsWith((strategy) =>
            {
                strategy.BusinessAddress.Add(a => a.City, new StringStrategy()).OneOfTheseValues(citiesForBusiness);
                strategy.HomeAddress.Add(a => a.City, new StringStrategy()).OneOfTheseValues(citiesForHome);
            })
            .Then((result) =>
            {
                Assert.IsTrue(result.Persons.All(p => p.HomeAddress != null));
                Assert.IsTrue(result.Persons.All(p => p.BusinessAddress != null));
                Assert.IsTrue(result.Persons.All(p => citiesForHome.Contains(p.HomeAddress.City)));
                Assert.IsTrue(result.Persons.All(p => citiesForBusiness.Contains(p.BusinessAddress.City)));
                Assert.IsTrue(result.Persons.All(p => p.HomeAddress.City.IsOneOfTheseValues(citiesForHome)));
                Assert.IsTrue(result.Persons.All(p => p.BusinessAddress.City.IsOneOfTheseValues(citiesForBusiness)));
            });
        }

        [Test]
        public void TestForStreetNumber()
        {
            When(CreateItemsWithComposedStrategy)
            .RunsWith((strategy) =>
            {
                strategy.BusinessAddress.Add(a => a.Number, new IntegerStrategy()).Increment(10);
                strategy.HomeAddress.Add(a => a.Number, new IntegerStrategy()).Decrement(200);
            })
            .Then((result) =>
            {
                Assert.IsTrue(result.Persons.All(p => p.HomeAddress != null));
                Assert.IsTrue(result.Persons.All(p => p.BusinessAddress != null));
                Assert.IsTrue(result.Persons.All(p => p.BusinessAddress.Number > 10));
                Assert.IsTrue(result.Persons.All(p => p.HomeAddress.Number < 200));
            });
        }

        public TestResult CreateItemsWithComposedStrategy(TestStrategy strategy)
        {
            // define the strategy for Person
            var personStrategy = new DataCreationStrategy<Person>();

            personStrategy.Add(p => p.Age, new IntegerStrategy()).Min(10).Max(100);
            personStrategy.Add(p => p.FirstName, new StringStrategy()).Length(35);
            personStrategy.Add(p => p.LastName, new StringStrategy()).Length(20).Prefix("CC_");

            // add the strategy for adress to the strategy for person
            personStrategy.Add(p => p.HomeAddress, strategy.HomeAddress);
            personStrategy.Add(p => p.BusinessAddress, strategy.BusinessAddress);

            var items = Common.CreateItems(100, personStrategy);

            Assert.IsTrue(items.All(i => i.FirstName.Length == 35));
            Assert.IsTrue(items.All(i => i.LastName.Length == 20));
            Assert.IsTrue(items.All(i => i.LastName.StartsWith("CC_")));

            return new TestResult { Persons = new List<Person>(items) };
        }
    }

    public class TestStrategy
    {
        public DataCreationStrategy<Address> HomeAddress = new DataCreationStrategy<Address>();

        public DataCreationStrategy<Address> BusinessAddress = new DataCreationStrategy<Address>();
    }

    public class TestResult
    {
        protected internal IEnumerable<Person> Persons { get; set; }
    }



}
