using System;
using dueltank.application.Models.Cards.Output;
using dueltank.core.Models.Cards;

namespace dueltank.application.Helpers
{
    public static class CardSearchMapperHelper
    {
        public static CardOutputModel MapToCardOutputModel(CardSearch cardSearch)
        {
            CardOutputModel cardOutputModel;
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

        public static CardOutputModel MapToMonsterCardOutputModel(CardSearch cardSearch)
        {
            return MonsterCardHelper.MapToCardOutputModel(cardSearch);
        }

        public static CardOutputModel MapToSpellCardOutputModel(CardSearch cardSearch)
        {
            return SpellCardHelper.MapToCardOutputModel(cardSearch);
        }

        public static CardOutputModel MapToTrapCardOutputModel(CardSearch cardSearch)
        {
            return TrapCardHelper.MapToCardOutputModel(cardSearch);
        }
    }
}