using DataCreationFramework.Tests.Types;
using DataCreationFramework.Data;
using NUnit.Framework;
using System;

namespace DataCreationFramework.Tests
{
    [TestFixture]
    public class DataValidationString
    {
        [Test]
        public void CreateOnNullValue()
        {
            var validation = new DefinitionOfValid<PrimaryType>();

            validation.Add(p => p.AnStringValue)
                .NotNull()
                .MaxLength(150)
                .MinLength(10);

            var strategy = validation.CreateViolation(p => p.AnStringValue, ViolationType.Presence);

            var item = Common.CreateItem(strategy);

            Assert.IsTrue(String.IsNullOrEmpty(item.AnStringValue));

        }

        [Test]
        public void CreateOnMaxValue()
        {
            var validation = new DefinitionOfValid<PrimaryType>();

            validation.Add(p => p.AnStringValue)
                .NotNull()
                .MaxLength(150)
                .MinLength(10);

            var strategy = validation.CreateViolation(p => p.AnStringValue, ViolationType.MaximumLength);

            var item = Common.CreateItem(strategy);

            Assert.IsTrue(item.AnStringValue.Length > 150);

        }

        [Test]
        public void CreateOnMinValue()
        {
            var validation = new DefinitionOfValid<PrimaryType>();

            validation.Add(p => p.AnStringValue)
                .NotNull()
                .MaxLength(150)
                .MinLength(10);

            var strategy = validation.CreateViolation(p => p.AnStringValue, ViolationType.MinimumLength);

            var item = Common.CreateItem(strategy);

            Assert.IsTrue(item.AnStringValue.Length < 10);

        }

    }
}
