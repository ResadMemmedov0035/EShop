namespace EShop.Application.Features.Products.DTOs;

public record ProductImageListItemDTO(Guid Id, string FileName, string Path, bool IsShowcase)
{
    public string Path { get; set; } = Path;
}
