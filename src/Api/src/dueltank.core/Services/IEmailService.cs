using dueltank.Domain.Model;

namespace dueltank.core.Services
{
    public interface IEmailService
    {
        void Send(EmailMessage emailMessage);
    }
}