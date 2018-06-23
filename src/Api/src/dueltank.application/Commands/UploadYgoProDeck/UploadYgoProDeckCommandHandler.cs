using System.Threading;
using System.Threading.Tasks;
using dueltank.core.Helpers;
using FluentValidation;
using MediatR;

namespace dueltank.application.Commands.UploadYgoProDeck
{
    public class UploadYgoProDeckCommandHandler : IRequestHandler<UploadYgoProDeckCommand, CommandResult>
    {
        private readonly IValidator<UploadYgoProDeckCommand> _commandValidator;

        public UploadYgoProDeckCommandHandler(IValidator<UploadYgoProDeckCommand> commandValidator)
        {
            _commandValidator = commandValidator;
        }

        public Task<CommandResult> Handle(UploadYgoProDeckCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResult();

            var commandValidationResult = _commandValidator.Validate(request);

            if (commandValidationResult.IsValid)
            {
                var ygoProDeck = YgoProDeckHelpers.MapToYgoProDeck(request.Deck);
                ygoProDeck.UserId = request.UserId;
                ygoProDeck.Name = request.Name;



            }

            return Task.FromResult(response);
        }
    }
}