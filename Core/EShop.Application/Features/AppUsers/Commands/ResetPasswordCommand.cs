using EShop.Application.Rules;
using EShop.Domain.Entities.Identity;
using EShop.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace EShop.Application.Features.AppUsers.Commands;

public record ResetPasswordCommand(string Email, string Token, string NewPassword, string NewPasswordConfirm) : IRequest
{
    internal class Handler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly UserManager<AppUser> _userManager;

        public Handler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            AppUser? user = await _userManager.FindByEmailAsync(request.Email);
            user.IfNullThrowNotFound("User");

            string token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));

            bool state = await _userManager.VerifyUserTokenAsync(user, 
                _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token);

            if (!state) throw new ResetTokenNotValidException();

            await _userManager.ResetPasswordAsync(user!, token, request.NewPassword);
            await _userManager.UpdateSecurityStampAsync(user); // expire reset password token

        }
    }
}
