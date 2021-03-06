﻿using System;
using AutoMapper;
using dueltank.application.Models.Cards.Output;
using dueltank.core.Models.Cards;
using dueltank.core.Models.Search.Banlists;

namespace dueltank.application.Helpers
{
    public static class CardSearchMapperHelper
    {
        public static CardOutputModel MapToCardOutputModel(IMapper mapper, CardSearch cardSearch)
        {
            CardOutputModel cardOutputModel;
            if (MonsterCardHelper.IsMonsterCard(cardSearch))
            {
                cardOutputModel = MonsterCardHelper.MapToCardOutputModel(mapper, cardSearch);
            }
            else if (SpellCardHelper.IsSpellCard(cardSearch))
            {
                cardOutputModel = SpellCardHelper.MapToCardOutputModel(mapper, cardSearch);
            }
            else if (TrapCardHelper.IsTrapCard(cardSearch))
            {
                cardOutputModel = TrapCardHelper.MapToCardOutputModel(mapper, cardSearch);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(cardSearch));
            }

            return cardOutputModel;
        }

        public static CardOutputModel MapToCardOutputModel(IMapper mapper, BanlistCardSearch cardSearch)
        {
            CardOutputModel cardOutputModel;
            if (MonsterCardHelper.IsMonsterCard(cardSearch))
            {
                cardOutputModel = MonsterCardHelper.MapToCardOutputModel(mapper, cardSearch);
            }
            else if (SpellCardHelper.IsSpellCard(cardSearch))
            {
                cardOutputModel = SpellCardHelper.MapToCardOutputModel(mapper, cardSearch);
            }
            else if (TrapCardHelper.IsTrapCard(cardSearch))
            {
                cardOutputModel = TrapCardHelper.MapToCardOutputModel(mapper, cardSearch);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(cardSearch));
            }

            return cardOutputModel;
        }
    }
}