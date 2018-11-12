using System;
using FluentValidation;

namespace dueltank.application.Validations.Users
{
    public class UserIdValidator : AbstractValidator<string>
    {
        public UserIdValidator()
        {
            RuleFor(id => id)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithMessage("{PropertyName} should not be empty.")
                .Must(IsValid)
                .WithMessage("{PropertyName} not in the correct format.")
                .WithName("UserId");
        }

        private bool IsValid(string userId)
        {
            return Guid.TryParse(userId, out _);
        }
    }
}