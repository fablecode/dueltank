using MediatR;

namespace dueltank.application.Commands.SendResetPasswordEmailPassword
{
    public class SendResetPasswordEmailPasswordCommand : IRequest
    {
        public string Email { get; set; }
        public string CallBackUrl { get; set; }
        public string Username { get; set; }
    }
}