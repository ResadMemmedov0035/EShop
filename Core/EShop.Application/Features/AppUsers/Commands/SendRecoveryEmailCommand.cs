using EShop.Application.Mailing;
using EShop.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace EShop.Application.Features.AppUsers.Commands;

public record SendRecoveryEmailCommand(string DestinationMail) : IRequest
{
    internal class Handler : IRequestHandler<SendRecoveryEmailCommand>
    {
        private readonly IMailService _mailService;
        private readonly UserManager<AppUser> _userManager;

        public Handler(IMailService mailService, UserManager<AppUser> userManager)
        {
            _mailService = mailService;
            _userManager = userManager;
        }

        public async Task Handle(SendRecoveryEmailCommand request, CancellationToken cancellationToken)
        {
            AppUser? user = await _userManager.FindByEmailAsync(request.DestinationMail);
            if (user is null) return;

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            
            await _mailService.SendMailAsync(
                subject: "EShop account recovery",
                body: @$"<h3>
                         In a real-life project, there would be a link directing you to the application's front-end page.
                         On this page, you would enter your new password, click 'Reset', and then should redirect to the login page.
                         This link would have the user's email and encoded token in the query. (eg: eshop.com/reset-password?email=...&token=...)
                         But this project has no front end now. So you must reset your password manually via Swagger UI.
                        </h3>
                         </br></br>
                        <strong><h3>Use this token to reset the password (See: api/auth/reset-password):</strong></h3> {token}",
                to: request.DestinationMail);
        }
    }
}
