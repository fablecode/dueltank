using AutoMapper;
using dueltank.application.Models.Cards.Input;
using dueltank.core.Models.Cards;

namespace dueltank.application.Mappings.Profiles
{
    public class CardProfile : Profile
    {
        public CardProfile()
        {
            CreateMap<CardInputModel, CardModel>();
        }
    }
}