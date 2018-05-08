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
        private readonly IEmailConfiguration _emailConfiguration;

        public SendRegistrationEmailCommandHandler(IEmailService emailService, IEmailConfiguration emailConfiguration)
        {
            _emailService = emailService;
            _emailConfiguration = emailConfiguration;
        }
        public Task Handle(SendRegistrationEmailCommand request, CancellationToken cancellationToken)
        {
            var emailMessage = new EmailMessage();

            emailMessage.ToAddresses.Add(new EmailAddress{ Address = request.Email, Name = request.Username });
            emailMessage.FromAddresses.Add(new EmailAddress{ Address = _emailConfiguration.SmtpUsername, Name = "DuelTank" });
            emailMessage.Subject = "DuelTank - Confirm your email";
            var callBackUrl = request.CallBackUrl;
            emailMessage.Content = $"Hi {request.Username}, <p>Thank you for registering with <a href='www.dueltank.com'>Dueltank</a>.</p>Please confirm your" +
                                   $" account by clicking this link: <a href='{callBackUrl}'>{callBackUrl}</a>" +
                                   "<p>Regards,<br/>Dueltank</p>";

            _emailService.Send(emailMessage);

            return Task.CompletedTask;
        }
    }
}