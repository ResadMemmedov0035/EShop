namespace EShop.Application.Features.Products.DTOs;

public record ProductListItemDTO(Guid Id, string Name, int Quantity, decimal UnitPrice);
