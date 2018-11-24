using FluentValidation;

namespace dueltank.application.Validations.Decks
{
    public class DeckIdValidator : AbstractValidator<long?>
    {
        public DeckIdValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(id => id)
                .GreaterThan(0)
                .LessThanOrEqualTo(long.MaxValue)
                .WithName("DeckId");
        }
    }
}