using dueltank.application.Models.Decks.Input;
using dueltank.application.Validations.Users;
using FluentValidation;

namespace dueltank.application.Validations.Decks
{
    public class DeckThumbnailValidator : AbstractValidator<DeckThumbnailInputModel>
    {
        public DeckThumbnailValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(dt => dt.UserId).UserIdValidator();

            RuleFor(dt => dt.DeckId).DeckIdValidator();

            RuleFor(dt => dt.Thumbnail)
                .NotNull();
        }
    }
}