using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using dueltank.application.Models.Decks.Input;
using dueltank.application.Validations.Decks;
using dueltank.core.Models.DeckDetails;
using dueltank.core.Services;
using FluentValidation;
using MediatR;

namespace dueltank.application.Commands.UpdateDeck
{
    public class UpdateDeckCommandHandler : IRequestHandler<UpdateDeckCommand, CommandResult>
    {
        private readonly IValidator<DeckInputModel> _validator;
        private readonly IDeckService _deckService;
        private readonly IMapper _mapper;

        public UpdateDeckCommandHandler(IValidator<DeckInputModel> validator, IDeckService deckService, IMapper mapper)
        {
            _validator = validator;
            _deckService = deckService;
            _mapper = mapper;
        }

        public async Task<CommandResult> Handle(UpdateDeckCommand request, CancellationToken cancellationToken)
        {
            var commandResult = new CommandResult();

            var validationResult = await _validator.ValidateAsync(request.Deck, ruleSet: $"default,{DeckValidator.UpdateDeckRuleSet}", cancellationToken: cancellationToken);

            if (validationResult.IsValid)
            {
                var deckModel = _mapper.Map<DeckModel>(request.Deck);

                var result = await _deckService.Update(deckModel);

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