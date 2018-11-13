using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using dueltank.application.Models.Decks.Input;
using dueltank.core.Models.DeckDetails;
using dueltank.core.Services;
using FluentValidation;
using MediatR;

namespace dueltank.application.Commands.CreateDeck
{
    public class CreateDeckCommandHandler : IRequestHandler<CreateDeckCommand, CommandResult>
    {
        private readonly IValidator<DeckInputModel> _validator;
        private readonly IDeckService _deckService;
        private readonly IMapper _mapper;

        public CreateDeckCommandHandler(IValidator<DeckInputModel> validator, IDeckService deckService, IMapper mapper)
        {
            _validator = validator;
            _deckService = deckService;
            _mapper = mapper;
        }

        public async Task<CommandResult> Handle(CreateDeckCommand request, CancellationToken cancellationToken)
        {
            var commandResult = new CommandResult();

            var validationResult = await _validator.ValidateAsync(request.Deck, cancellationToken);

            if (validationResult.IsValid)
            {
                var deckModel = _mapper.Map<DeckModel>(request.Deck);

                var result = await _deckService.Add(deckModel);

                commandResult.Data = result.Id;
            }
            else
            {
                commandResult.Errors = validationResult.Errors.Select(err => err.ErrorMessage).ToList();
            }

            return commandResult;
        }
    }
}