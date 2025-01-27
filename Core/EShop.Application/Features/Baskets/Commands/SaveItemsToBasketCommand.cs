using EShop.Application.Features.Baskets.DTOs;
using EShop.Application.Repositories;
using EShop.Domain.Entities;

namespace EShop.Application.Features.Baskets.Commands;

public record SaveItemsToBasketCommand(Guid UserId, IEnumerable<BasketItemDTO> BasketItems) : IRequest
{
    internal class Handler : IRequestHandler<SaveItemsToBasketCommand>
    {
        private readonly IReadRepository<Basket, Guid> _basketReadRepository;
        private readonly IWriteRepository<Basket, Guid> _basketWriteRepository;

        public Handler(IReadRepository<Basket, Guid> basketReadRepository, IWriteRepository<Basket, Guid> basketWriteRepository)
        {
            _basketReadRepository = basketReadRepository;
            _basketWriteRepository = basketWriteRepository;
        }

        public async Task Handle(SaveItemsToBasketCommand request, CancellationToken cancellationToken)
        {
            Basket? basket = await _basketReadRepository.GetAsync(b => b.UserId == request.UserId);

            if (basket is null)
            {
                basket = new() { UserId = request.UserId };
                await _basketWriteRepository.CreateAsync(basket);
            }

            basket.Items = new List<BasketItem>();

            foreach (BasketItemDTO item in request.BasketItems)
            {
                basket.Items.Add(new()
                {
                    BasketId = basket.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }
            await _basketWriteRepository.SaveAsync();
        }
    }
}
