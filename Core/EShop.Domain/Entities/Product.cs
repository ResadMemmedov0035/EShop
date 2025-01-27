using EShop.Domain.Entities.Common;

namespace EShop.Domain.Entities;

public class Product : Entity<Guid>, IAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
    public ICollection<ProductImage> Images { get; set; } = null!;
}
