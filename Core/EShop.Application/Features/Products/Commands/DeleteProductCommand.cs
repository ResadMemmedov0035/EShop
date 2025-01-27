using EShop.Application.Repositories;
using EShop.Domain.Entities;

namespace EShop.Application.Features.Products.Commands;

public record DeleteProductCommand(Guid Id) : IRequest
{
    internal class Handler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IWriteRepository<Product, Guid> _productWriteRepository;

        public Handler(IWriteRepository<Product, Guid> productWriteRepository)
        {
            _productWriteRepository = productWriteRepository;
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            await _productWriteRepository.DeleteAsync(request.Id);
        }
    }
}
