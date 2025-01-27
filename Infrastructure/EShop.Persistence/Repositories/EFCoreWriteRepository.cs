using EShop.Application.Repositories;
using EShop.Application.Rules;
using EShop.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace EShop.Persistence.Repositories
{
    public class EFCoreWriteRepository<TEntity, TId> : EFCoreRepository, IWriteRepository<TEntity, TId>
        where TEntity : Entity<TId>, new()
        where TId : struct
    {
        public EFCoreWriteRepository(DbContext context)
            : base(context)
        { }

        public async Task CreateAsync(TEntity entity, bool saveChanges = true)
        {
            await Context.AddAsync(entity);
            if (saveChanges)
                await Context.SaveChangesAsync();
        }

        public async Task CreateAsync(IEnumerable<TEntity> entities, bool saveChanges = true)
        {
            await Context.AddRangeAsync(entities);
            if (saveChanges)
                await Context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity, bool saveChanges = true)
        {
            Context.Update(entity);
            if (saveChanges)
                await Context.SaveChangesAsync();
        }

        public async Task UpdateAsync(IEnumerable<TEntity> entities, bool saveChanges = true)
        {
            Context.UpdateRange(entities);
            if (saveChanges)
                await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TId id, bool saveChanges = true)
        {
            TEntity? entity = await Context.FindAsync<TEntity>(id);
            await DeleteAsync(entity.IfNullThrowNotFound(), saveChanges);
        }

        public async Task DeleteAsync(TEntity entity, bool saveChanges = true)
        {
            Context.Remove(entity);
            if (saveChanges)
                await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(IEnumerable<TEntity> entities, bool saveChanges = true)
        {
            Context.RemoveRange(entities);
            if (saveChanges)
                await Context.SaveChangesAsync();
        }

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
