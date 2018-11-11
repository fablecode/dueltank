using NUnit.Framework;

namespace dueltank.application.unit.tests.Validations.DeckTests
{
    //[TestFixture]
    //public class DeckNameValidatorTests
    //{
    //    private DeckInputModel _inputModel;
    //    private DeckNameValidator _sut;

    //    [SetUp]
    //    public void SetUp()
    //    {
    //        _sut = new DeckNameValidator();
    //        _inputModel = new DeckInputModel();
    //    }

    //    [Test]
    //    public void Given_DeckName_When_EqualToNull_Validation_Fails()
    //    {
    //        // Arrange
    //        var expected = "'Deck name' must not be empty.";

    //        // Act
    //        var results = _sut.Validate(_inputModel);

    //        // Assert
    //        results.Errors.Should().ContainSingle(err => err.ErrorMessage == expected);
    //    }

    //    [Test]
    //    public void Given_DeckName_When_EqualToEmpty_Validation_Fails()
    //    {
    //        // Arrange
    //        var expected = "'Deck name' should not be empty.";
    //        _inputModel.Name = string.Empty;

    //        // Act
    //        var results = _sut.Validate(_inputModel);

    //        // Assert
    //        results.Errors.Should().ContainSingle(err => err.ErrorMessage == expected);
    //    }

    //    [Test]
    //    public void Given_DeckName_When_NotNullAndNotEmpty_Validation_Pass()
    //    {
    //        // Arrange
    //        _inputModel.Name = "deck name";

    //        // Act
    //        var results = _sut.Validate(_inputModel);

    //        // Assert
    //        results.Errors.Should().BeEmpty();
    //    }

    //    [Test]
    //    public void Given_DeckName_When_LengthIsLessThan_Minimun_Validation_Fails()
    //    {
    //        // Arrange
    //        var expected = "Deck name must be between 3-50 characters.";

    //        // Act
    //        _inputModel.Name = "de";
    //        var results = _sut.Validate(_inputModel);

    //        // Assert
    //        results.Errors.Should().ContainSingle(err => err.ErrorMessage == expected);
    //    }

    //    [Test]
    //    public void Given_DeckName_When_LengthIsGreaterThan_Max_Validation_Fails()
    //    {
    //        // Arrange
    //        var expected = "Deck name must be between 3-50 characters.";
    //        _inputModel.Name = "deck name deck name deck namedeck name deck name deck name deck name deck name deck name";

    //        // Act
    //        var results = _sut.Validate(_inputModel);

    //        // Assert
    //        results.Errors.Should().ContainSingle(err => err.ErrorMessage == expected);
    //    }
    //}
}