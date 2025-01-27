using EShop.Application.Features.Baskets.Queries;
using EShop.Domain.Entities;
using System.Linq.Expressions;

namespace EShop.Application.Mapping.Manual;

public static class BasketMapper
{
    public static readonly Expression<Func<Basket, GetBasketQuery.Response>> ProjectToGetResponse
        = basket => new
        (
            basket.Id,
            basket.Items.Select(item => new
            {
                item.Product.Name,
                item.Quantity
            })
        );
}
