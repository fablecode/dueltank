using System;
using System.Drawing;
using System.IO;
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
                .NotNull()
                .Must(BeAValidImage)
                .WithMessage("'{PropertyName}' thumbnail is not a valid image.");
        }

        private static bool BeAValidImage(byte[] bytes)
        {
            try
            {
                using (var ms = new MemoryStream(bytes))
                {
                    Image.FromStream(ms);
                }
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }
    }
}