using dueltank.application.Models.Decks.Input;
using dueltank.application.Validations.Deck;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace dueltank.application.unit.tests.Validations.DeckTests
{
    [TestFixture]
    public class DeckNameValidatorTests
    {
        private DeckInputModel _inputModel;
        private DeckNameValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new DeckNameValidator();
            _inputModel = new DeckInputModel();
        }

        [Test]
        public void Given_DeckName_When_EqualToNull_Validation_Fails()
        {
            // Arrange

            // Act

            // Assert
            _sut.ShouldHaveValidationErrorFor(m => m.Name, _inputModel);
        }

        [Test]
        public void Given_DeckName_When_EqualToEmpty_Validation_Fails()
        {
            // Arrange
            _inputModel.Name = string.Empty;

            // Act

            // Assert
            _sut.ShouldHaveValidationErrorFor(m => m.Name, _inputModel);
        }

        [Test]
        public void Given_DeckName_When_NotNullAndNotEmpty_Validation_Pass()
        {
            // Arrange
            _inputModel.Name = "deck name";

            // Act

            // Assert
            _sut.ShouldNotHaveValidationErrorFor(m => m.Name, _inputModel);
        }

        [Test]
        public void Given_DeckName_When_LengthIsLessThan_Minimun_Validation_Fails()
        {
            // Arrange
            _inputModel.Name = "de";

            // Act

            // Assert
            _sut.ShouldHaveValidationErrorFor(m => m.Name, _inputModel);
        }

        [Test]
        public void Given_DeckName_When_LengthIsGreaterThan_Max_Validation_Fails()
        {
            // Arrange
            _inputModel.Name = new string('*', 256);

            // Act

            // Assert
            _sut.ShouldHaveValidationErrorFor(m => m.Name, _inputModel);
        }
    }
}