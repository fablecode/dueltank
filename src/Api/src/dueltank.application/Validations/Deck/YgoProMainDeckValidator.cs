using System.Collections.Generic;
using dueltank.core.Constants;
using dueltank.core.Models.YgoPro;
using FluentValidation;

namespace dueltank.application.Validations.Deck
{
    public class YgoProMainDeckValidator : AbstractValidator<YgoProDeck>
    {
        public YgoProMainDeckValidator()
        {
            RuleFor(d => d.Main)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .NotEmpty()
                .Must(Between40To60Cards)
                .WithMessage("{PropertyName} must have at least 40 to 60 cards.");
        }

        private static bool Between40To60Cards(List<long> deck)
        {
            return deck.Count >= DeckConstants.MainDeckMinSize && deck.Count <= DeckConstants.MainDeckMaxSize;
        }
    }
}