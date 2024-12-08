using İdentity.Utilities.EmailHandler.Models;

namespace İdentity.Utilities.EmailHandler.Abstract
{
    public interface IEmailService
    {
        void SendEmail(Message message);
    }
}
