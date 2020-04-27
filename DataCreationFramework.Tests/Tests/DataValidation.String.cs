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
        public void CreateForNullValue()
        {
            var validation = new DefinitionOfValid<Person>();

            validation.Add(p => p.FirstName)
                .NotNull()
                .MaxLength(150)
                .MinLength(5);

            var strategy = validation.CreateViolation(p => p.FirstName, ViolationType.Presence);

            var item = Common.CreateItem(strategy);

            Assert.IsTrue(String.IsNullOrEmpty(item.FirstName));

        }

        [Test]
        public void CreateForMaxValue()
        {
            var validation = new DefinitionOfValid<Person>();

            validation.Add(p => p.FirstName)
                .NotNull()
                .MaxLength(150)
                .MinLength(10);

            var strategy = validation.CreateViolation(p => p.FirstName, ViolationType.MaximumLength);

            var item = Common.CreateItem(strategy);

            Assert.IsTrue(item.FirstName.Length > 150);

        }

        [Test]
        public void CreateForMinValue()
        {
            var validation = new DefinitionOfValid<Person>();

            validation.Add(p => p.FirstName)
                .NotNull()
                .MaxLength(150)
                .MinLength(10);

            var strategy = validation.CreateViolation(p => p.FirstName, ViolationType.MinimumLength);

            var item = Common.CreateItem(strategy);

            Assert.IsTrue(item.FirstName.Length < 10);

        }

    }
}
