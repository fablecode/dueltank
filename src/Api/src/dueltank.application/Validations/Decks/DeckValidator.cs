using System.Linq;
using dueltank.application.Models.Decks.Input;
using dueltank.application.Validations.Deck;
using dueltank.application.Validations.Users;
using FluentValidation;

namespace dueltank.application.Validations.Decks
{
    public class DeckValidator : AbstractValidator<DeckInputModel>
    {
        public const string InsertDeckRuleSet = "Insert";

        public DeckValidator()
        {
            Include(new DeckNameValidator());
            Include(new YoutubeUrlValidator());
            Include(new MainDeckValidator());
            Include(new ExtraDeckValidator());
            Include(new SideDeckValidator());

            RuleFor(d => d)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(OnlyThreeCopiesOfTheSameCard)
                .WithMessage("You can only have up to 3 copies of the same card in your Main, Extra and Side Deck combined.");

            RuleFor(d => d.UserId).UserIdValidator();

            RuleSet(InsertDeckRuleSet, () =>
            {
                RuleFor(d => d.Id)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .Must(d => !d.HasValue)
                    .WithMessage("{PropertyName} cannot have a value when creating a deck.");
            });
        }

        private static bool OnlyThreeCopiesOfTheSameCard(DeckInputModel deck)
        {
            var allCards =
                deck
                    .MainDeck
                    .Concat(deck.ExtraDeck)
                    .Concat(deck.SideDeck)
                    .ToList();

            var threeOrMoreCopies = allCards.GroupBy(c => c.Id).Count(x => x.Count() > 3);

            return threeOrMoreCopies == 0;
        }
    }
}