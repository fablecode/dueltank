using System;
using dueltank.application.Commands.UploadYgoProDeck;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace dueltank.application.unit.tests.Commands.Validations
{
    [TestFixture]
    public class UploadYgoProDeckCommandValidatorTests
    {
        private UploadYgoProDeckCommandValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new UploadYgoProDeckCommandValidator();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Given_An_Invalid_Deck_Validation_Should_Fail(string deck)
        {
            var command = new UploadYgoProDeckCommand
            {
                Deck = deck
            };

            // Act
            Action act = () => _sut.ShouldHaveValidationErrorFor(c => c.Deck, command);

            // Assert
            act.Invoke();
        }
    }
}