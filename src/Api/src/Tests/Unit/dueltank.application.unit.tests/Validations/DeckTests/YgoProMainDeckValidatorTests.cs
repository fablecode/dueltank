using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using dueltank.application.Validations.Deck;
using dueltank.core.Models.YgoPro;
using FluentAssertions;
using NUnit.Framework;

namespace dueltank.application.unit.tests.Validations.DeckTests
{
    [TestFixture]
    public class YgoProMainDeckValidatorTests
    {
        private YgoProDeck _inputModel;
        private YgoProMainDeckValidator _sut;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _sut = new YgoProMainDeckValidator();
            _inputModel = new YgoProDeck();
            _fixture = new Fixture();
        }

        [Test]
        public void Given_MainDeck_When_Empty_Validation_Fails()
        {
            // Arrange
            var expected = "'Main deck' should not be empty.";

            _inputModel.Main = new List<string>();

            // Act
            var results = _sut.Validate(_inputModel);

            // Assert
            results.Errors.Should().ContainSingle(err => err.ErrorMessage == expected);
        }

        [Test]
        public void Given_MainDeck_When_NumberOfCards_LessThan_40Cards_Validation_Fails()
        {
            // Arrange
            var expected = "Main deck must have at least 40 to 60 cards.";
            _fixture.RepeatCount = 39;
            _inputModel.Main =
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
        public void Given_MainDeck_When_NumberOfCards_GreaterThan_60Cards_Validation_Fails()
        {
            // Arrange
            var expected = "Main deck must have at least 40 to 60 cards.";
            _fixture.RepeatCount = 61;
            _inputModel.Main =
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
        public void Given_MainDeck_When_NumberOfCards_Between40And60Cards_Validation_Pass()
        {
            // Arrange
            _fixture.RepeatCount = 50;
            _inputModel.Main =
                _fixture
                    .Build<string>()
                    .CreateMany()
                    .ToList();

            // Act
            var results = _sut.Validate(_inputModel);

            // Assert
            results.Errors.Should().BeEmpty();
        }

        [Test]
        public void Given_MainDeck_With_40Cards_IfAllCards_AreValidCardTypes_Validation_Pass()
        {
            // Arrange
            _fixture.RepeatCount = 40;
            _inputModel.Main =
                _fixture
                    .Build<string>()
                    .CreateMany()
                    .ToList();

            // Act
            var results = _sut.Validate(_inputModel);

            // Assert
            results.IsValid.Should().BeTrue();
        }
    }
}