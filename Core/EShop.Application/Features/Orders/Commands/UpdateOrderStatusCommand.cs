using EShop.Application.Repositories;
using EShop.Application.Rules;
using EShop.Domain.Entities;

namespace EShop.Application.Features.Orders.Commands;

public record UpdateOrderStatusCommand(Guid OrderId, OrderStatus Status) : IRequest
{
    public class Handler : IRequestHandler<UpdateOrderStatusCommand>
    {
        private readonly IWriteRepository<Order, Guid> _orderWriteRepository;
        private readonly IReadRepository<Order, Guid> _orderReadRepository;

        public Handler(IWriteRepository<Order, Guid> orderWriteRepository, IReadRepository<Order, Guid> orderReadRepository)
        {
            _orderWriteRepository = orderWriteRepository;
            _orderReadRepository = orderReadRepository;
        }

        public async Task Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            Order? order = await _orderReadRepository.GetAsync(request.OrderId);
            order.IfNullThrowNotFound();

            order!.Status = request.Status;

            await _orderWriteRepository.SaveAsync();
        }
    }
}
