using EShop.Application.Features.Products.Commands;
using EShop.Application.Features.Products.DTOs;
using EShop.Application.Features.Products.Queries;
using EShop.Domain.Entities;
using System.Linq.Expressions;

namespace EShop.Application.Mapping.Manual;

public static class ProductMapper
{
    public static readonly Expression<Func<Product, GetProductByIdQuery.Response>> ProjectToGetByIdResponse
        = p => new(p.Id,
            p.Name,
            p.Quantity,
            p.UnitPrice,
            p.Created,
            p.LastModified,
            p.Images.Select(i => new ProductImageListItemDTO(i.Id, i.FileName, i.Path, i.IsShowcase)));

    public static Product MapToProduct(this CreateProductCommand command)
        => new()
        {
            Name = command.Name,
            Quantity = command.Quantity,
            UnitPrice = command.UnitPrice
        };

    public static void MapToProduct(this UpdateProductCommand command, Product product)
    {
        product.Id = command.Id;
        product.Name = command.Name;
        product.Quantity = command.Quantity;
        product.UnitPrice = command.UnitPrice;
    }
}
