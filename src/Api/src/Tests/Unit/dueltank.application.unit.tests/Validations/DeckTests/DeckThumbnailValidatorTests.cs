using dueltank.application.Models.Decks.Input;
using dueltank.application.Validations.Decks;
using FluentAssertions;
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
            var expected = "'Thumbnail' must not be empty.";

            // Act
            var results = _sut.Validate(_inputModel);

            // Assert
            results.Errors.Should().ContainSingle(err => err.ErrorMessage == expected);
        }
    }
}