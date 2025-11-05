using System.Threading.Tasks;

namespace Pharma263.Application.Contracts.Email
{
    public interface IEmailSender
    {
        Task<bool> SendEmail(string email, string subject, string message);
        Task SendPasswordResetEmailAsync(string email, string resetUrl);
        Task SendPasswordChangedEmailAsync(string email);
    }
}
