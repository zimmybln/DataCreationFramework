using DataCreationFramework.Tests.Types;
using DataCreationFramework.Data;
using NUnit.Framework;
using System.Linq;

namespace DataCreationFramework.Tests
{
    [TestFixture]
    public class DataValidationInteger
    {
        [Test]
        public void CreateMaxValidation()
        {
            var validation = new DefinitionOfValid<PrimaryType>();

            validation.Add(p => p.AnIntegerValue)
                .Min(50)
                .Max(100);

            var strategy = validation.CreateViolation(nameof(PrimaryType.AnIntegerValue), ViolationType.MaximumValue);

            var item = Common.CreateItem(strategy);

            Assert.IsTrue(item.AnIntegerValue > 100);

        }

        [Test]
        public void CreateMaxValidationByExpression()
        {
            var validation = new DefinitionOfValid<PrimaryType>();

            validation.Add(p => p.AnIntegerValue)
                .Min(50)
                .Max(100);

            var strategy = validation.CreateViolation(p => p.AnIntegerValue, ViolationType.MaximumValue);

            var items = Common.CreateItems(100, strategy);

            Assert.IsTrue(items.All(p => p.AnIntegerValue < 50 || p.AnIntegerValue > 100));
        }

        [Test]
        public void CreateMinValidation()
        {
            var validation = new DefinitionOfValid<PrimaryType>();

            validation.Add(p => p.AnIntegerValue)
                .Min(50)
                .Max(100);

            var strategy = validation.CreateViolation(nameof(PrimaryType.AnIntegerValue), ViolationType.MinimumValue);

            var items = Common.CreateItems(100, strategy);

            Assert.IsTrue(items.All(p => p.AnIntegerValue < 50 || p.AnIntegerValue > 100));
        }

        [Test]
        public void CreateIsRequiredValidation()
        {
            var validation = new DefinitionOfValid<PrimaryType>();

            validation.Add(p => p.AnIntegerValue)
                .Min(50)
                .Max(100);

            var strategy = validation.CreateViolation(nameof(PrimaryType.AnIntegerValue), ViolationType.OppositeValue);

            var item = Common.CreateItem(strategy);

            Assert.IsTrue(item.AnIntegerValue == 0);
        }
    }
}
