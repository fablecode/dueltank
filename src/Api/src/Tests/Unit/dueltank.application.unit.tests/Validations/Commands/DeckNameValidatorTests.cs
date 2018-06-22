using FluentValidation;
using NUnit.Framework;

namespace dueltank.application.unit.tests.Validations.Commands
{
    [TestFixture]
    public class YgoProDeckNameValidatorTests
    {
        [SetUp]
        public void Setup()
        {
            var sut = new YgoProDeckNameValidator();
        }
    }

    public class YgoProDeckNameValidator
    {
    }
}