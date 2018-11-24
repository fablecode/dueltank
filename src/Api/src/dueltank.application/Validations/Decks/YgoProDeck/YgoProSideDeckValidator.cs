using System.Collections.Generic;
using dueltank.core.Constants;
using FluentValidation;

namespace dueltank.application.Validations.Decks.YgoProDeck
{
    public class YgoProSideDeckValidator : AbstractValidator<core.Models.YgoPro.YgoProDeck>
    {
        public YgoProSideDeckValidator()
        {
            RuleFor(d => d.Side)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(Between0To15Cards)
                .WithMessage("{PropertyName} must be 0 to 15 cards.");
        }

        private static bool Between0To15Cards(List<long> deck)
        {
            return deck.Count <= DeckConstants.SideDeckMaxSize;
        }
    }
}