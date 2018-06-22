using System;
using MediatR;

namespace dueltank.application.Commands.UploadYgoProDeck
{
    public class UploadYgoProDeckCommand : IRequest<CommandResult>
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Deck { get; set; }
    }
}