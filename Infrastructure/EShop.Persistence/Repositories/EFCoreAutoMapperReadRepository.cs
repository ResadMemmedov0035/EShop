using AutoMapper.QueryableExtensions;
using EShop.Application.Criterias.Common;
using EShop.Application.Repositories;
using EShop.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EShop.Persistence.Repositories
{
    public class EFCoreAutoMapperReadRepository<TEntity, TId> : EFCoreReadRepository<TEntity, TId>, IMapperReadRepository<TEntity, TId>
        where TEntity : Entity<TId>, new()
        where TId : struct
    {
        private readonly AutoMapper.IConfigurationProvider _mapperConfiguration;

        public EFCoreAutoMapperReadRepository(DbContext context, AutoMapper.IConfigurationProvider mapperConfiguration)
            : base(context)
        {
            _mapperConfiguration = mapperConfiguration;
        }

        public async Task<TDestination?> GetAsync<TDestination>(TId id, bool tracking = true)
        {
            return await GetAsync<TDestination>(x => x.Id.Equals(id), tracking);
        }

        public async Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, bool tracking = true)
        {
            return await Query(tracking)
                .Where(predicate)
                .ProjectTo<TDestination>(_mapperConfiguration)
                .FirstOrDefaultAsync();
        }

        public async Task<TDestination?> GetAsync<TDestination>(Criteria<TEntity> criteria, bool tracking = true)
        {
            return await criteria.Evaluate(Query(tracking))
                .ProjectTo<TDestination>(_mapperConfiguration)
                .FirstOrDefaultAsync();
        }

        public async Task<List<TDestination>> GetListAsync<TDestination>(bool tracking = true)
        {
            return await Query(tracking)
                .ProjectTo<TDestination>(_mapperConfiguration)
                .ToListAsync();
        }

        public async Task<List<TDestination>> GetListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, bool tracking = true)
        {
            return await Query(tracking)
                .Where(predicate)
                .ProjectTo<TDestination>(_mapperConfiguration)
                .ToListAsync();
        }

        public async Task<List<TDestination>> GetListAsync<TDestination>(Criteria<TEntity> criteria, bool tracking = true)
        {
            return await criteria.Evaluate(Query(tracking))
                .ProjectTo<TDestination>(_mapperConfiguration)
                .ToListAsync();
        }
    }
}
