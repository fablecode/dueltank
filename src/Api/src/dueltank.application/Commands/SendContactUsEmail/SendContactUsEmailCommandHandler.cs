using dueltank.core.Services;
using dueltank.Domain.Configuration;
using dueltank.Domain.Model;
using MailKit;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace dueltank.application.Commands.SendContactUsEmail
{
    public class SendContactUsEmailCommandHandler : IRequestHandler<SendContactUsEmailCommand, CommandResult>
    {
        private readonly IEmailService _emailService;
        private readonly IEmailConfiguration _emailConfiguration;

        public SendContactUsEmailCommandHandler(IEmailService emailService, IEmailConfiguration emailConfiguration)
        {
            _emailService = emailService;
            _emailConfiguration = emailConfiguration;
        }

        public Task<CommandResult> Handle(SendContactUsEmailCommand request, CancellationToken cancellationToken)
        {
            var commandResult = new CommandResult();

            const string unableToSendMessage = "Unable to send message.";

            try
            {
                var emailMessage = new EmailMessage();

                emailMessage.FromAddresses.Add(new EmailAddress { Address = request.Email, Name = request.Name });
                emailMessage.ToAddresses.Add(new EmailAddress { Address = _emailConfiguration.SmtpUsername, Name = "DuelTank" });
                emailMessage.Subject = "DuelTank - Contact Us: Message";

                emailMessage.Content = $"Hi DuelTank, <p>A Message from {request.Name}({request.Email}).</p>" +
                                       $"<p>{request.Message}</p>" +
                                       "<p>Regards,<br/>Dueltank</p>"; ;



                _emailService.Send(emailMessage);

                commandResult.IsSuccessful = true;

            }
            catch (ServiceNotConnectedException ex)
            {
                commandResult.Errors = new List<string>{unableToSendMessage};
            }
            catch (ServiceNotAuthenticatedException ex)
            {
                commandResult.Errors = new List<string>{unableToSendMessage};
            }

            return Task.FromResult(commandResult);

        }
    }
}