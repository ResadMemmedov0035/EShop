using EShop.API.Controllers.Common;
using EShop.Application.Features.Orders.Commands;
using EShop.Application.Features.Orders.Queries;
using EShop.Application.RequestParameters;
using EShop.Application.Security;
using EShop.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ApiControllerBase
    {
        [HttpGet("all")]
        [Authorize(Roles = ApplicationRoles.Admin)]
        public async Task<IActionResult> GetAllOrders([FromQuery] Pagination pagination)
        {
            var response = await Mediator.Send(new GetOrderListQuery(pagination));
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderList([FromQuery] Pagination pagination)
        {
            var response = await Mediator.Send(new GetOrderListByUserIdQuery(pagination, AuthenticatedUserId));
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetOrderById([FromRoute] Guid id)
        {
            var response = await Mediator.Send(new GetOrderByIdQuery(id, AuthenticatedUserId));
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderCommand command)
        {
            Guid id = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetOrderById), new { id }, new { OrderId = id });
        }

        [HttpPost("complete/{id:guid}")]
        public async Task<IActionResult> CompleteOrder(Guid id)
        {
            await Mediator.Send(new UpdateOrderStatusCommand(id, OrderStatus.Completed));
            return Ok();
        }

        [HttpPost("cancel/{id:guid}")]
        public async Task<IActionResult> CancelOrder(Guid id)
        {
            await Mediator.Send(new UpdateOrderStatusCommand(id, OrderStatus.Canceled));
            return Ok();
        }
    }
}
