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
                cardOutputModel = MapToMonsterCardOutputModel(deckCardSearch);
            }
            else if (SpellCardHelper.IsSpellCard(deckCardSearch))
            {
                cardOutputModel = MapToSpellCardOutputModel(deckCardSearch);
            }
            else if (TrapCardHelper.IsTrapCard(deckCardSearch))
            {
                cardOutputModel = MapToTrapCardOutputModel(deckCardSearch);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(deckCardSearch));
            }

            return cardOutputModel;
        }

        public static CardDetailOutputModel MapToMonsterCardOutputModel(DeckCardDetail deckCardSearch)
        {
            return MonsterCardHelper.MapToCardOutputModel(deckCardSearch);
        }

        public static CardDetailOutputModel MapToSpellCardOutputModel(DeckCardDetail deckCardSearch)
        {
            return SpellCardHelper.MapToCardOutputModel(deckCardSearch);
        }

        public static CardDetailOutputModel MapToTrapCardOutputModel(DeckCardDetail deckCardSearch)
        {
            return TrapCardHelper.MapToCardOutputModel(deckCardSearch);
        }
    }
}