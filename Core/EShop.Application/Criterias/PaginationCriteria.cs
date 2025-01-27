using EShop.Application.Criterias.Common;
using EShop.Application.RequestParameters;

namespace EShop.Application.Criterias
{
    public class PaginationCriteria<T> : Criteria<T>
        where T : class, new()
    {
        private readonly Pagination _pagination;

        public PaginationCriteria(Pagination pagination)
        {
            _pagination = pagination;
        }

        public override IQueryable<T> Evaluate(IQueryable<T> query)
        {
            return query
                .Skip(_pagination.Index * _pagination.Size)
                .Take(_pagination.Size);
        }
    }
}
