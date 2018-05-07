using System.Threading;
using System.Threading.Tasks;
using dueltank.Domain.Model;
using dueltank.Domain.Service;
using MediatR;

namespace dueltank.application.Commands.SendRegistrationEmail
{
    public class SendRegistrationEmailCommandHandler : IRequestHandler<SendRegistrationEmailCommand>
    {
        private readonly IEmailService _emailService;

        public SendRegistrationEmailCommandHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }
        public Task Handle(SendRegistrationEmailCommand request, CancellationToken cancellationToken)
        {
            var emailMessage = new EmailMessage();

            emailMessage.ToAddresses.Add(new EmailAddress{ Address = request.Email, Name = request.Username });
            emailMessage.Subject = "DuelTank - Confirm your email";
            emailMessage.Content = $"Hi {request.Username}, <br/>Please confirm your account by clicking this link: <a href='{System.Net.WebUtility.UrlEncode(request.CallBackUrl)}'>link</a>";

            _emailService.Send(emailMessage);

            return Task.CompletedTask;
        }
    }
}