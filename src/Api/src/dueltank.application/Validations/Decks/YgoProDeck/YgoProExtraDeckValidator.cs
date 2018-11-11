using System.Collections.Generic;
using dueltank.core.Constants;
using FluentValidation;

namespace dueltank.application.Validations.Deck.YgoProDeck
{
    public class YgoProExtraDeckValidator : AbstractValidator<core.Models.YgoPro.YgoProDeck>
    {
        public YgoProExtraDeckValidator()
        {
            RuleFor(d => d.Extra)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(Between0To15Cards)
                .WithMessage("{PropertyName} must be 0 to 15 cards.");
        }

        private static bool Between0To15Cards(List<long> deck)
        {
            return deck.Count <= DeckConstants.ExtraDeckMaxSize;
        }
    }
}