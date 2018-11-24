using dueltank.application.Models.Decks.Input;
using MediatR;

namespace dueltank.application.Commands.UpdateDeckThumbnail
{
    public class UpdateDeckThumbnailCommand : IRequest<CommandResult>
    {
        public DeckThumbnailInputModel DeckThumbnail { get; set; }
    }
}