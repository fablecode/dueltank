using System.Linq;
using AutoFixture;
using dueltank.application.Validations.Deck;
using dueltank.core.Models.YgoPro;
using FluentAssertions;
using NUnit.Framework;

namespace dueltank.application.unit.tests.Validations.DeckTests
{
    [TestFixture]
    public class YgoProExtraDeckValidatorTests
    {
        private Fixture _fixture;
        private YgoProDeck _inputModel;
        private YgoProExtraDeckValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new YgoProExtraDeckValidator();
            _inputModel = new YgoProDeck();
            _fixture = new Fixture();
        }

        [Test]
        public void Given_ExtraDeck_When_NumberOfCards_GreaterThan_15Cards_Validation_Fails()
        {
            // Arrange
            var expected = "Extra deck must be 0 to 15 cards.";
            _fixture.RepeatCount = 16;
            _inputModel.Extra =
                _fixture
                    .Build<string>()
                    .CreateMany()
                    .ToList();

            // Act
            var results = _sut.Validate(_inputModel);

            // Assert
            results.Errors.Should().ContainSingle(err => err.ErrorMessage == expected);
        }

        [Test]
        public void Given_ExtraDeck_When_NumberOfCards_Between0And15Cards_Validation_Pass()
        {
            // Arrange
            _fixture.RepeatCount = 10;
            _inputModel.Extra =
                _fixture
                    .Build<string>()
                    .CreateMany()
                    .ToList();

            // Act
            var results = _sut.Validate(_inputModel);

            // Assert
            results.Errors.Should().BeEmpty();
        }
    }
}