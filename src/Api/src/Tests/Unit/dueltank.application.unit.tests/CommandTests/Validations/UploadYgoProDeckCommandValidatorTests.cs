﻿using System;
using dueltank.application.Commands.UploadYgoProDeck;
using dueltank.tests.core;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace dueltank.application.unit.tests.CommandTests.Validations
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class UploadYgoProDeckCommandValidatorTests
    {
        [SetUp]
        public void SetUp()
        {
            _sut = new UploadYgoProDeckCommandValidator();
        }

        private UploadYgoProDeckCommandValidator _sut;

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