using EShop.Application.Security.Token;
using EShop.Domain.Entities.Identity;
using EShop.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EShop.Application.Features.AppUsers.Commands;

public record RefreshAccessTokenCommand(string RefreshToken) : IRequest<RefreshAccessTokenCommand.Response>
{
    public record Response(AccessToken AccessToken);

    internal class Handler : IRequestHandler<RefreshAccessTokenCommand, Response>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenHandler _tokenHandler;

        public Handler(UserManager<AppUser> userManager, ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<Response> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
        {
            AppUser? user = await _userManager.Users.FirstOrDefaultAsync(user
                => user.RefreshToken == request.RefreshToken && user.RefreshTokenExpiration > DateTime.UtcNow);

            if (user is null)
                throw new RefreshTokenNotValidException();

            return new(_tokenHandler.CreateAccessToken(user));
        }
    }
}
