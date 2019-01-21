using System.Collections.Generic;
using dueltank.core.Services;
using dueltank.Domain.Configuration;
using dueltank.Domain.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MailKit;

namespace dueltank.application.Commands.SendRegistrationEmail
{
    public class SendRegistrationEmailCommandHandler : IRequest<SendRegistrationEmailCommand>
    {
        private readonly IEmailService _emailService;
        private readonly IEmailConfiguration _emailConfiguration;

        public SendRegistrationEmailCommandHandler(IEmailService emailService, IEmailConfiguration emailConfiguration)
        {
            _emailService = emailService;
            _emailConfiguration = emailConfiguration;
        }
        public Task<CommandResult> Handle(SendRegistrationEmailCommand request, CancellationToken cancellationToken)
        {
            const string unableToSendMessage = "Unable to send new registration message.";

            var commandResult = new CommandResult();

            try
            {
                var emailMessage = new EmailMessage();

                emailMessage.ToAddresses.Add(new EmailAddress { Address = request.Email, Name = request.Username });
                emailMessage.FromAddresses.Add(new EmailAddress { Address = _emailConfiguration.SmtpUsername, Name = "DuelTank" });
                emailMessage.Subject = "DuelTank - Confirm your email";
                var callBackUrl = request.CallBackUrl;
                emailMessage.Content = $"Hi {request.Username}, <p>Thank you for registering with <a href='http://www.dueltank.com'>Dueltank</a>.</p>Please confirm your" +
                                       $" account by clicking this link: <a href='{callBackUrl}'>Activate your account</a>" +
                                       "<p>Regards,<br/>Dueltank</p>";

                _emailService.Send(emailMessage);
            }
            catch (ServiceNotConnectedException ex)
            {
                commandResult.Errors = new List<string> { unableToSendMessage };
            }
            catch (ServiceNotAuthenticatedException ex)
            {
                commandResult.Errors = new List<string> { unableToSendMessage };
            }



            return Task.FromResult(commandResult);
        }
    }
}