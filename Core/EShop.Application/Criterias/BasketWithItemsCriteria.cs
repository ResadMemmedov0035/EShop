using EShop.Application.Criterias.Common;
using EShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EShop.Application.Criterias
{
    public class BasketWithItemsCriteria : Criteria<Basket>
    {
        private readonly Guid _basketId;

        public BasketWithItemsCriteria(Guid basketId)
        {
            _basketId = basketId;
        }

        public override IQueryable<Basket> Evaluate(IQueryable<Basket> query)
        {
            return query.Include(x => x.Items).Where(x => x.Id == _basketId);
        }
    }
}
