using System;
using dueltank.application.Commands.UploadYgoProDeck;
using FluentValidation;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace dueltank.application.unit.tests.Validations.Commands
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
        [TestCase("deck name deck name deck namedeck name deck name deck name deck name deck name deck name")]
        public void Given_An_Invalid_Name_Validation_Should_Fail(string name)
        {
            // Arrange
            var command = new UploadYgoProDeckCommand
            {
                Name = name
            };

            // Act
            Action act = () => _sut.ShouldHaveValidationErrorFor(c => c.Name, command);
            
            // Assert
            act.Invoke();
        }
    }

    public class UploadYgoProDeckCommandValidator : AbstractValidator<UploadYgoProDeckCommand>
    {
        public UploadYgoProDeckCommandValidator()
        {
            RuleFor(c => c.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .DeckNameValidator();
        }
    }


    public static class DeckValidationHelpers
    {
        public static IRuleBuilderOptions<T, string> DeckNameValidator<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .NotNull()
                .NotEmpty()
                .Length(3, 50)
                    .WithMessage("{PropertyName} must be between 3-50 characters.");
        }
    }
}