using System.Linq;
using dueltank.application.Validations.Helpers;
using dueltank.core.Constants;
using FluentValidation;

namespace dueltank.application.Validations.Deck.YgoProDeck
{
    public class YgoProDeckValidator : AbstractValidator<core.Models.YgoPro.YgoProDeck>
    {
        public YgoProDeckValidator()
        {
            RuleFor(d => d.Name)
                .DeckNameValidator();

            Include(new YgoProMainDeckValidator());
            Include(new YgoProExtraDeckValidator());
            Include(new YgoProSideDeckValidator());

            RuleFor(d => d.Description)
                .MaximumLength(4000);

            RuleFor(d => d)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(OnlyThreeCopiesOfTheSameCard)
                .WithMessage("You can only have up to 3 copies of the same card in your Main, Extra and Side Deck combined.");
        }

        private static bool OnlyThreeCopiesOfTheSameCard(core.Models.YgoPro.YgoProDeck deck)
        {
            var allCards =
                deck
                    .Main
                    .Concat(deck.Extra)
                    .Concat(deck.Side)
                    .ToList();

            var threeOrMoreCopies = allCards.GroupBy(c => c).Count(x => x.Count() > CardConstants.MaxCardCopies);

            return threeOrMoreCopies == 0;
        }
    }
}