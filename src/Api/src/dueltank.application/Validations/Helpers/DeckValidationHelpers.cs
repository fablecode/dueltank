using FluentValidation;

namespace dueltank.application.Validations.Helpers
{
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