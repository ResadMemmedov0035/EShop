using EShop.Application.Criterias;
using EShop.Application.Criterias.Common;
using EShop.Application.Features.Products.DTOs;
using EShop.Application.Repositories;
using EShop.Application.RequestParameters;
using EShop.Domain.Entities;

namespace EShop.Application.Features.Products.Queries;

public record GetProductListQuery(Pagination Pagination) : IRequest<GetProductListQuery.Response>
{
    public record Response(IList<ProductListItemDTO> Items, int Count, int TotalCount);

    internal class Handler : IRequestHandler<GetProductListQuery, Response>
    {
        private readonly IMapperReadRepository<Product, Guid> _productReadRepository;

        public Handler(IMapperReadRepository<Product, Guid> productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }

        public async Task<Response> Handle(GetProductListQuery request, CancellationToken cancellationToken)
        {
            var criteria = Criteria<Product>.Collect(
                new OrderedProductListCriteria(),
                new PaginationCriteria<Product>(request.Pagination)
            );

            var list = await _productReadRepository.GetListAsync<ProductListItemDTO>(criteria);
            int totalCount = await _productReadRepository.CountAsync();
            return new(list, list.Count, totalCount);
        }
    }
}
