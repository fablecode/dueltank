using System.Linq;
using dueltank.Domain.Configuration;
using dueltank.Domain.Model;
using dueltank.Domain.Smtp;
using MimeKit;
using MimeKit.Text;

namespace dueltank.infrastructure.Smtp
{
    public class SmtpClient : ISmtpClient
    {
        private readonly IEmailConfiguration _emailConfiguration;

        public SmtpClient(IEmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        public void Send(EmailMessage emailMessage)
        {
            var message = new MimeMessage();

            message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

            message.Subject = emailMessage.Subject;

            //We will say we are sending HTML. But there are options for plaintext etc. 
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = emailMessage.Content
            };

            //Be careful that the SmtpClient class is the one from Mailkit not the framework!
            using (var emailClient = new MailKit.Net.Smtp.SmtpClient())
            {
                //The last parameter here is to use SSL (Which you should!)
                emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort);

                //Remove any OAuth functionality as we won't be using it. 
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);

                emailClient.Send(message);

                emailClient.Disconnect(true);
            }
        }
    }
}