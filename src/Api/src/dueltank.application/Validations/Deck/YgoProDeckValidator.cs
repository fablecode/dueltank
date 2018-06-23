using System.Linq;
using dueltank.application.Validations.Helpers;
using dueltank.core.Models.YgoPro;
using FluentValidation;

namespace dueltank.application.Validations.Deck
{
    public class YgoProDeckValidator : AbstractValidator<YgoProDeck>
    {
        public YgoProDeckValidator()
        {
            RuleFor(d => d.Name)
                .DeckNameValidator();

            RuleFor(d => d.Description)
                .MaximumLength(4000);

            RuleFor(d => d)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(OnlyThreeCopiesOfTheSameCard)
                .WithMessage("You can only have up to 3 copies of the same card in your Main, Extra and Side Deck combined.");
        }

        private bool OnlyThreeCopiesOfTheSameCard(YgoProDeck deck)
        {
            var allCards =
                deck
                    .Main
                    .Concat(deck.Extra)
                    .Concat(deck.Side)
                    .ToList();

            var threeOrMoreCopies = allCards.GroupBy(c => c).Count(x => x.Count() > 3);

            return threeOrMoreCopies == 0;
        }
    }
}