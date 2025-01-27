using EShop.Domain.Entities.Common;
using EShop.Domain.Entities.Identity;

namespace EShop.Domain.Entities;

public class Order : Entity<Guid>, IAuditableEntity
{
    public string Address { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public ICollection<OrderItem> Items { get; set; } = null!;
    public OrderStatus Status { get; set; }
}
