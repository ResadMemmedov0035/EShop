using EShop.Application.Criterias.Common;
using EShop.Domain.Entities;

namespace EShop.Application.Criterias;

public class OrderedOrderListCriteria : Criteria<Order>
{
    private readonly Guid _userId;

    public OrderedOrderListCriteria(Guid userId)
    {
        _userId = userId;
    }

    public override IQueryable<Order> Evaluate(IQueryable<Order> query)
    {
        return query
            .OrderByDescending(x => x.LastModified ?? x.Created)
            .Where(x => x.UserId == _userId);
    }
}
