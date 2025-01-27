using Microsoft.EntityFrameworkCore;

namespace EShop.Persistence.Repositories
{
    public abstract class EFCoreRepository
    {
        protected DbContext Context { get; }

        public EFCoreRepository(DbContext context)
            => Context = context;
    }
}
