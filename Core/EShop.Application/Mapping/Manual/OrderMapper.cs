using EShop.Application.Features.Orders.DTOs;
using EShop.Application.Features.Orders.Queries;
using EShop.Domain.Entities;
using System.Linq.Expressions;

namespace EShop.Application.Mapping.Manual;

public static class OrderMapper
{
    public static readonly Expression<Func<Order, GetOrderByIdQuery.Response>> ProjectToGetByIdResponse
        = order => new
        (
            order.Id,
            order.Address,
            order.User.UserName,
            $"{order.User.FirstName} {order.User.LastName}",
            order.Created,
            order.LastModified,
            order.Items.Select(item => new
            {
                item.Product.Id,
                item.Product.Name,
                item.Product.UnitPrice,
                item.Quantity
            }),
            order.Items.Sum(item => item.Quantity * item.Product.UnitPrice)
        );

    public static readonly Expression<Func<Order, OrderListItemDTO>> ProjectToListItem
        = order => new
        (
            order.Id,
            order.Address,
            order.User.UserName,
            order.Created
        );
}
