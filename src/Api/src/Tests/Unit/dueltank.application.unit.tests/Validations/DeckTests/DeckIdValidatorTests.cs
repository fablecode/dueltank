using dueltank.application.Validations.Decks;
using dueltank.tests.core;
using FluentAssertions;
using NUnit.Framework;

namespace dueltank.application.unit.tests.Validations.DeckTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class DeckIdValidatorTests
    {
        private DeckIdValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new DeckIdValidator();
        }

        [Test]
        public void Given_DeckId_When_EqualsZero_Validation_Fails()
        {
            // Arrange
            const string expected = "'DeckId' must be greater than '0'.";
            const int input = 0;

            // Act
            var results = _sut.Validate(input);

            // Assert
            results.Errors.Should().ContainSingle(err => err.ErrorMessage == expected);
        }

        [Test]
        public void Given_DeckId_When_LessThanZero_Validation_Fails()
        {
            // Arrange
            const string expected = "'DeckId' must be greater than '0'.";
            const int input = -23;

            // Act
            var results = _sut.Validate(input);

            // Assert
            results.Errors.Should().ContainSingle(err => err.ErrorMessage == expected);
        }

        [Test]
        public void Given_DeckId_When_GreaterThanZero_Validation_Pass()
        {
            // Arrange
            const int input = 56;

            // Act
            var results = _sut.Validate(input);

            // Assert
            results.Errors.Should().BeEmpty();
        }
    }
}