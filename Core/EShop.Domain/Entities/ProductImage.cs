namespace EShop.Domain.Entities;

public class ProductImage : File
{
    public bool IsShowcase { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
