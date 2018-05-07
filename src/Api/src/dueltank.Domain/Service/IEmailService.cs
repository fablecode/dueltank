using dueltank.Domain.Model;

namespace dueltank.Domain.Service
{
    public interface IEmailService
    {
        void Send(EmailMessage emailMessage);
    }
}