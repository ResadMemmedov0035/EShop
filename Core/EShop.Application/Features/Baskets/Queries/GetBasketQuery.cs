using EShop.Application.Mapping.Manual;
using EShop.Application.Repositories;
using EShop.Domain.Entities;

namespace EShop.Application.Features.Baskets.Queries;

public record GetBasketQuery(Guid UserId) : IRequest<GetBasketQuery.Response>
{
    public record Response(Guid BasketId, object Items);

    internal class Handler : IRequestHandler<GetBasketQuery, Response?>
    {
        private readonly IReadRepository<Basket, Guid> _basketReadRepository;

        public Handler(IReadRepository<Basket, Guid> basketReadRepository)
        {
            _basketReadRepository = basketReadRepository;
        }

        public async Task<Response?> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            return await _basketReadRepository.GetAsync(b => b.UserId == request.UserId, BasketMapper.ProjectToGetResponse);
        }
    }
}
