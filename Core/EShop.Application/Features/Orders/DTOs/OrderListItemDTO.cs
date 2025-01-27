namespace EShop.Application.Features.Orders.DTOs;

public record OrderListItemDTO(Guid Id, string Address, string UserName, DateTime Created);
