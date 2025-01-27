using EShop.Application.Features.Products.DTOs;
using EShop.Application.Repositories;
using EShop.Application.Rules;
using EShop.Application.Storages;
using EShop.Domain.Entities;

namespace EShop.Application.Features.Products.Queries;

public record GetProductByIdQuery(Guid Id) : IRequest<GetProductByIdQuery.Response>
{
    public record Response(
        Guid Id,
        string Name,
        int Quantity,
        decimal UnitPrice,
        DateTime Created,
        DateTime? LastModified,
        IEnumerable<ProductImageListItemDTO> Images);

    internal class Handler : IRequestHandler<GetProductByIdQuery, Response>
    {
        private readonly IMapperReadRepository<Product, Guid> _productReadRepository;
        private readonly IStorage _storage;

        public Handler(IMapperReadRepository<Product, Guid> productReadRepository, IStorage storage)
        {
            _productReadRepository = productReadRepository;
            _storage = storage;
        }

        public async Task<Response> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var response = await _productReadRepository.GetAsync<Response>(request.Id);
            response = response.IfNullThrowNotFound(name: "Product");

            foreach (ProductImageListItemDTO image in response.Images)
                image.Path = $"{_storage.BaseUrl}/{image.Path}";

            return response;
        }
    }
}
