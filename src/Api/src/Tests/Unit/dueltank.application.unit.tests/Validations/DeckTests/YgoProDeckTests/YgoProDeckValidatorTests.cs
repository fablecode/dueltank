using System;
using System.Collections.Generic;
using dueltank.application.Validations.Deck;
using dueltank.application.Validations.Decks.YgoProDeck;
using dueltank.core.Models.YgoPro;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace dueltank.application.unit.tests.Validations.DeckTests.YgoProDeckTests
{
    [TestFixture]
    public class YgoProDeckValidatorTests
    {
        private YgoProDeckValidator _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new YgoProDeckValidator();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("deck name deck name deck namedeck name deck name deck name deck name deck name deck name")]
        public void Given_An_Invalid_Name_Validation_Should_Fail(string name)
        {
            // Arrange
            var ygoProDeck = new YgoProDeck
            {
                Name = name
            };

            // Act
            Action act = () => _sut.ShouldHaveValidationErrorFor(c => c.Name, ygoProDeck);

            // Assert
            act.Invoke();
        }

        [Test]
        public void Given_A_Description_With_More_Than_4000_Characters_Validation_Should_Fail()
        {
            // Arrange
            var ygoProDeck = new YgoProDeck
            {
                Description = new string('*', 4001)
            };

            // Act
            Action act = () => _sut.ShouldHaveValidationErrorFor(c => c.Description, ygoProDeck);

            // Assert
            act.Invoke();
        }

        [Test]
        public void Given_A_Deck_If_Card_Has_More_3_Copies_Of_The_Same_Card_In_Main_Extra_And_Side_Deck_Validation_Should_Fail()
        {
            // Arrange
            var ygoProDeck = new YgoProDeck
            {
                Name = "A Random Deck",
                Description = "A Description always helps",
                Main = new List<long> {41620959, 41620959},
                Extra = new List<long> {41620959},
                Side = new List<long> {41620959}
            };

            // Act
            var result = _sut.Validate(ygoProDeck);

            // Assert
            result.IsValid.Should().BeFalse();
        }
    }
}