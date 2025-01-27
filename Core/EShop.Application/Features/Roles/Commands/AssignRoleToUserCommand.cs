using EShop.Application.Rules;
using EShop.Domain.Entities.Identity;
using EShop.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace EShop.Application.Features.Roles.Commands;

public record AssignRoleToUserCommand(string UserEmail, string Role) : IRequest
{
    internal class Handler : IRequestHandler<AssignRoleToUserCommand>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public Handler(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
        {
            AppUser? user = await _userManager.FindByEmailAsync(request.UserEmail);
            user!.IfNullThrowNotFound();

            if (await _userManager.IsInRoleAsync(user, request.Role))
                throw new AuthenticationException($"The user has already role: {request.Role}");

            if (!await _roleManager.RoleExistsAsync(request.Role))
                throw new AuthenticationException($"There is no such role as: {request.Role}");

            await _userManager.AddToRoleAsync(user, request.Role);
        }
    }
}
