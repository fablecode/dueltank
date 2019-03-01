using dueltank.Domain.Model;

namespace dueltank.Domain.Smtp
{
    public interface ISmtpClient
    {
        void Send(EmailMessage emailMessage);
    }
}