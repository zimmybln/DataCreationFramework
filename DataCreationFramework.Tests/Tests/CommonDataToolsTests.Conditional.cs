using DataCreationFramework.Tests.Types;
using DataCreationFramework.Data;
using NUnit.Framework;
using System.Linq;

namespace DataCreationFramework.Tests
{
    [TestFixture]
    public class CommonDataToolsTestsConditional
    {
        [Test]
        public void CreateValueConditional()
        {
            DataCreationStrategy<Person> strategyPerson = new DataCreationStrategy<Person>();

            // create default values
            strategyPerson.Add(p => p.Age, new IntegerStrategy()).OneOfTheseValues(30, 31, 32, 33);
            strategyPerson.Add(p => p.IsMember, new BooleanStrategy()).Toggle();

            // apply conditional values
            strategyPerson.When(p => p.Age == 32).Then(p => p.IsMember, new BooleanStrategy()).Value(true);

            // create items
            var items = Common.CreateItems(1000, strategyPerson).ToList();

            // assert data
            Assert.IsTrue(items.Where(p => p.Age == 32).All(p => p.IsMember == true));
            Assert.IsFalse(items.Where(p => p.Age != 32).All(p => p.IsMember == true));
        }


    }
}
