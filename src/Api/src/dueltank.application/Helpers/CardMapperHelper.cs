using System;
using dueltank.application.Models.Cards.Output;
using dueltank.core.Models.Cards;

namespace dueltank.application.Helpers
{
    public static class CardMapperHelper
    {
        public static CardDetailOutputModel MapToCardOutputModel(DeckCardDetail deckCardSearch)
        {
            CardDetailOutputModel cardOutputModel;
            if (MonsterCardHelper.IsMonsterCard(deckCardSearch))
            {
                cardOutputModel = MonsterCardHelper.MapToCardOutputModel(deckCardSearch);
            }
            else if (SpellCardHelper.IsSpellCard(deckCardSearch))
            {
                cardOutputModel = SpellCardHelper.MapToCardOutputModel(deckCardSearch);
            }
            else if (TrapCardHelper.IsTrapCard(deckCardSearch))
            {
                cardOutputModel = TrapCardHelper.MapToCardOutputModel(deckCardSearch);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(deckCardSearch));
            }

            return cardOutputModel;
        }
    }
}