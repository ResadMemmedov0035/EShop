using EShop.Application.Mapping.Manual;
using EShop.Application.Repositories;
using EShop.Application.Rules;
using EShop.Domain.Entities;
using System.Text.Json.Serialization;

namespace EShop.Application.Features.Products.Commands;

public record UpdateProductCommand(Guid Id, string Name, int Quantity, decimal UnitPrice) : IRequest
{
    [JsonIgnore]
    public Guid Id { get; set; } = Id;

    internal class Handler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IWriteRepository<Product, Guid> _productWriteRepository;
        private readonly IReadRepository<Product, Guid> _productReadRepository;

        public Handler(IWriteRepository<Product, Guid> productWriteRepository,
            IReadRepository<Product, Guid> productReadRepository)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
        }

        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            Product? product = await _productReadRepository.GetAsync(request.Id, tracking: true);
            request.MapToProduct(product.IfNullThrowNotFound());
            await _productWriteRepository.SaveAsync();
        }
    }
}
