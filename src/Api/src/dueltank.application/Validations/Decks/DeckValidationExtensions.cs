using FluentValidation;

namespace dueltank.application.Validations.Decks
{
    public static class DeckValidationExtensions
    {
        public static IRuleBuilderOptions<T, long?> DeckIdValidator<T>(this IRuleBuilder<T, long?> rule)
        {
            return rule.
                NotNull().
                SetValidator(new DeckIdValidator());
        }
    }
}