using EShop.API.Controllers.Common;
using EShop.Application.Features.ProductImages.Commands;
using EShop.Application.Features.Products.Commands;
using EShop.Application.Features.Products.Queries;
using EShop.Application.RequestParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ApiControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList([FromQuery] Pagination pagination)
        {
            GetProductListQuery.Response response = await Mediator.Send(new GetProductListQuery(pagination));
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreateProductCommand command)
        {
            Guid id = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, new { ProductId = id });
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            GetProductByIdQuery.Response response = await Mediator.Send(new GetProductByIdQuery(id));
            return Ok(response);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, UpdateProductCommand command)
        {
            command.Id = id;
            await Mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteProductCommand(id));
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("{id:guid}/upload-images")]
        public async Task<IActionResult> Upload(Guid id, [FromForm] IFormFileCollection files)
        {
            //38c385d5-ebc8-447d-86e5-f28277055498
            var response = await Mediator.Send(new UploadProductImagesCommand(id, files));
            return Ok(response);
        }
    }
}
