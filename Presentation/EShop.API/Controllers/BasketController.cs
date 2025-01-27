using EShop.API.Controllers.Common;
using EShop.Application.Features.Baskets.Commands;
using EShop.Application.Features.Baskets.DTOs;
using EShop.Application.Features.Baskets.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            var response = await Mediator.Send(new GetBasketQuery(AuthenticatedUserId));
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SaveItemsToBasket([FromBody] IEnumerable<BasketItemDTO> items)
        {
            await Mediator.Send(new SaveItemsToBasketCommand(AuthenticatedUserId, items));
            return Ok();
        }
    }
}

/* [
{
"productId": "8cfbf4a7-017a-4a77-a5c7-68b815748692",
"quantity": 2
},
{
"productId": "b1cc57f8-35f7-4cc8-a50a-ab9a7703260c",
"quantity": 3
}
]*/