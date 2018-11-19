using AutoMapper;
using dueltank.api.Controllers;

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