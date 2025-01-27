using EShop.API.Controllers.Common;
using EShop.Application.Features.Roles.Commands;
using EShop.Application.Features.Roles.Queries;
using EShop.Application.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.API.Controllers;

[Authorize(Roles = ApplicationRoles.Admin)]
[Route("api/[controller]")]
[ApiController]
public class RolesController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetRoleList()
    {
        var response = await Mediator.Send(new GetRoleListQuery());
        return Ok(response);
    }

    [HttpPost("assign-role-to-user")]
    public async Task<IActionResult> AssignRoleToUser(string userEmail, string role)
    {
        await Mediator.Send(new AssignRoleToUserCommand(userEmail, role));
        return Ok();
    }
}
