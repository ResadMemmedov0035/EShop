using EShop.API.Controllers.Common;
using EShop.Application.Features.ProductImages.Commands;
using Microsoft.AspNetCore.Mvc;

namespace EShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImagesController : ApiControllerBase
    {
        [HttpPost("{id:guid}/set-showcase")]
        public async Task<IActionResult> SetShowcase(Guid id)
        {
            var response = await Mediator.Send(new SetProductShowcaseImageCommand(id));
            return Ok(response);
        }
    }
}
