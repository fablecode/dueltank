using System.Collections.Generic;
using dueltank.core.Constants;
using dueltank.core.Models.YgoPro;
using FluentValidation;

namespace dueltank.application.Validations.Deck
{
    public class YgoProExtraDeckValidator : AbstractValidator<YgoProDeck>
    {
        public YgoProExtraDeckValidator()
        {
            RuleFor(d => d.Extra)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(Between0To15Cards)
                .WithMessage("{PropertyName} must be 0 to 15 cards.");
        }

        private static bool Between0To15Cards(List<string> deck)
        {
            return deck.Count <= DeckConstants.ExtraDeckMaxSize;
        }
    }
}