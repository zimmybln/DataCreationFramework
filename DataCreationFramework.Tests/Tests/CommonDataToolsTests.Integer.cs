using DataCreationFramework.Tests.Types;
using DataCreationFramework.Data;
using DataCreationFramework.Extensions;
using NUnit.Framework;
using System;
using System.Linq;

namespace DataCreationFramework.Tests
{

    [TestFixture]
    public class CommonDataToolsTestsInteger
    {
        [Test]
        public void CreateItemsWithMinMax()
        {
            // Definiere die Datenstrategy
            var strategy = new DataCreationStrategy<Person>();

            strategy.Add(p => p.Age, new IntegerStrategy()).Min(10).Max(100);

            var peoply = Common.CreateItems(10, strategy).ToList();

            Assert.IsFalse(peoply.Any(p => p.Age < 10 || p.Age > 100));
        }

        [Test]
        public void CreateItemsWithIncrement()
        {
            // Definiere die Datenstrategy
            var strategy = new DataCreationStrategy<Person>();

            strategy.Add(p => p.Age, new IntegerStrategy()).Increment(20);

            var people = Common.CreateItems(10, strategy).ToList();

            Assert.IsTrue(people.Distinct().Count() == 10);
            Assert.IsTrue(people.Select(p => p.Age).IsAscending());
        }

        [Test]
        public void CreateItemsWithDecrement()
        {
            // Definiere die Datenstrategy
            var strategy = new DataCreationStrategy<Person>();

            strategy.Add(p => p.Age, new IntegerStrategy()).Decrement(20);

            var people = Common.CreateItems(100, strategy).ToList();

            Assert.IsTrue(people.Distinct().Count() == 100);
            Assert.IsTrue(people.Select(p => p.Age).IsDescending());

        }

        [Test]
        public void CreateItemsWithOneOfTheseValues()
        {
            // Definiere die Datenstrategy
            var strategy = new DataCreationStrategy<Person>();

            strategy.Add(p => p.Age, new IntegerStrategy()).OneOfTheseValues(10, 20, 30);

            var people = Common.CreateItems(100, strategy).ToList();

            var template = new[] { 10, 20, 30 };

            Assert.IsTrue(people.All(p => template.Contains(p.Age)));
            Assert.IsTrue(people.Any(p => p.Age == 10));
            Assert.IsTrue(people.Any(p => p.Age == 20));
            Assert.IsTrue(people.Any(p => p.Age == 30));
        }

        [Test]
        public void CreateItemsWithOneOfTheseValuesByCreatedStrategy()
        {
            // Definiere die Datenstrategy
            var strategy = new DataCreationStrategy<Person>();

            strategy.Add<int, IntegerStrategy>(p => p.Age).OneOfTheseValues(10, 20, 30);

            var people = Common.CreateItems(100, strategy).ToList();

            var template = new[] { 10, 20, 30 };

            Assert.IsTrue(people.All(p => template.Contains(p.Age)));
            Assert.IsTrue(people.Any(p => p.Age == 10));
            Assert.IsTrue(people.Any(p => p.Age == 20));
            Assert.IsTrue(people.Any(p => p.Age == 30));
        }

        [Test]
        public void CreateItemsWithExplicitValue()
        {
            var personStrategy = new DataCreationStrategy<Person>();

            personStrategy.Add(p => p.Age, new IntegerStrategy()).Value(42);

            var people = Common.CreateItems(100, personStrategy).ToList();

            Assert.IsTrue(people.All(p => p.Age == 42));
        }

        [Test]
        public void CreateItemsWithUniqueValue()
        {
            var personStrategy = new DataCreationStrategy<Person>();

            personStrategy.Add(p => p.Age, new IntegerStrategy()).Unique();

            var people = Common.CreateItems(200, personStrategy).ToList();

            Assert.IsTrue(people.Select(p => p.Age).Distinct().Count() == 200);
        }

        [Test]
        public void CreateItemsWithUniqueValueWithMin()
        {
            var personStrategy = new DataCreationStrategy<Person>();

            personStrategy.Add(p => p.Age, new IntegerStrategy()).Unique().Min(20);

            var people = Common.CreateItems(200, personStrategy).ToList();

            Assert.IsTrue(people.Select(p => p.Age).Distinct().Count() == 200);
            Assert.IsTrue(people.All(p => p.Age >= 20));
        }

        [Test]
        public void CreateItemsWithUniqueValueWithMax()
        {
            var personStrategy = new DataCreationStrategy<Person>();

            personStrategy.Add(p => p.Age, new IntegerStrategy()).Unique().Max(154);

            var people = Common.CreateItems(200, personStrategy).ToList();

            Assert.IsTrue(people.Select(p => p.Age).Distinct().Count() == 200);
            Assert.IsTrue(people.All(p => p.Age <= 154));
        }

    }

}
