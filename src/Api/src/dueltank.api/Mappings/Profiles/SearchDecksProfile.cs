using AutoMapper;
using dueltank.application.Models.Decks.Input;

namespace dueltank.api.Mappings.Profiles
{
    public class SearchDecksProfile : Profile
    {
        public SearchDecksProfile()
        {
            CreateMap<SearchDecksInputModel, DecksByUserIdInputModel>();
            CreateMap<SearchDecksInputModel, DecksByUsernameInputModel>();
        }
    }
}