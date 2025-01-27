using EShop.Application.Mapping.Manual;
using EShop.Domain.Entities.Identity;
using EShop.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace EShop.Application.Features.AppUsers.Commands;

public record RegisterAppUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    string Password,
    string PasswordConfirm) : IRequest
{
    internal class Handler : IRequestHandler<RegisterAppUserCommand>
    {
        private readonly UserManager<AppUser> _userManager;

        public Handler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task Handle(RegisterAppUserCommand request, CancellationToken cancellationToken)
        {
            IdentityResult result = await _userManager.CreateAsync(request.MapToAppUser(), request.Password);

            if (!result.Succeeded)
            {
                IdentityError error = result.Errors.First();
                throw new UserRegistrationFailedException($"{error.Code}: {error.Description}");
            }
        }
    }
}
