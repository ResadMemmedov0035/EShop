using AutoMapper;
using EShop.Domain.Entities;
using EShop.Application.Features.Products.DTOs;
using EShop.Application.Features.Products.Queries;

namespace EShop.Application.Mapping.AutoMapper;

internal class ProductMapperProfile : Profile
{
    public ProductMapperProfile()
    {
        CreateProjection<ProductImage, ProductImageListItemDTO>();

        CreateProjection<Product, GetProductByIdQuery.Response>();

        CreateProjection<Product, ProductListItemDTO>();
    }
}
