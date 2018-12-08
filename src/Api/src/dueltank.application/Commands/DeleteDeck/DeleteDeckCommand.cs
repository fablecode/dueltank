using dueltank.application.Models.Decks.Input;
using MediatR;

namespace dueltank.application.Commands.DeleteDeck
{
    public class DeleteDeckCommand : IRequest<CommandResult>
    {
        public DeckInputModel Deck { get; set; }
    }
}