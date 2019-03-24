using System;
using AutoMapper;
using dueltank.application.Models.Cards.Output;
using dueltank.core.Models.Cards;

namespace dueltank.application.Helpers
{
    public static class CardMapperHelper
    {
        public static CardDetailOutputModel MapToCardOutputModel(IMapper mapper, DeckCardDetail deckCardSearch)
        {
            CardDetailOutputModel cardOutputModel;
            if (MonsterCardHelper.IsMonsterCard(deckCardSearch))
            {
                cardOutputModel = MonsterCardHelper.MapToCardOutputModel(mapper, deckCardSearch);
            }
            else if (SpellCardHelper.IsSpellCard(deckCardSearch))
            {
                cardOutputModel = SpellCardHelper.MapToCardOutputModel(mapper, deckCardSearch);
            }
            else if (TrapCardHelper.IsTrapCard(deckCardSearch))
            {
                cardOutputModel = TrapCardHelper.MapToCardOutputModel(mapper, deckCardSearch);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(deckCardSearch));
            }

            return cardOutputModel;
        }
    }
}