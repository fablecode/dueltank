using System;
using System.Linq;
using dueltank.application.Models.Decks.Input;
using dueltank.application.Validations.Decks;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace dueltank.application.unit.tests.Validations.DeckTests
{
    [TestFixture]
    public class DeckThumbnailValidatorTests
    {
        private DeckThumbnailValidator _sut;
        private DeckThumbnailInputModel _inputModel;

        [SetUp]
        public void SetUp()
        {
            _sut = new DeckThumbnailValidator();
            _inputModel = new DeckThumbnailInputModel();
        }

        [Test]
        public void Given_A_DeckThumbnailInputModel_If_The_Thumbnail_IsNull_ValidationFails()
        {
            // Arrange
            const string expected = "'Thumbnail' must not be empty.";

            // Act
            var results = _sut.Validate(_inputModel);

            // Assert
            results.Errors.Should().ContainSingle(err => err.ErrorMessage == expected);
        }

        [Test]
        public void Given_An_Invalid_Thumbnail_File_Validation_Should_Fail()
        {
            // Arrange
            _inputModel.Thumbnail = new byte[0];

            // Act
            var results = _sut.ShouldHaveValidationErrorFor(t => t.Thumbnail, _inputModel);

            // Assert
            results.Should().ContainSingle(err => err.ErrorMessage == "'Thumbnail' thumbnail is not a valid image.");
        }
    }
}