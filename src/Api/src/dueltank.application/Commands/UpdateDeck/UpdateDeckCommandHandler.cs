using System.Collections.Generic;
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
        private readonly IUserService _userService;

        public UpdateDeckCommandHandler(IValidator<DeckInputModel> validator, IDeckService deckService, IMapper mapper, IUserService userService)
        {
            _validator = validator;
            _deckService = deckService;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<CommandResult> Handle(UpdateDeckCommand request, CancellationToken cancellationToken)
        {
            var commandResult = new CommandResult();

            var validationResult = await _validator.ValidateAsync(request.Deck, ruleSet: $"default,{DeckValidator.UpdateDeckRuleSet}", cancellationToken: cancellationToken);

            if (validationResult.IsValid)
            {
                var isOwnerOfDeck = await _userService.IsUserDeckOwner(request.Deck.UserId, request.Deck.Id.GetValueOrDefault());

                if (isOwnerOfDeck)
                {
                    var deckModel = _mapper.Map<DeckModel>(request.Deck);

                    var result = await _deckService.Update(deckModel);

                    commandResult.Data = new UpdateDeckOutputModel { DeckId = result.Id };
                    commandResult.IsSuccessful = true;
                }
                else
                {
                    commandResult.Errors = new List<string> { "Insufficient permissions to modify this deck." };
                }
            }
            else
            {
                commandResult.Errors = validationResult.Errors.Select(err => err.ErrorMessage).ToList();
            }

            return commandResult;
        }
    }

    public class UpdateDeckOutputModel
    {
        public long DeckId { get; set; }
    }

    public class UpdateDeckThumbnailOutputModel
    {
        public long DeckId { get; set; }

        public string Thumbnail { get; set; }
    }
}