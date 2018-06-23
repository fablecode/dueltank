using dueltank.application.Validations.Helpers;
using FluentValidation;

namespace dueltank.application.Commands.UploadYgoProDeck
{
    public class UploadYgoProDeckCommandValidator : AbstractValidator<UploadYgoProDeckCommand>
    {
        public UploadYgoProDeckCommandValidator()
        {
            RuleFor(c => c.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .DeckNameValidator();

            RuleFor(c => c.Deck)
                .NotNull()
                .NotEmpty();
        }
    }
}