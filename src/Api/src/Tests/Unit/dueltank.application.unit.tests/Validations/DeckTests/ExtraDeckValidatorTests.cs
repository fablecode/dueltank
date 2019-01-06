using System.Linq;
using AutoFixture;
using dueltank.application.Models.Cards.Input;
using dueltank.application.Models.Decks.Input;
using dueltank.application.Validations.Decks;
using dueltank.tests.core;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace dueltank.application.unit.tests.Validations.DeckTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ExtraDeckValidatorTests
    {
        private Fixture _fixture;
        private DeckInputModel _inputModel;
        private ExtraDeckValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new ExtraDeckValidator();
            _inputModel = new DeckInputModel();
            _fixture = new Fixture();
        }

        [Test]
        public void Given_ExtraDeck_When_NumberOfCards_GreaterThan_15Cards_Validation_Fails()
        {
            // Arrange
            const string expected = "Extra deck must be 0 to 15 cards.";
            _fixture.RepeatCount = 16;
            _inputModel.ExtraDeck =
                _fixture
                    .Build<CardInputModel>()
                    .With(c => c.BaseType, "synchro")
                    .Without(c => c.Types)
                    .CreateMany()
                    .ToList();

            // Act
            var results = _sut.ShouldHaveValidationErrorFor(deck => deck.ExtraDeck, _inputModel);

            // Assert
            results.Should().ContainSingle(err => err.ErrorMessage == expected);
        }

        [Test]
        public void Given_ExtraDeck_When_NumberOfCards_Between0And15Cards_Validation_Pass()
        {
            // Arrange
            _fixture.RepeatCount = 10;
            _inputModel.ExtraDeck =
                        _fixture
                            .Build<CardInputModel>()
                            .With(c => c.BaseType, "fusion")
                            .Without(c => c.Types)
                            .CreateMany()
                            .ToList();

            // Act

            // Assert
            _sut.ShouldNotHaveValidationErrorFor(deck => deck.ExtraDeck, _inputModel);
        }

        [Test]
        public void Given_ExtraDeck_With_15Cards_IfAnyCard_IsInvalidCardType_Validation_Fails()
        {
            // Arrange
            const string expected = "Extra deck has an invalid card.";
            _fixture.RepeatCount = 15;
            _inputModel.ExtraDeck =
                _fixture
                    .Build<CardInputModel>()
                    .With(c => c.BaseType, "trap")
                    .Without(c => c.Types)
                    .CreateMany()
                    .ToList();

            // Act
            var results = _sut.ShouldHaveValidationErrorFor(deck => deck.ExtraDeck, _inputModel);

            // Assert
            results.Should().ContainSingle(err => err.ErrorMessage == expected);
        }
    }
}