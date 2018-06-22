using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace dueltank.application.Commands.UploadYgoProDeck
{
    public class UploadYgoProDeckCommandHandler : IRequestHandler<UploadYgoProDeckCommand, CommandResult>
    {
        public Task<CommandResult> Handle(UploadYgoProDeckCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}