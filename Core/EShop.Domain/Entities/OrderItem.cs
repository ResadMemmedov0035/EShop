using EShop.Domain.Entities.Common;

namespace EShop.Domain.Entities;

public class OrderItem : Entity<Guid>
{
    public int Quantity { get; set; }
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public Order Order { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
