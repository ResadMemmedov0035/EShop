using EShop.API.Controllers.Common;
using EShop.Application.Features.AppUsers.Commands;
using Microsoft.AspNetCore.Mvc;

namespace EShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiControllerBase
    {
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterAppUserCommand command)
        {
            await Mediator.Send(command);
            return CreatedAtAction(nameof(Login), null);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginAppUserCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("refresh-access-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshAccessToken(RefreshAccessTokenCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("send-recovery-email")]
        public async Task<IActionResult> SendRecoveryEmail(SendRecoveryEmailCommand command)
        {
            await Mediator.Send(command);
            return Ok("Mail sent to your account. Please check your mail inbox.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command) 
        {
            await Mediator.Send(command);
            return Ok();
        }
    }
}
