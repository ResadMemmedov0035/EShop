using EShop.Application.Criterias.Common;
using EShop.Domain.Entities;

namespace EShop.Application.Criterias
{
    public class OrderedProductListCriteria : Criteria<Product>
    {
        public override IQueryable<Product> Evaluate(IQueryable<Product> query)
        {
            return query
                .OrderByDescending(x => x.LastModified ?? x.Created)
                .ThenBy(x => x.Name);
        }
    }
}
