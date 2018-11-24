using AutoMapper;
using dueltank.application.Models.Decks.Input;
using dueltank.core.Models.DeckDetails;

namespace dueltank.application.Mappings.Profiles
{
    public class DeckThumbnailProfile : Profile
    {
        public DeckThumbnailProfile()
        {
            CreateMap<DeckThumbnailInputModel, DeckThumbnail>();
        }
    }
}