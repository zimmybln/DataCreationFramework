using DataCreationFramework.Tests.Types;
using DataCreationFramework.Data;
using NUnit.Framework;
using System.Linq;

namespace DataCreationFramework.Tests
{
    [TestFixture]
    public class DataValidationBoolean
    {
        [Test]
        public void CreateOpositeValue()
        {
            var validation = new DefinitionOfValid<PrimaryType>();

            validation.Add(p => p.AnBooleanValue).Value(true);

            var strategy = validation.CreateViolation(p => p.AnBooleanValue, ViolationType.OppositeValue);

            var items = Common.CreateItems(100, strategy);

            Assert.IsTrue(items.All(p => p.AnBooleanValue == false));
        }
    }
}
