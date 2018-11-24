using System;
using System.Collections.Generic;
using System.Linq;
using dueltank.application.Models.Cards.Input;
using dueltank.application.Models.Decks.Input;
using FluentValidation;

namespace dueltank.application.Validations.Decks
{
    public class ExtraDeckValidator : AbstractValidator<DeckInputModel>
    {
        private static readonly string[] ValidCardTypes = {"fusion", "xyz", "synchro"};

        public ExtraDeckValidator()
        {
            RuleFor(d => d.ExtraDeck)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(Between0To15Cards)
                .WithMessage("{PropertyName} must be 0 to 15 cards.")
                .Must(AreAllCardTypesValid)
                .WithMessage("{PropertyName} has an invalid card.");
        }

        private static bool AreAllCardTypesValid(List<CardInputModel> deck)
        {
            return deck.All(c => ValidCardTypes.Contains(c.BaseType, StringComparer.OrdinalIgnoreCase));
        }

        private static bool Between0To15Cards(List<CardInputModel> deck)
        {
            return deck.Count <= 15;
        }
    }
}