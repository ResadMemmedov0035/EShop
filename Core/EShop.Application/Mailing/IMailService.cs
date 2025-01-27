namespace EShop.Application.Mailing;

public interface IMailService
{
    Task SendMailAsync(string subject, string body, string to, bool isBodyHtml = true);
    Task SendMailAsync(string subject, string body, string[] to, bool isBodyHtml = true);
}
