using EShop.Application.Criterias.Common;
using EShop.Application.Repositories;
using EShop.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EShop.Persistence.Repositories
{
    public class EFCoreReadRepository<TEntity, TId> : EFCoreRepository, IReadRepository<TEntity, TId>
        where TEntity : Entity<TId>, new()
        where TId : struct
    {
        public EFCoreReadRepository(DbContext context) 
            : base(context)
        {
            var a = context.Set<Domain.Entities.Identity.AppUser>();

        }

        // NOTE: Could be public/interface method
        protected IQueryable<TEntity> Query(bool tracking)
        {
            var set = Context.Set<TEntity>();
            return tracking ? set : set.AsNoTracking();
        }

        public async Task<TEntity?> GetAsync(TId id, bool tracking = true)
        {
            return await GetAsync(x => x.Id.Equals(id), tracking);
        }

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = true)
        {
            return await Query(tracking).FirstOrDefaultAsync(predicate);
        }

        public async Task<TEntity?> GetAsync(Criteria<TEntity> criteria, bool tracking = true)
        {
            return await criteria.Evaluate(Query(tracking)).FirstOrDefaultAsync();
        }

        public async Task<List<TEntity>> GetListAsync(bool tracking = true)
        {
            return await Query(tracking).ToListAsync();
        }

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = true)
        {
            return await Query(tracking)
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<List<TEntity>> GetListAsync(Criteria<TEntity> criteria, bool tracking = true)
        {
            return await criteria.Evaluate(Query(tracking)).ToListAsync();
        }

        public async Task<TDestination?> GetAsync<TDestination>(TId id, Expression<Func<TEntity, TDestination>> projectTo, bool tracking = true)
        {
            return await GetAsync(x => x.Id.Equals(id), projectTo, tracking);
        }

        public async Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projectTo, bool tracking = true)
        {
            return await Query(tracking)
                .Where(predicate)
                .Select(projectTo)
                .FirstOrDefaultAsync();
        }

        public async Task<TDestination?> GetAsync<TDestination>(Criteria<TEntity> criteria, Expression<Func<TEntity, TDestination>> projectTo, bool tracking = true)
        {
            return await criteria.Evaluate(Query(tracking))
                .Select(projectTo)
                .FirstOrDefaultAsync();
        }

        public async Task<List<TDestination>> GetListAsync<TDestination>(Expression<Func<TEntity, TDestination>> projectTo, bool tracking = true)
        {
            return await Query(tracking)
                .Select(projectTo)
                .ToListAsync();
        }

        public async Task<List<TDestination>> GetListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projectTo, bool tracking = true)
        {
            return await Query(tracking)
                .Where(predicate)
                .Select(projectTo)
                .ToListAsync();
        }

        public async Task<List<TDestination>> GetListAsync<TDestination>(Criteria<TEntity> criteria, Expression<Func<TEntity, TDestination>> projectTo, bool tracking = true)
        {
            return await criteria.Evaluate(Query(tracking))
                .Select(projectTo)
                .ToListAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            return predicate == null
                ? await Context.Set<TEntity>().AnyAsync()
                : await Context.Set<TEntity>().AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            return predicate == null
                ? await Context.Set<TEntity>().CountAsync()
                : await Context.Set<TEntity>().CountAsync(predicate);
        }
    }
}
