using EShop.Application.Mailing;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace EShop.Infrastructure.Mailing
{
    public class MailService : IMailService
    {
        private readonly MailOptions _options;

        public MailService(IOptions<MailOptions> options)
        {
            _options = options.Value;
        }

        public Task SendMailAsync(string subject, string body, string to, bool isBodyHtml = true) 
        {
            return SendMailAsync(subject, body, new[] { to }, isBodyHtml);
        }

        public async Task SendMailAsync(string subject, string body, string[] to, bool isBodyHtml = true)
        {
            MailMessage message = new()
            {
                Subject = subject,
                Body = body,
                From = new MailAddress(_options.Email, "EShop"),
                IsBodyHtml = isBodyHtml
            };
            foreach (string _to in to)
                message.To.Add(_to);

            using SmtpClient smtp = new(_options.Host, _options.Port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_options.Email, _options.Password)
            };
            await smtp.SendMailAsync(message);
        }
    }
}
