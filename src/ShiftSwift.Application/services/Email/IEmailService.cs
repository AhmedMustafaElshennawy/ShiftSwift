

namespace ShiftSwift.Application.services.Email
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string to, string subject, string body);
    }
}