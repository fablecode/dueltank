using dueltank.application.Models.Decks.Input;
using MediatR;

namespace dueltank.application.Commands.UpdateDeck
{
    public class UpdateDeckCommand : IRequest<CommandResult>
    {
        public DeckInputModel Deck { get; set; }
    }
}