using EShop.Application.Criterias;
using EShop.Application.Features.Orders.DTOs;
using EShop.Application.Mapping.Manual;
using EShop.Application.Repositories;
using EShop.Application.RequestParameters;
using EShop.Domain.Entities;

namespace EShop.Application.Features.Orders.Queries;

public record GetOrderListQuery(Pagination Pagination) : IRequest<GetOrderListQuery.Response>
{
    public record Response(IEnumerable<OrderListItemDTO> Items);

    internal class Handler : IRequestHandler<GetOrderListQuery, Response>
    {
        private readonly IReadRepository<Order, Guid> _orderReadRepository;

        public Handler(IReadRepository<Order, Guid> orderReadRepository)
        {
            _orderReadRepository = orderReadRepository;
        }

        public async Task<Response> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
        {
            var criteria = new PaginationCriteria<Order>(request.Pagination);
            var list = await _orderReadRepository.GetListAsync(criteria, OrderMapper.ProjectToListItem);
            return new(list);
        }
    }
}
