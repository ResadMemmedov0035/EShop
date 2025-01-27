using EShop.Application.Criterias.Common;
using EShop.Domain.Entities.Common;
using System.Linq.Expressions;

namespace EShop.Application.Repositories;

public interface IMapperReadRepository<TEntity, TId> : IReadRepository<TEntity, TId>
    where TEntity : Entity<TId>, new()
    where TId : struct
{
    Task<TDestination?> GetAsync<TDestination>(TId id, bool tracking = true);
    Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, bool tracking = true);
    Task<TDestination?> GetAsync<TDestination>(Criteria<TEntity> criteria, bool tracking = true);

    Task<List<TDestination>> GetListAsync<TDestination>(bool tracking = true);
    Task<List<TDestination>> GetListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, bool tracking = true);
    Task<List<TDestination>> GetListAsync<TDestination>(Criteria<TEntity> criteria, bool tracking = true);
}
