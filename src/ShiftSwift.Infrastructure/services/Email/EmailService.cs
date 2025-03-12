using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using ShiftSwift.Application.services.Email;
using ShiftSwift.Application.settings;


namespace ShiftSwift.Infrastructure.services.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> emailSettings) => _emailSettings = emailSettings.Value;
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_emailSettings.Email),
                Subject = subject,
            };

            email.To.Add(MailboxAddress.Parse(to));
            var builder = new BodyBuilder { HtmlBody = body };
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_emailSettings.DisplayName, _emailSettings.Email));

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
