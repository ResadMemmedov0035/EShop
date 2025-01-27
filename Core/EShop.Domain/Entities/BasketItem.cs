using EShop.Domain.Entities.Common;

namespace EShop.Domain.Entities;

public class BasketItem : Entity<Guid>
{
    public int Quantity { get; set; }
    public Guid BasketId { get; set; }
    public Guid ProductId { get; set; }
    public Basket Basket { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
