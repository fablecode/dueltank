using AutoMapper;
using dueltank.application.Models.Decks.Input;
using dueltank.core.Models.DeckDetails;

namespace dueltank.application.Mappings.Profiles
{
    public class DeckProfile : Profile
    {
        public DeckProfile()
        {
            CreateMap<DeckInputModel, DeckModel>();
        }
    }
}