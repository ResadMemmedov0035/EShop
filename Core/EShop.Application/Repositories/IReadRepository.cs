using EShop.Application.Criterias.Common;
using EShop.Domain.Entities.Common;
using System.Linq.Expressions;

namespace EShop.Application.Repositories;

public interface IReadRepository<TEntity, TId>
    where TEntity : Entity<TId>, new()
    where TId : struct
{
    Task<TEntity?> GetAsync(TId id, bool tracking = true);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = true);
    Task<TEntity?> GetAsync(Criteria<TEntity> criteria, bool tracking = true);

    Task<List<TEntity>> GetListAsync(bool tracking = true);
    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = true);
    Task<List<TEntity>> GetListAsync(Criteria<TEntity> criteria, bool tracking = true);

    Task<TDestination?> GetAsync<TDestination>(TId id, Expression<Func<TEntity, TDestination>> projectTo, bool tracking = true);
    Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projectTo, bool tracking = true);
    Task<TDestination?> GetAsync<TDestination>(Criteria<TEntity> criteria, Expression<Func<TEntity, TDestination>> projectTo, bool tracking = true);

    Task<List<TDestination>> GetListAsync<TDestination>(Expression<Func<TEntity, TDestination>> projectTo, bool tracking = true);
    Task<List<TDestination>> GetListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projectTo, bool tracking = true);
    Task<List<TDestination>> GetListAsync<TDestination>(Criteria<TEntity> criteria, Expression<Func<TEntity, TDestination>> projectTo, bool tracking = true);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null);
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);
}
