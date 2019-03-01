using dueltank.core.Services;
using dueltank.Domain.Configuration;
using dueltank.Domain.Model;
using dueltank.Domain.Smtp;

namespace dueltank.Domain.Service
{
    public class EmailService : IEmailService
    {
        private readonly ISmtpClient _smtpClient;

        public EmailService(ISmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }

        public void Send(EmailMessage emailMessage)
        {
            _smtpClient.Send(emailMessage);
        }
    }
}