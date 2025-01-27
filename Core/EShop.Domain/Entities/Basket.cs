using EShop.Domain.Entities.Common;
using EShop.Domain.Entities.Identity;

namespace EShop.Domain.Entities;

public class Basket : Entity<Guid>
{
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public ICollection<BasketItem> Items { get; set; } = null!;
}
