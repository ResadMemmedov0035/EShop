using EShop.Application.Mapping.Manual;
using EShop.Application.Repositories;
using EShop.Domain.Entities;

namespace EShop.Application.Features.Products.Commands;

public record CreateProductCommand(string Name, int Quantity, decimal UnitPrice) : IRequest<Guid>
{
    internal class Handler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IWriteRepository<Product, Guid> _productWriteRepository;

        public Handler(IWriteRepository<Product, Guid> productWriteRepository)
        {
            _productWriteRepository = productWriteRepository;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = request.MapToProduct();
            await _productWriteRepository.CreateAsync(product);
            return product.Id;
        }
    }
}
