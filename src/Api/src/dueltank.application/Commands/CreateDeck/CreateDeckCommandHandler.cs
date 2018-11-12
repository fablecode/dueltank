using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Models.Decks.Input;
using FluentValidation;
using MediatR;

namespace dueltank.application.Commands.CreateDeck
{
    public class CreateDeckCommandHandler : IRequestHandler<CreateDeckCommand, CommandResult>
    {
        private readonly IValidator<DeckInputModel> _validator;

        public CreateDeckCommandHandler(IValidator<DeckInputModel> validator)
        {
            _validator = validator;
        }

        public async Task<CommandResult> Handle(CreateDeckCommand request, CancellationToken cancellationToken)
        {
            var commandResult = new CommandResult();

            var validationResult = await _validator.ValidateAsync(request.Deck, cancellationToken);

            if (validationResult.IsValid)
            {

            }
            else
            {
                commandResult.Errors = validationResult.Errors.Select(err => err.ErrorMessage).ToList();
            }

            return commandResult;
        }
    }
}