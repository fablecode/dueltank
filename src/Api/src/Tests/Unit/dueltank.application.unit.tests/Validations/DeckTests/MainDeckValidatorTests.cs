using AutoFixture;
using dueltank.application.Models.Cards.Input;
using dueltank.application.Models.Decks.Input;
using dueltank.application.Validations.Decks;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using dueltank.tests.core;

namespace dueltank.application.unit.tests.Validations.DeckTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class MainDeckValidatorTests
    {
        private DeckInputModel _inputModel;
        private MainDeckValidator _sut;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _sut = new MainDeckValidator();
            _inputModel = new DeckInputModel();
            _fixture = new Fixture();
        }

        [Test]
        public void Given_MainDeck_When_Null_Validation_Fails()
        {
            // Arrange
            const string expected = "'Main deck' must not be empty.";

            // Act
            var results = _sut.ShouldHaveValidationErrorFor(md => md.MainDeck, _inputModel);

            // Assert
            results.Should().ContainSingle(err => err.ErrorMessage == expected);
        }

        [Test]
        public void Given_MainDeck_When_Empty_Validation_Fails()
        {
            // Arrange
            const string expected = "'Main deck' should not be empty.";

            _inputModel.MainDeck = new List<CardInputModel>();

            // Act
            var results = _sut.ShouldHaveValidationErrorFor(md => md.MainDeck, _inputModel);

            // Assert
            results.Should().ContainSingle(err => err.ErrorMessage == expected);
        }

        [Test]
        public void Given_MainDeck_When_NumberOfCards_LessThan_40Cards_Validation_Fails()
        {
            // Arrange
            const string expected = "Main deck must have at least 40 to 60 cards.";
            _fixture.RepeatCount = 39;
            _inputModel.MainDeck =
                        _fixture
                            .Build<CardInputModel>()
                            .With(c => c.BaseType, "monster")
                            .Without(c => c.Types)
                            .CreateMany()
                            .ToList();
            // Act
            var results = _sut.ShouldHaveValidationErrorFor(md => md.MainDeck, _inputModel);

            // Assert
            results.Should().ContainSingle(err => err.ErrorMessage == expected);
        }

        [Test]
        public void Given_MainDeck_When_NumberOfCards_GreaterThan_60Cards_Validation_Fails()
        {
            // Arrange
            var expected = "Main deck must have at least 40 to 60 cards.";
            _fixture.RepeatCount = 61;
            _inputModel.MainDeck =
                            _fixture
                                .Build<CardInputModel>()
                                .With(c => c.BaseType, "monster")
                                .Without(c => c.Types)
                                .CreateMany()
                                .ToList();

            // Act
            var results = _sut.ShouldHaveValidationErrorFor(md => md.MainDeck, _inputModel);

            // Assert
            results.Should().ContainSingle(err => err.ErrorMessage == expected);
        }

        [Test]
        public void Given_MainDeck_When_NumberOfCards_Between40And60Cards_Validation_Pass()
        {
            // Arrange
            _fixture.RepeatCount = 50;
            _inputModel.MainDeck =
                    _fixture
                        .Build<CardInputModel>()
                        .With(c => c.BaseType, "monster")
                        .Without(c => c.Types)
                        .CreateMany()
                        .ToList();

            // Act

            // Assert
            _sut.ShouldNotHaveValidationErrorFor(md => md.MainDeck, _inputModel);
        }

        [Test]
        public void Given_MainDeck_With_40Cards_IfAnyCard_IsInvalidCardType_Validation_Fails()
        {
            // Arrange
            var expected = "Main deck has an invalid card.";
            _fixture.RepeatCount = 40;
            _inputModel.MainDeck =
                    _fixture
                        .Build<CardInputModel>()
                        .With(c => c.BaseType, "fusion")
                        .Without(c => c.Types)
                        .CreateMany()
                        .ToList();

            // Act
            var results = _sut.ShouldHaveValidationErrorFor(md => md.MainDeck, _inputModel);

            // Assert
            results.Should().ContainSingle(err => err.ErrorMessage == expected);
        }

        [Test]
        public void Given_MainDeck_With_40Cards_IfAllCards_AreValidCardTypes_Validation_Pass()
        {
            // Arrange
            _fixture.RepeatCount = 40;
            _inputModel.MainDeck =
                _fixture
                    .Build<CardInputModel>()
                    .With(c => c.BaseType, "trap")
                    .Without(c => c.Types)
                    .CreateMany()
                    .ToList();

            // Act

            // Assert
            _sut.ShouldNotHaveValidationErrorFor(md => md.MainDeck, _inputModel);
        }
    }
}