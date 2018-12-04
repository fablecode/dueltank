using MediatR;

namespace dueltank.application.Commands.SendContactUsEmail
{
    public class SendContactUsEmailCommand : IRequest<CommandResult>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}