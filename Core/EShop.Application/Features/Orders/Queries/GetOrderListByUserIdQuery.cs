using EShop.Application.Criterias;
using EShop.Application.Criterias.Common;
using EShop.Application.Features.Orders.DTOs;
using EShop.Application.Mapping.Manual;
using EShop.Application.Repositories;
using EShop.Application.RequestParameters;
using EShop.Domain.Entities;

namespace EShop.Application.Features.Orders.Queries;

public record GetOrderListByUserIdQuery(Pagination Pagination, Guid UserId) : IRequest<GetOrderListByUserIdQuery.Response>
{
    public record Response(IEnumerable<OrderListItemDTO> Items);

    internal class Handler : IRequestHandler<GetOrderListByUserIdQuery, Response>
    {
        private readonly IReadRepository<Order, Guid> _orderReadRepository;

        public Handler(IReadRepository<Order, Guid> orderReadRepository)
        {
            _orderReadRepository = orderReadRepository;
        }

        public async Task<Response> Handle(GetOrderListByUserIdQuery request, CancellationToken cancellationToken)
        {
            var criteria = Criteria<Order>.Collect(
                new OrderedOrderListCriteria(request.UserId),
                new PaginationCriteria<Order>(request.Pagination));

            var list = await _orderReadRepository.GetListAsync(criteria, OrderMapper.ProjectToListItem);
            return new(list);
        }
    }
}
