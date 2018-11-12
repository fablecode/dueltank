using System;
using dueltank.application.Validations.Users;
using FluentAssertions;
using NUnit.Framework;

namespace dueltank.application.unit.tests.Validations.DeckTests
{
    [TestFixture]
    public class UserIdValidatorTests
    {
        private UserIdValidator _sut;
        private string _inputModel;

        [SetUp]
        public void SetUp()
        {
            _sut = new UserIdValidator();
            _inputModel = null;

        }

        [Test]
        public void Given_A_Null_UserId_Then_Validation_StopOnFirstFailure()
        {
            // Arrange

            // Act
            var results = _sut.Validate(_inputModel);

            // Assert
            results.Errors.Should().HaveCount(1);
        }

        [Test]
        public void Given_A_Empty_UserId_Then_Validation_StopOnFirstFailure()
        {
            // Arrange
            _inputModel = string.Empty;

            // Act
            var results = _sut.Validate(_inputModel);

            // Assert
            results.Errors.Should().HaveCount(1);
        }

        [Test]
        public void Given_An_Empty_UserId_Then_Should_Return_Empty_ErrorMessage()
        {
            // Arrange
            const string expected = "UserId should not be empty.";

            _inputModel = string.Empty;

            // Act
            var results = _sut.Validate(_inputModel);

            // Assert
            results.Errors.Should().ContainSingle(err => err.ErrorMessage == expected);
        }

        [Test]
        public void Given_An_Invalid_UserId_Then_Validation_Should_Fail()
        {
            // Arrange
            const string expected = "UserId not in the correct format.";
            _inputModel = "sdfjsafhasjdfaisd-sdfs";

            // Act
            var results = _sut.Validate(_inputModel);

            // Assert
            results.Errors.Should().ContainSingle(err => err.ErrorMessage == expected);
        }

        [Test]
        public void Given_A_Valid_UserId_Then_Validation_Should_Pass()
        {
            // Arrange
            _inputModel = Guid.NewGuid().ToString();

            // Act
            var results = _sut.Validate(_inputModel);

            // Assert
            results.Errors.Should().BeEmpty();
        }
    }
}