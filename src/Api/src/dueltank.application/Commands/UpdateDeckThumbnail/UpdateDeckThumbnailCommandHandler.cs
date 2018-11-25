using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using dueltank.application.Commands.UpdateDeck;
using dueltank.application.Models.Decks.Input;
using dueltank.core.Models.DeckDetails;
using dueltank.core.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;

namespace dueltank.application.Commands.UpdateDeckThumbnail
{
    public class UpdateDeckThumbnailCommandHandler : IRequestHandler<UpdateDeckThumbnailCommand, CommandResult>
    {
        private readonly IOptions<ApplicationSettings> _settings;
        private readonly IValidator<DeckThumbnailInputModel> _validator;
        private readonly IUserService _userService;
        private readonly IDeckService _deckService;
        private readonly IMapper _mapper;

        public UpdateDeckThumbnailCommandHandler
        (
            IOptions<ApplicationSettings> settings, 
            IValidator<DeckThumbnailInputModel> validator, 
            IUserService userService,
            IDeckService deckService,
            IMapper mapper
        )
        {
            _settings = settings;
            _validator = validator;
            _userService = userService;
            _deckService = deckService;
            _mapper = mapper;
        }
        public async Task<CommandResult> Handle(UpdateDeckThumbnailCommand request, CancellationToken cancellationToken)
        {
            var commandResult = new CommandResult();

            var validationResult = _validator.Validate(request.DeckThumbnail);

            if (validationResult.IsValid)
            {
                var isOwnerOfDeck = await _userService.IsUserDeckOwner(request.DeckThumbnail.UserId, request.DeckThumbnail.DeckId.GetValueOrDefault());

                if (isOwnerOfDeck)
                {
                    var deckThumbnailModel = _mapper.Map<DeckThumbnail>(request.DeckThumbnail);

                    deckThumbnailModel.ImageFilePath = Path.Combine(_settings.Value.DeckThumbnailImageFolderPath, $"{request.DeckThumbnail.DeckId}.png");

                    var result = _deckService.SaveDeckThumbnail(deckThumbnailModel);

                    commandResult.Data = new UpdateDeckThumbnailOutputModel { DeckId = result, Thumbnail = $"{request.DeckThumbnail.DeckId}.png" };
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
}