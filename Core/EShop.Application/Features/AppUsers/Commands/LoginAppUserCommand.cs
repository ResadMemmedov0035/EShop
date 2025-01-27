using EShop.Application.Rules;
using EShop.Application.Security.Token;
using EShop.Domain.Entities.Identity;
using EShop.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EShop.Application.Features.AppUsers.Commands;

public record LoginAppUserCommand(string EmailOrUserName, string Password) : IRequest<LoginAppUserCommand.Response>
{
    public record Response(AccessToken AccessToken, RefreshToken RefreshToken);

    internal class Handler : IRequestHandler<LoginAppUserCommand, Response>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenHandler _tokenHandler;

        public Handler(UserManager<AppUser> userManager, ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<Response> Handle(LoginAppUserCommand request, CancellationToken cancellationToken)
        {
            AppUser? user = await _userManager.Users.FirstOrDefaultAsync(user
                => user.UserName == request.EmailOrUserName || user.Email == request.EmailOrUserName);

            bool result = await _userManager.CheckPasswordAsync(user.IfNullThrowNotFound("User"), request.Password);

            if (result is false)
                throw new UserLoginFailedException();

            RefreshToken refreshToken = _tokenHandler.CreateRefreshToken();
            (user!.RefreshToken, user.RefreshTokenExpiration!) = refreshToken;
            await _userManager.UpdateAsync(user);

            return new(_tokenHandler.CreateAccessToken(user), refreshToken);
        }
    }
}
