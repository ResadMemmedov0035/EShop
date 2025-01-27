using EShop.Application.Criterias;
using EShop.Application.Repositories;
using EShop.Application.Rules;
using EShop.Domain.Entities;
using EShop.Domain.Exceptions;

namespace EShop.Application.Features.Orders.Commands;

public record CreateOrderCommand(Guid BasketId, string Address) : IRequest<Guid>
{
    internal class Handler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IReadRepository<Basket, Guid> _basketReadRepository;
        private readonly IWriteRepository<Order, Guid> _orderWriteRepository;

        public Handler(IReadRepository<Basket, Guid> basketReadRepository, IWriteRepository<Order, Guid> orderWriteRepository)
        {
            _basketReadRepository = basketReadRepository;
            _orderWriteRepository = orderWriteRepository;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            Basket? basket = await _basketReadRepository.GetAsync(new BasketWithItemsCriteria(request.BasketId));
            basket.IfNullThrowNotFound();

            if (basket!.Items.Count == 0)
                throw new BusinessException("Basket is empty.");

            Order order = new()
            {
                Address = request.Address,
                UserId = basket.UserId,
                Items = basket.Items.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList(),
                Status = OrderStatus.Ongoing
            };

            basket.Items.Clear();
            await _orderWriteRepository.CreateAsync(order);
            return order.Id;
        }
    }
}
