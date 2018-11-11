using dueltank.application.Models.Decks.Input;
using FluentValidation;

namespace dueltank.application.Validations.Deck
{
    public class DeckNameValidator : AbstractValidator<DeckInputModel>
    {
        public DeckNameValidator()
        {
            RuleFor(d => d.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .NotEmpty()
                .Length(3, 255)
                .WithMessage("{PropertyName} must be between 3-255 characters.");
        }
    }
}