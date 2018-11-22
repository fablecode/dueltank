using AutoMapper;
using dueltank.api.Controllers;
using dueltank.application.Models.Decks.Input;

namespace dueltank.api.Mappings.Profiles
{
    public class SearchDecksByUserIdProfile : Profile
    {
        public SearchDecksByUserIdProfile()
        {
            CreateMap<SearchDecksByUserIdInputModel, DecksByUserIdInputModel>();
        }
    }
}