using System;
using dueltank.application.Models.Cards.Output;
using dueltank.core.Models.Cards;

namespace dueltank.application.Helpers
{
    public static class CardMapperHelper
    {
        public static CardDetailOutputModel MapToCardOutputModel(CardDetail cardSearch)
        {
            CardDetailOutputModel cardOutputModel;
            if (MonsterCardHelper.IsMonsterCard(cardSearch))
            {
                cardOutputModel = MapToMonsterCardOutputModel(cardSearch);
            }
            else if (SpellCardHelper.IsSpellCard(cardSearch))
            {
                cardOutputModel = MapToSpellCardOutputModel(cardSearch);
            }
            else if (TrapCardHelper.IsTrapCard(cardSearch))
            {
                cardOutputModel = MapToTrapCardOutputModel(cardSearch);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(cardSearch));
            }

            return cardOutputModel;
        }

        public static CardDetailOutputModel MapToMonsterCardOutputModel(CardDetail cardSearch)
        {
            return MonsterCardHelper.MapToCardOutputModel(cardSearch);
        }

        public static CardDetailOutputModel MapToSpellCardOutputModel(CardDetail cardSearch)
        {
            return SpellCardHelper.MapToCardOutputModel(cardSearch);
        }

        public static CardDetailOutputModel MapToTrapCardOutputModel(CardDetail cardSearch)
        {
            return TrapCardHelper.MapToCardOutputModel(cardSearch);
        }
    }
}