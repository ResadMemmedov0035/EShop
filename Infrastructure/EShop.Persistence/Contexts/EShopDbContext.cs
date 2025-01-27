using EShop.Application.Security;
using EShop.Domain.Entities;
using EShop.Domain.Entities.Common;
using EShop.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EShop.Persistence.Contexts
{
    public class EShopDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public EShopDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Product> Products { get; set; } = null!;

        public DbSet<Domain.Entities.File> Files { get; set; } = null!;
        public DbSet<ProductImage> ProductImages { get; set; } = null!;

        public DbSet<Basket> Baskets { get; set; } = null!;
        public DbSet<BasketItem> BasketItems { get; set; } = null!;

        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Basket>()
                .HasIndex(x => x.UserId)
                .IsUnique(true);

            builder.Entity<AppRole>()
                .HasData(new AppRole { Id = Guid.NewGuid(), Name = ApplicationRoles.Admin, NormalizedName = ApplicationRoles.Admin.ToUpper() });
            
            base.OnModelCreating(builder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
            {
                _ = entry.State switch
                {
                    EntityState.Added => entry.Entity.Created = DateTime.UtcNow,
                    EntityState.Modified => entry.Entity.LastModified = DateTime.UtcNow,
                    _ => null
                };
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
