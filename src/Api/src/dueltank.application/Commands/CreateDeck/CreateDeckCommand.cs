using dueltank.application.Models.Decks.Input;
using MediatR;

namespace dueltank.application.Commands.CreateDeck
{
    public class CreateDeckCommand : IRequest<CommandResult>
    {
        public DeckInputModel Deck { get; set; }
    }
}