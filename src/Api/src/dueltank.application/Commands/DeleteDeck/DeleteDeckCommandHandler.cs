using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using dueltank.core.Services;
using MediatR;

namespace dueltank.application.Commands.DeleteDeck
{
    public class DeleteDeckCommandHandler : IRequestHandler<DeleteDeckCommand, CommandResult>
    {
        private readonly IUserService _userService;
        private readonly IDeckService _deckService;

        public DeleteDeckCommandHandler(IUserService userService, IDeckService deckService)
        {
            _userService = userService;
            _deckService = deckService;
        }

        public async Task<CommandResult> Handle(DeleteDeckCommand request, CancellationToken cancellationToken)
        {
            var commandResult = new CommandResult();

            var isOwnerOfDeck = await _userService.IsUserDeckOwner(request.Deck.UserId, request.Deck.Id.GetValueOrDefault());

            if (isOwnerOfDeck)
            {
                await _deckService.DeleteDeckByIdAndUserId(request.Deck.UserId, request.Deck.Id.GetValueOrDefault());

                commandResult.Data = request.Deck.Id.GetValueOrDefault();
                commandResult.IsSuccessful = true;
            }
            else
            {
                commandResult.Errors = new List<string> { "Insufficient permissions to delete this deck." };
            }

            return commandResult;
        }
    }
}