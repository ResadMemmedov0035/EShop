using EShop.Domain.Entities.Common;

namespace EShop.Application.Repositories;

public interface IWriteRepository<TEntity, TId>
    where TEntity : Entity<TId>, new()
    where TId : struct
{
    Task CreateAsync(TEntity entity, bool saveChanges = true);
    Task CreateAsync(IEnumerable<TEntity> entities, bool saveChanges = true);
    Task UpdateAsync(TEntity entity, bool saveChanges = true);
    Task UpdateAsync(IEnumerable<TEntity> entities, bool saveChanges = true);
    Task DeleteAsync(TId id, bool saveChanges = true);
    Task DeleteAsync(TEntity entity, bool saveChanges = true);
    Task DeleteAsync(IEnumerable<TEntity> entities, bool saveChanges = true);
    Task SaveAsync();
}
