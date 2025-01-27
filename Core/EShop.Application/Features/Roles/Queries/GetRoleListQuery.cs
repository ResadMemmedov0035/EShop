using EShop.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace EShop.Application.Features.Roles.Queries;

public record GetRoleListQuery : IRequest<GetRoleListQuery.Response>
{
    public record Response(IEnumerable<string> Roles);

    internal class Handler : IRequestHandler<GetRoleListQuery, Response>
    {
        private readonly RoleManager<AppRole> _roleManager;

        public Handler(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public Task<Response> Handle(GetRoleListQuery request, CancellationToken cancellationToken)
        {
            Response response = new(_roleManager.Roles.Select(role => role.Name));
            return Task.FromResult(response);
        }
    }
}
