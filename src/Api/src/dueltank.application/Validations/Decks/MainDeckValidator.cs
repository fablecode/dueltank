using System;
using System.Collections.Generic;
using System.Linq;
using dueltank.application.Models.Cards.Input;
using dueltank.application.Models.Decks.Input;
using FluentValidation;

namespace dueltank.application.Validations.Decks
{
    public class MainDeckValidator : AbstractValidator<DeckInputModel>
    {
        private const int MinimumCards = 40;
        private const int MaximumCards = 60;
        private static readonly string[] ValidCardTypes = { "monster", "spell", "trap" };

        public MainDeckValidator()
        {
            RuleFor(d => d.MainDeck)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .NotEmpty()
                .Must(Between40To60Cards)
                .WithMessage("{PropertyName} must have at least " + MinimumCards + " to " + MaximumCards + " cards.")
                .Must(AreAllCardTypesValid)
                .WithMessage("{PropertyName} has an invalid card.");

        }

        private static bool AreAllCardTypesValid(List<CardInputModel> deck)
        {
            return deck.All(c => ValidCardTypes.Contains(c.BaseType, StringComparer.OrdinalIgnoreCase));
        }

        private static bool Between40To60Cards(List<CardInputModel> deck)
        {
            return deck.Count >= MinimumCards && deck.Count <= MaximumCards;
        }
    }
}