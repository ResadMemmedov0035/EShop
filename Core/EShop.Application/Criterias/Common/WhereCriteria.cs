using System.Linq.Expressions;

namespace EShop.Application.Criterias.Common
{
    public class WhereCriteria<T> : Criteria<T>
        where T : class, new()
    {
        private readonly Expression<Func<T, bool>> _predicate;

        public WhereCriteria(Expression<Func<T, bool>> predicate)
        {
            _predicate = predicate;
        }

        public override IQueryable<T> Evaluate(IQueryable<T> query)
        {
            return query.Where(_predicate);
        }
    }
}
