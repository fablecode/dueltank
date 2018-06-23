using FluentValidation;

namespace dueltank.application.Commands.UploadYgoProDeck
{
    public class UploadYgoProDeckCommandValidator : AbstractValidator<UploadYgoProDeckCommand>
    {
        public UploadYgoProDeckCommandValidator()
        {
            RuleFor(c => c.Deck)
                .NotNull()
                .NotEmpty();
        }
    }
}