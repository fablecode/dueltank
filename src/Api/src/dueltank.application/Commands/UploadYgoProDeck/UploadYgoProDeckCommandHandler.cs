using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dueltank.core.Helpers;
using dueltank.core.Models.YgoPro;
using FluentValidation;
using MediatR;

namespace dueltank.application.Commands.UploadYgoProDeck
{
    public class UploadYgoProDeckCommandHandler : IRequestHandler<UploadYgoProDeckCommand, CommandResult>
    {
        private readonly IValidator<UploadYgoProDeckCommand> _commandValidator;
        private readonly IValidator<YgoProDeck> _ygoProDeckValidator;

        public UploadYgoProDeckCommandHandler(IValidator<UploadYgoProDeckCommand> commandValidator, IValidator<YgoProDeck> ygoProDeckValidator)
        {
            _commandValidator = commandValidator;
            _ygoProDeckValidator = ygoProDeckValidator;
        }

        public Task<CommandResult> Handle(UploadYgoProDeckCommand request, CancellationToken cancellationToken)
        {
            var commandResult = new CommandResult();

            var commandValidationResult = _commandValidator.Validate(request);

            if (commandValidationResult.IsValid)
            {
                var ygoProDeck = YgoProDeckHelpers.MapToYgoProDeck(request.Deck);

                var ygoProDeckValidationResult = _ygoProDeckValidator.Validate(ygoProDeck);

                if (ygoProDeckValidationResult.IsValid)
                {
                    ygoProDeck.UserId = request.UserId;
                    ygoProDeck.Name = request.Name;
                }
                else
                {
                    commandResult.Errors = ygoProDeckValidationResult.Errors.Select(err => err.ErrorMessage).ToList();
                }
            }
            else
            {
                commandResult.Errors = commandValidationResult.Errors.Select(err => err.ErrorMessage).ToList();
            }

            return Task.FromResult(commandResult);
        }
    }
}