using EShop.Application.Mapping.Manual;
using EShop.Application.Repositories;
using EShop.Application.Rules;
using EShop.Domain.Entities;
using EShop.Domain.Entities.Identity;
using EShop.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace EShop.Application.Features.Orders.Queries;

public record GetOrderByIdQuery(Guid Id, Guid UserId) : IRequest<GetOrderByIdQuery.Response>
{
    public record Response(Guid Id, string Address, string UserName, string UserFullName,
        DateTime Created, DateTime? LastModified, object Products, decimal TotalCost);

    internal class Handler : IRequestHandler<GetOrderByIdQuery, Response>
    {
        private readonly IReadRepository<Order, Guid> _orderReadRepository;

        public Handler(IReadRepository<Order, Guid> orderReadRepository, UserManager<AppUser> userManager)
        {
            _orderReadRepository = orderReadRepository;
        }

        public async Task<Response> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            // If requested order does not belong to specified user
            if (!await _orderReadRepository.AnyAsync(x => x.Id == request.Id && x.UserId == request.UserId))
                throw new OrderDoesntBelongToUserException();

            Response? order = await _orderReadRepository.GetAsync(request.Id, OrderMapper.ProjectToGetByIdResponse);
            return order.IfNullThrowNotFound("Order");
        }
    }
}
