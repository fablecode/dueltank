using System.Linq;
using AutoMapper;
using dueltank.application.Models.Cards.Input;
using dueltank.application.Models.Cards.Output;
using dueltank.core.Constants;
using dueltank.core.Models.Cards;
using dueltank.core.Models.Db;

namespace dueltank.application.Mappings.Profiles
{
    public class CardProfile : Profile
    {
        public CardProfile()
        {
            CreateMap<CardInputModel, CardModel>();

            CreateMap<Card, CardDetailOutputModel>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => $"/api/images/cards?name={src.Name}"))
                .ForMember(dest => dest.Limit, opt => opt.MapFrom(src => src.BanlistCard.Any() ? src.BanlistCard.First().Limit.Name.ToLower() : LimitConstants.Unlimited.ToLower()));

            CreateMap<Card, CardOutputModel>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => $"/api/images/cards?name={src.Name}"))
                .ForMember(dest => dest.Limit, opt => opt.MapFrom(src => src.BanlistCard.Any() ? src.BanlistCard.First().Limit.Name.ToLower() : LimitConstants.Unlimited.ToLower()));
        }
    }
}