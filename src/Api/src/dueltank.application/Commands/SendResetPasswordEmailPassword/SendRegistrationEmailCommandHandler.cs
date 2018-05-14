﻿using System.Threading;
using System.Threading.Tasks;
using dueltank.Domain.Model;
using dueltank.Domain.Service;
using MediatR;

namespace dueltank.application.Commands.SendResetPasswordEmailPassword
{
    public class SendRegistrationEmailCommandHandler : IRequestHandler<SendResetPasswordEmailPasswordCommand>
    {
        private readonly IEmailService _emailService;
        private readonly IEmailConfiguration _emailConfiguration;

        public SendRegistrationEmailCommandHandler(IEmailService emailService, IEmailConfiguration emailConfiguration)
        {
            _emailService = emailService;
            _emailConfiguration = emailConfiguration;
        }
        public Task Handle(SendResetPasswordEmailPasswordCommand request, CancellationToken cancellationToken)
        {
            var emailMessage = new EmailMessage();

            emailMessage.ToAddresses.Add(new EmailAddress { Address = request.Email, Name = request.Username });
            emailMessage.FromAddresses.Add(new EmailAddress { Address = _emailConfiguration.SmtpUsername, Name = "DuelTank" });
            emailMessage.Subject = "DuelTank - Reset Password";
            var callBackUrl = request.CallBackUrl;
            emailMessage.Content = $"Hi {request.Username}, <p>You've recently requested to reset your password for your <a href='http://www.dueltank.com'>Dueltank</a> account.</p>" +
                                   $"<p>Please reset your password by clicking here: <a href=\'{callBackUrl}\'>Reset your password</a></p>" +
                                   "<p>Regards,<br/>Dueltank</p>";

            _emailService.Send(emailMessage);

            return Task.CompletedTask;
        }
    }
}